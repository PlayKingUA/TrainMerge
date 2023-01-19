using _Scripts.Game_States;
using _Scripts.Levels;
using _Scripts.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Displays
{
    public class ProgressDisplay : MonoBehaviour
    {
        #region Variables

        [SerializeField] private CanvasGroup distanceTextObject;
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;
        [SerializeField] private Slider progressSlider;

        [Inject] private ZombieManager _zombieManager;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelManager _levelManager;
        #endregion
    
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _zombieManager.OnHpChanged += DisplayHp;
            
            _gameStateManager.PrepareToBattle += () => { EnableDistanceObject(false);};
            _gameStateManager.AttackStarted += () =>
            {
                DisplayHp();
                EnableDistanceObject(true);
            };

            _levelManager.OnLevelLoaded += UpdateLevelText;
        }
        #endregion

        private void DisplayHp()
        {
            progressSlider.value = _zombieManager.LostHp / _zombieManager.WholeHpSum;
        }

        private void DisplayDistance()
        {
            
        }

        private void UpdateRemainingZombies()
        {
            
        }

        private void UpdateLevelText(int level)
        {
            currentLevelText.text = level.ToString();
            nextLevelText.text = (level + 1).ToString();
        }
        
        private void EnableDistanceObject(bool isEnabled)
        {
            WindowsManager.CanvasGroupSwap(distanceTextObject, isEnabled);
        }
    }
}
