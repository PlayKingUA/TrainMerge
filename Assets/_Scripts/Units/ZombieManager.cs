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
        [ShowInInspector, ReadOnly] private List<Zombie> _aliveZombies = new ();

        private Queue<Zombie> _zombieToCreate;
        [ShowInInspector, ReadOnly] private Vector2 _timeBetweenZombieCreation;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private DiContainer _diContainer;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _gameStateManager.AttackStarted += StartCreatingZombies;
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
            StartCoroutine(CreateZombies());
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
            zombie.DeadEvent += RemoveZombie;
            _aliveZombies.Add(zombie);
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
    }
}
