using _Scripts.Game_States;
using _Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Buttons
{
    public class StartButton : MonoBehaviour
    {
        #region Variables
        private Button _attackButton;

        [Inject] private WindowsManager _windowsManager;
        [Inject] private GameStateManager _gameStateManager;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _attackButton = GetComponent<Button>();
            _attackButton.onClick.AddListener(StartWave);
        }
        #endregion

        private void StartWave()
        {
            _windowsManager.SwapWindow(WindowType.Game);
            _gameStateManager.ChangeState(GameState.Battle);
        }
    }
}
