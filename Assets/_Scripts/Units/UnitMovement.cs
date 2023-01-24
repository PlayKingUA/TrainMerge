using _Scripts.Levels;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Scripts.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed;
        private Chunk _currentChunk;

        private float _currentChunkProgress;

        [Inject] private Train.Train _train;

        private float _targetPositionX;
        #endregion

        public void Move()
        {
            _currentChunkProgress += Time.deltaTime * movementSpeed / _currentChunk.Length;
            if (_currentChunkProgress > 1f)
            {
                _currentChunkProgress--;
                _currentChunk = _currentChunk.nextChunk;
            }

            var targetPosition = _currentChunk.GetPoint(_currentChunkProgress);
            
            var direction = transform.position - targetPosition;
            var rotateY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var targetRotation  = Quaternion.Euler(0, rotateY, 0);

            transform.position = targetPosition;
            if (Quaternion.Angle(transform.rotation, targetRotation) == 0) return;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                targetRotation,  
                Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f));
        }

        
    }
}