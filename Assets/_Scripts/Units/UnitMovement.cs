using _Scripts.Interface;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Scripts.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitMovement : MonoBehaviour, IMove
    {
        #region Variables
        private NavMeshAgent _agent;

        [Inject] private Train.Train _train;
        #endregion
        
        #region Monobehaviour Callbacks

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        #endregion

        public void Move()
        {
            _agent.SetDestination(_train.transform.position);
        }

        public void StopMove()
        {
            _agent.isStopped = true;
        }

        public void SetSpeed(float targetSpeed)
        {
            _agent.speed = targetSpeed;
        }
    }
}