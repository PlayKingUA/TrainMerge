using System;
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
        [SerializeField] private float speedForDistance;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;
        [SerializeField] private Slider progressSlider;

        [Inject] private ZombieManager _zombieManager;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelManager _levelManager;

        private float _startPlayTime;
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
                _startPlayTime = Time.time;
            };

            _levelManager.OnLevelLoaded += UpdateLevelText;
        }

        private void Update()
        {
            if (_gameStateManager.CurrentState == GameState.Battle)
            {
                DisplayDistance();
            }
        }

        #endregion

        private void DisplayHp()
        {
            progressSlider.value = _zombieManager.LostHp / _zombieManager.WholeHpSum;
        }

        private void DisplayDistance()
        {
            distanceText.text = ((Time.time - _startPlayTime) * speedForDistance).ToString("F0");
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
