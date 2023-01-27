using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Game_States;
using _Scripts.Levels;
using _Scripts.Money_Logic;
using _Scripts.Train;
using _Scripts.UI.Upgrade;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts.Units
{
    [RequireComponent(typeof(ChunkMovement))]
    public class ZombieManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform creatingPositionFrom;
        [SerializeField] private Transform creatingPositionTo;
        [SerializeField] private Zombie usualZombie;
        [SerializeField] private Zombie fastZombie;
        [SerializeField] private Zombie bigZombie;
        
        private readonly List<Zombie> _aliveZombies = new ();

        private List<Wave> _zombiesWaves;
        private int _zombiesLeft;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private UpgradeMenu _upgradeMenu;
        [Inject] private MoneyWallet _moneyWallet;
        [Inject] private Train.Train _train;
        [Inject] private DiContainer _diContainer;

        private ChunkMovement _chunkMovement;
        private Coroutine _creatingCoroutine;

        public List<Zombie> DeadZombies { get; } = new ();
        public float WholeHpSum { get; private set; }
        public float LostHp { get; private set; }
        
        public event Action OnHpChanged;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _chunkMovement = GetComponent<ChunkMovement>();
        }
        private void Start()
        {
            _gameStateManager.AttackStarted += StartCreatingZombies;
            _gameStateManager.AttackStarted += () => { _chunkMovement.ChangeState(true);};
            _gameStateManager.Fail += ZombieWin;
        }
        #endregion

        #region Init
        public void Init(List<Wave> zombiesWaves)
        {
            _zombiesWaves = zombiesWaves;

            foreach (var subWave in _zombiesWaves.SelectMany(zombieWave => zombieWave.subWaves))
            {
                _zombiesLeft += subWave.ZombieCount.UsualZombieCount;
                _zombiesLeft += subWave.ZombieCount.FastZombieCount;
                _zombiesLeft += subWave.ZombieCount.BigZombieCount;

                WholeHpSum += subWave.ZombieCount.UsualZombieCount * usualZombie.StartHp(_levelManager.CurrentLevel);
                WholeHpSum += subWave.ZombieCount.FastZombieCount * fastZombie.StartHp(_levelManager.CurrentLevel);
                WholeHpSum += subWave.ZombieCount.BigZombieCount * bigZombie.StartHp(_levelManager.CurrentLevel);
            }
        }

        public void InitMotion(Chunk firstChunk)
        {
            _chunkMovement.Init(firstChunk);
            _chunkMovement.SetSpeed(_train.TrainSpeed);
        }
        #endregion

        private void UpdateLostHp(int deltaHp)
        {
            LostHp += deltaHp;
            OnHpChanged?.Invoke();
        }

        #region Zombie Creating
        private void StartCreatingZombies()
        {
            _creatingCoroutine = StartCoroutine(CreateZombies());
        }

        private IEnumerator CreateZombies()
        {
            foreach (var zombieWave in _zombiesWaves)
            {
                foreach (var subWave in zombieWave.subWaves)
                {
                    var usualZombieLeft = subWave.ZombieCount.UsualZombieCount;
                    var fastZombieLeft = subWave.ZombieCount.FastZombieCount;
                    var bigZombieLeft = subWave.ZombieCount.BigZombieCount;
                    var zombiesLeft = usualZombieLeft + fastZombieLeft + bigZombieLeft;
                    
                    while (zombiesLeft-- > 0)
                    {
                        ZombieType zombieType;
                        while (true)
                        {
                            zombieType = (ZombieType) Random.Range(0, (int) ZombieType.CountTypes);
                            if (zombieType == ZombieType.Usual && usualZombieLeft > 0)
                            {
                                usualZombieLeft--;
                                break;
                            }
                            if (zombieType == ZombieType.Fast && fastZombieLeft > 0)
                            {
                                fastZombieLeft--;
                                break;
                            }
                            if (zombieType == ZombieType.Big && bigZombieLeft > 0)
                            {
                                bigZombieLeft--;
                                break;
                            }
                        }
                        
                        CreateZombie(GetTargetZombie(zombieType));
                        yield return new WaitForSeconds(subWave.TimeBetweenZombie);
                    }
                    yield return new WaitForSeconds(subWave.TimeBetweenWaves);
                }
                yield return new WaitForSeconds(zombieWave.TimeBetweenWaves);
            }
        }

        private void CreateZombie(Zombie targetZombie)
        {
            var zombie = _diContainer.InstantiatePrefabForComponent<Zombie>(targetZombie, transform);
            
            zombie.Init(_chunkMovement.CurrentChunk, ZombieDelta);
            
            zombie.DeadEvent += RemoveZombie;
            zombie.GetDamageEvent += UpdateLostHp;
            zombie.GetDamageEvent += value =>
            {
                var reward = value * _upgradeMenu.IncomeCoefficient;
                _moneyWallet.Add((int) reward);
            };
            
            _aliveZombies.Add(zombie);
        }

        private Zombie GetTargetZombie(ZombieType zombieType)
        {
            var targetZombie = usualZombie;
            switch (zombieType)
            {
                case ZombieType.Usual:
                    break;
                case ZombieType.Fast:
                    targetZombie = fastZombie;
                    break;
                case ZombieType.Big:
                    targetZombie = bigZombie;
                    break;
                case ZombieType.CountTypes:
                default:
                    throw new ArgumentOutOfRangeException(nameof(zombieType), zombieType, null);
            }
            return targetZombie;
        }

        private float ZombieDelta => Random.Range(creatingPositionFrom.position.x,
            creatingPositionTo.position.x);
        #endregion

        public Zombie GetNearestZombie(Transform fromTransform)
        {
            Zombie targetZombie = null;
            var minDistance = 1e9f;
            
            foreach (var zombie in _aliveZombies)
            {
                var currentDistance = Vector3.Distance(fromTransform.position, zombie.ShootPoint.position);
                if (!(currentDistance < minDistance)) continue;
                minDistance = currentDistance;
                targetZombie = zombie;
            }

            return targetZombie;
        }
        
        private void RemoveZombie(Zombie zombie)
        {
            _aliveZombies.Remove(zombie);
            DeadZombies.Add(zombie);
            _zombiesLeft--;
            
            if (_zombiesLeft <= 0 && _aliveZombies.Count == 0)
            {
                _gameStateManager.ChangeState(GameState.Victory);
            }
        }
        
        private void ZombieWin()
        {
            _chunkMovement.ChangeState(false);
            
            if (_creatingCoroutine != null) 
                StopCoroutine(_creatingCoroutine);
            
            foreach (var zombie in _aliveZombies)
            {
                zombie.ChangeState(UnitState.Victory);
            }
        }
    }
}
