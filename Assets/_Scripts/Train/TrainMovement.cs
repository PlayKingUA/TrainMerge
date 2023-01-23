using System;
using _Scripts.Game_States;
using _Scripts.Interface;
using _Scripts.Levels;
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
        
        private float _currentChunkProgress;
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
        }

        private void Move()
        {
            _currentChunkProgress += Time.deltaTime * movementSpeed / _currentChunk.Length;
            if (_currentChunkProgress > 1f)
            {
                _currentChunkProgress--;
                _currentChunk = _currentChunk.nextChunk;
            }
            transform.position = _currentChunk.GetPoint(_currentChunkProgress);
            transform.rotation = Quaternion.LookRotation(_currentChunk.GetFirstDerivative(_currentChunkProgress));
        }
    }
}