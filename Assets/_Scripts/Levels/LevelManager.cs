using System;
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

        [Inject] private ZombieManager _zombieManager;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelGeneration _levelGeneration;

        public int LevelNumber { get; private set; }

        public event Action<Level> OnLevelLoaded;
        #endregion

        #region Properties
        public int CurrentLevel => _currentLevel.index; 

        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            Application.targetFrameRate = 60;
            Load();
            _gameStateManager.Victory += IncreaseLevel;
        }
        #endregion

        private void IncreaseLevel()
        {
            LevelNumber++;

            Save();
        }
        
        #region Restart Logic
        public static void RestartForce()
        {
            RestartGame();
        }

        private static void RestartGame()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion
        
        private void LoadLevel()
        {
            var currentLevel = LevelNumber % levels.Length;
            _currentLevel = levels[currentLevel];
            _currentLevel.index = currentLevel + 1;
            _zombieManager.Init(_currentLevel);
            _levelGeneration.SetLocation(_currentLevel.LevelLocation);
            
            OnLevelLoaded?.Invoke(_currentLevel);
        }
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(SaveKey, LevelNumber);
        }

        private void Load()
        {
            LevelNumber = PlayerPrefs.GetInt(SaveKey, 0);
            LoadLevel();
        }
        #endregion
    }
}
