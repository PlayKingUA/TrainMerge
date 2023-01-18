using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Game_States
{
    public class GameStateManager : MonoBehaviour
    {
        #region Variables
        [ShowInInspector, ReadOnly] private GameState _currentState;

        public GameState CurrentState => _currentState;
        
        public event Action PrepareToBattle;
        public event Action AttackStarted;
        public event Action Victory;
        public event Action Fail;
        #endregion
    
        #region Monobehaviour Callbacks
        private void Start()
        {
            ChangeState(GameState.PrepareToBattle);
        }
        #endregion
        
        public void ChangeState(GameState newState)
        {
            if (_currentState is GameState.Fail or GameState.Victory)
                return;

            _currentState = newState;

            switch (_currentState)
            {
                case GameState.PrepareToBattle:
                    PrepareToBattle?.Invoke();
                    break;
                case GameState.Battle:
                    AttackStarted?.Invoke();
                    break;
                case GameState.Victory:
                    Victory?.Invoke();
                    break;
                case GameState.Fail:
                    Fail?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
