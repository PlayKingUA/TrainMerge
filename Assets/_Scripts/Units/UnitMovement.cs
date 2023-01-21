using _Scripts.Interface;
using Sirenix.OdinInspector;
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

        private float _targetPositionX;
        #endregion

        #region Properties
        private Vector3 TargetPosition => _train.GetTargetZombiePosition(_targetPositionX);
        public float DistanceFromTarget => Vector3.Distance(transform.position, TargetPosition);
        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _targetPositionX = _train.GetZombiePositionX();
        }
        #endregion

        public void Move()
        {
            _agent.SetDestination(TargetPosition);
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