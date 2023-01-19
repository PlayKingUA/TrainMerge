using _Scripts.Levels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Buttons
{
    public class RestartButton : MonoBehaviour
    {
        #region Variables
        private Button _button;

        [Inject] private LevelManager _levelManager;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(StartWave);
        }
        #endregion

        private void StartWave()
        {
            _levelManager.RestartForce();
        }
    }
}
