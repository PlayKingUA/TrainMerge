using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Game_States;
using _Scripts.Levels;
using _Scripts.Train;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Scripts.Units
{
    [RequireComponent(typeof(ChunkMovement))]
    public class ZombieManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform creatingPositionFrom;
        [SerializeField] private Transform creatingPositionTo;
        private readonly List<Zombie> _aliveZombies = new ();
        private List<Zombie> _deadZombies = new ();

        private Queue<Zombie> _zombieToCreate;
        [ShowInInspector, ReadOnly] private Vector2 _timeBetweenZombieCreation;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private Train.Train _train;
        [Inject] private DiContainer _diContainer;

        private ChunkMovement _chunkMovement;
        private Coroutine _creatingCoroutine;

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

        public void Init(Queue<Zombie> targetZombies, Vector2 timeBetweenZombieCreation)
        {
            _zombieToCreate = targetZombies;
            _timeBetweenZombieCreation = timeBetweenZombieCreation;

            WholeHpSum = _zombieToCreate.Sum(zombie => zombie.Health);
        }

        public void InitMotion(Chunk firstChunk)
        {
            _chunkMovement.Init(firstChunk);
            _chunkMovement.SetSpeed(_train.TrainSpeed);
        }

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
            while (!_zombieToCreate.IsEmpty())
            {
                var timeToWait = Random.Range(_timeBetweenZombieCreation.x, _timeBetweenZombieCreation.y);
                yield return new WaitForSeconds(timeToWait);
                CreateZombie(_zombieToCreate.Dequeue());
            }
        }

        private void CreateZombie(Object targetZombie)
        {
            var zombie = _diContainer.InstantiatePrefabForComponent<Zombie>(targetZombie, transform);
            
            zombie.InitMotion(_chunkMovement.CurrentChunk, ZombieDelta);
            
            zombie.DeadEvent += RemoveZombie;
            zombie.GetDamageEvent += UpdateLostHp;
            
            _aliveZombies.Add(zombie);
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
            _deadZombies.Add(zombie);
            if (_zombieToCreate.IsEmpty() && _aliveZombies.Count == 0)
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
