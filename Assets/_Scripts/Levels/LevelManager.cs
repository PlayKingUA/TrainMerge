using _Scripts.Game_States;
using _Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Scripts.Levels
{
    public class LevelManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Level[] levels;
        [ShowInInspector, ReadOnly] private Level _currentLevel;
        
        private const string SaveKey = "Level";

        private int _currentLevelIndex;

        [Inject] private ZombieManager _zombieManager;
        [Inject] private GameStateManager _gameStateManager;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            Load();
            _gameStateManager.Victory += IncreaseLevel;
        }
        #endregion

        private void IncreaseLevel()
        {
            if (_currentLevelIndex + 1 < levels.Length)
            {
                _currentLevelIndex++;
            }
            
            Save();
        }
        
        #region Restart Logic
        public void RestartForce()
        {
            RestartGame();
        }

        private void RestartGame()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion
        
        
        private void LoadLevel()
        {
            var currentLevel = _currentLevelIndex % levels.Length;
            _currentLevel = levels[currentLevel];
            _zombieManager.Init(_currentLevel.Zombies, _currentLevel.TimeBetweenZombie);
        }
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(SaveKey, _currentLevelIndex);
        }

        private void Load()
        {
            _currentLevelIndex = PlayerPrefs.GetInt(SaveKey, 0);
            LoadLevel();
        }
        #endregion
    }
}
