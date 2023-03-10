using System;
using _Scripts.UI;
using _Scripts.UI.Windows;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Game_States
{
    public class GameStateManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float failDelay;
        [SerializeField] private float victoryDelay;
        
        [ShowInInspector, ReadOnly] private GameState _currentState;
        
        [Inject] private WindowsManager _windowsManager;
        
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
                    _windowsManager.SwapWindow(WindowType.Results, victoryDelay);
                    break;
                case GameState.Fail:
                    Fail?.Invoke();
                    _windowsManager.SwapWindow(WindowType.Results, failDelay);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
