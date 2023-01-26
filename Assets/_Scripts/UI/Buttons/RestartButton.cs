using _Scripts.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Buttons
{
    public class RestartButton : MonoBehaviour
    {
        #region Variables
        private Button _button;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Restart);
        }
        #endregion

        private void Restart()
        {
            LevelManager.RestartForce();
        }
    }
}
