using _Scripts.Levels;
using UnityEngine;

namespace _Scripts.Train
{
    public class ChunkMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed;
        [SerializeField] private bool mirrorRotation;
        
        private float _currentChunkProgress;
        private bool _isMoving;
        
        public float MovementSpeed => movementSpeed;
        public Chunk CurrentChunk { get; private set; }
        #endregion

        #region Monobehaviour Callbacks
        private void Update()
        {
            if (_isMoving)
            {
                Move();
            }
        }
        #endregion
        
        public void Init(Chunk firstChunk)
        {
            CurrentChunk = firstChunk;
            _currentChunkProgress = CurrentChunk.GetCurrentProgress(transform.position);
            transform.position = CurrentChunk.GetPoint(_currentChunkProgress);
        }
        
        public void SetSpeed(float targetSpeed)
        {
            movementSpeed = targetSpeed;
        }
        
        public void ChangeState(bool isMoving)
        {
            _isMoving = isMoving;
        }

        private void Move()
        {
            _currentChunkProgress += Time.deltaTime * movementSpeed / CurrentChunk.Length;
            if (_currentChunkProgress > 1f)
            {
                _currentChunkProgress--;
                CurrentChunk = CurrentChunk.nextChunk;
            }

            var targetPosition = CurrentChunk.GetPoint(_currentChunkProgress);
            
            var direction = (targetPosition - transform.position).normalized;
            if (mirrorRotation)
                direction *= -1;
            
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