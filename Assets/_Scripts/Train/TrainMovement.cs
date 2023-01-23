using System;
using _Scripts.Game_States;
using _Scripts.Interface;
using _Scripts.Levels;
using DG.Tweening.Plugins.Core.PathCore;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Train
{
    public class TrainMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed;
        
        private Chunk _currentChunk;

        [Inject] private GameStateManager _gameStateManager;

        private bool _isMoving;
        
        [ShowInInspector] private float _currentChunkProgress;
        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            _gameStateManager.AttackStarted += () => { _isMoving = true;};
            _gameStateManager.Fail += () => { _isMoving = false;};
        }

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
            _currentChunk = firstChunk;
            _currentChunkProgress = _currentChunk.GetCurrentProgress(transform.position);
            transform.position = _currentChunk.GetPoint(_currentChunkProgress);
        }

        private void Move()
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