using UnityEngine;
using Lofelt.NiceVibrations;

namespace _Scripts
{
    public class VibrationManager : MonoBehaviour
    {
        #region Variables
        private const string Key = "VibrationKey";
        private bool _isEnabled;

        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            Load();
        }
        #endregion
        
        public void Haptic(HapticPatterns.PresetType presetType)
        {
            if (!_isEnabled)
                return;
            
            HapticPatterns.PlayPreset(presetType);
        }

        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(Key, _isEnabled ? 1 : 0);
        }

        private void Load()
        {
            _isEnabled = PlayerPrefs.GetInt(Key) > 0;
        }
        #endregion
    }
}
