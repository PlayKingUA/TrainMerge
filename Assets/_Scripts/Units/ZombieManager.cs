using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Game_States;
using _Scripts.Levels;
using _Scripts.Money_Logic;
using _Scripts.Train;
using _Scripts.UI.Displays;
using _Scripts.UI.Upgrade;
using _Scripts.Weapons;
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
        [SerializeField] private Transform zombieTransform;
        [SerializeField] private Zombie[] usualZombie;
        [SerializeField] private Zombie fastZombie;
        [SerializeField] private Zombie bigZombie;
        [Space]
        [SerializeField] private ZombieTable zombieTable;
        [SerializeField] private float messageTimeBeforeLastWave = 2f;
        [SerializeField] private float tapMessageDelayAfterLastWave = 0.5f;

        private List<Wave> _zombiesWaves;
        private int _zombiesLeft;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private UpgradeMenu _upgradeMenu;
        [Inject] private MoneyWallet _moneyWallet;
        [Inject] private SpeedUpLogic _speedUpLogic;
        [Inject] private Train.Train _train;
        [Inject] private DiContainer _diContainer;

        private ChunkMovement _chunkMovement;
        private Coroutine _creatingCoroutine;

        public readonly List<Zombie> AliveZombies = new ();
        public List<Zombie> DeadZombies { get; } = new ();
        public float WholeHpSum { get; private set; }
        public float LostHp { get; private set; }
        public float HpToLastWave { get; private set;}
        public float Progress => LostHp / WholeHpSum;
        
        public event Action OnHpChanged;
        public event Action LastWaveStarted;
        public event Action HugeWaveMessage;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _chunkMovement = GetComponent<ChunkMovement>();
        }
        private void Start()
        {
            _gameStateManager.AttackStarted += StartCreatingZombies;
            _gameStateManager.AttackStarted += () => {  _chunkMovement.SetSpeed(_train.TrainSpeed);};
            _gameStateManager.Fail += ZombieWin;

            _speedUpLogic.OnTapCountChanged += () => { _chunkMovement.SetSpeed(_train.TrainSpeed); };
            
            StartCoroutine(StartMotion());
        }
        #endregion

        #region Init
        public void Init(Level currentLevel)
        {
            _zombiesWaves = currentLevel.ZombiesWaves;

            foreach (var zombieWave in _zombiesWaves)
            {
                if (zombieWave == _zombiesWaves[^1])
                    HpToLastWave = WholeHpSum;

                foreach (var subWave in zombieWave.subWaves)
                {
                    _zombiesLeft += subWave.ZombieCount.UsualZombieCount;
                    _zombiesLeft += subWave.ZombieCount.FastZombieCount;
                    _zombiesLeft += subWave.ZombieCount.BigZombieCount;

                    WholeHpSum += subWave.ZombieCount.UsualZombieCount *
                                  usualZombie[0].StartHp(_levelManager.CurrentLevel);
                    WholeHpSum += subWave.ZombieCount.FastZombieCount * fastZombie.StartHp(_levelManager.CurrentLevel);
                    WholeHpSum += subWave.ZombieCount.BigZombieCount * bigZombie.StartHp(_levelManager.CurrentLevel);
                }
            }
            
            zombieTable.UpdatePanel(currentLevel.ZombieCount);
        }

        public void InitMotion(Chunk firstChunk)
        {
            _chunkMovement.Init(firstChunk);
        }
        
        private IEnumerator StartMotion()
        {
            // wait for init
            yield return new WaitForSeconds(0.2f);
            _chunkMovement.SetSpeed(_train.TrainSpeed);
            _chunkMovement.ChangeState(true);
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
                if (zombieWave == _zombiesWaves[^1])
                    StartCoroutine(LasWaveStartedEvent());
                
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
                        
                        CreateZombie(GetTargetZombie(zombieType), zombieWave.SpeedMultiplier);
                        yield return new WaitForSeconds(subWave.TimeBetweenZombie);
                    }
                    yield return new WaitForSeconds(subWave.TimeBetweenWaves);
                }
                yield return new WaitForSeconds(zombieWave.TimeBetweenWaves - messageTimeBeforeLastWave);
                if (zombieWave == _zombiesWaves[^2])
                    HugeWaveMessage?.Invoke();
                yield return new WaitForSeconds(messageTimeBeforeLastWave);

            }
        }

        private IEnumerator LasWaveStartedEvent()
        {
            yield return new WaitForSeconds(tapMessageDelayAfterLastWave);
            LastWaveStarted?.Invoke();
        }

        private void CreateZombie(Zombie targetZombie, float speedMultiplier)
        {
            var zombie = _diContainer.InstantiatePrefabForComponent<Zombie>(targetZombie, transform.position, transform.rotation, zombieTransform);
            
            zombie.Init(_chunkMovement.CurrentChunk, ZombieDelta, speedMultiplier);
            
            zombie.DeadEvent += RemoveZombie;
            zombie.GetDamageEvent += UpdateLostHp;
            zombie.GetDamageEvent += value =>
            {
                var reward = value * _upgradeMenu.IncomeCoefficient;
                _moneyWallet.Add((int) reward);
            };
            
            AliveZombies.Add(zombie);
        }

        private Zombie GetTargetZombie(ZombieType zombieType)
        {
            Zombie targetZombie;
            switch (zombieType)
            {
                case ZombieType.Usual:
                    targetZombie = usualZombie[Random.Range(0, usualZombie.Length)];
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

        private float ZombieDelta => Random.Range(creatingPositionFrom.localPosition.x,
            creatingPositionTo.localPosition.x);
        #endregion

        public Zombie GetNearestZombie(Transform fromTransform)
        {
            Zombie targetZombie = null;
            var minDistance = 1e9f;
            
            foreach (var zombie in AliveZombies)
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
            AliveZombies.Remove(zombie);
            DeadZombies.Add(zombie);
            zombieTable.RemoveZombie(zombie.ZombieType);
            _zombiesLeft--;
            
            if (_zombiesLeft <= 0 && AliveZombies.Count == 0)
            {
                _gameStateManager.ChangeState(GameState.Victory);
            }
        }
        
        private void ZombieWin()
        {
            _chunkMovement.ChangeState(false);
            
            if (_creatingCoroutine != null) 
                StopCoroutine(_creatingCoroutine);
        }
    }
}
