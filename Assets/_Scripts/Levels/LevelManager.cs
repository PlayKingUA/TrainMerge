using _Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
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
        #endregion

        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            Load();
        }
        #endregion

        public void IncreaseLevel()
        {
            _currentLevelIndex++;
            
            Save();
        }
        
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
