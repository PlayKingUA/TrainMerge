using System.Collections;
using System.Collections.Generic;
using _Scripts.Game_States;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Units
{
    public class ZombieManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform zombieCreatingPositions;
        [ShowInInspector, ReadOnly] private List<Zombie> _aliveZombies = new ();

        private Queue<Zombie> _zombieToCreate;
        [ShowInInspector, ReadOnly] private Vector2 _timeBetweenZombieCreation;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private DiContainer _diContainer;

        private Coroutine _creatingCoroutine;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _gameStateManager.AttackStarted += StartCreatingZombies;
            _gameStateManager.Fail += ZombieWin;
        }
        #endregion

        public void Init(Queue<Zombie> targetZombies, Vector2 timeBetweenZombieCreation)
        {
            _zombieToCreate = targetZombies;
            _timeBetweenZombieCreation = timeBetweenZombieCreation;
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

        private void CreateZombie(Zombie targetZombie)
        {
            var zombie = _diContainer.InstantiatePrefabForComponent<Zombie>(targetZombie, 
                GetZombieCreatingPosition(),
                targetZombie.transform.rotation, transform);
            
            zombie.DeadEvent += RemoveZombie;
            _aliveZombies.Add(zombie);
        }

        private Vector3 GetZombieCreatingPosition()
        {
            return zombieCreatingPositions.
                GetChild(Random.Range(0, zombieCreatingPositions.childCount)).position;
        }
        #endregion

        public Zombie GetNearestZombie(Transform fromTransform)
        {
            Zombie targetZombie = null;
            var minDistance = 1e9f;
            
            foreach (var zombie in _aliveZombies)
            {
                var currentDistance = Vector3.Distance(fromTransform.position, zombie.transform.position);
                if (!(currentDistance < minDistance)) continue;
                minDistance = currentDistance;
                targetZombie = zombie;
            }

            return targetZombie;
        }
        
        private void RemoveZombie(Zombie zombie)
        {
            _aliveZombies.Remove(zombie);
            if (_zombieToCreate.IsEmpty() && _aliveZombies.Count == 0)
            {
                _gameStateManager.ChangeState(GameState.Victory);
            }
        }
        
        private void ZombieWin()
        {
            if (_creatingCoroutine != null) 
                StopCoroutine(_creatingCoroutine);
            
            foreach (var zombie in _aliveZombies)
            {
                zombie.ChangeState(UnitState.Victory);
            }
        }
    }
}
