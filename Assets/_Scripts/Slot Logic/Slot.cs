using _Scripts.Weapons;
using UnityEngine;
using Zenject;

namespace _Scripts.Slot_Logic
{
    public class Slot : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform weaponPosition;
        [Inject] private WeaponManager _weaponManager;
        public SlotState SlotState { get; private set; }
        private Weapon _weapon;
        
        private string _saveKey = "weaponLevel";

        private const int NoSlotLevel = -1;
        #endregion

        #region Properties
        public int GetCurrentLevel => (_weapon) ? _weapon.Level : NoSlotLevel;

        #endregion
    
        #region Monobehaviour Callbacks
        private void Start()
        {

        }
        #endregion

        public void Init()
        {
            _saveKey += transform.GetSiblingIndex();
            Load();
        }

        public Weapon Spawn(int level)
        {
            _weapon = Instantiate(_weaponManager.GetWeapon(level), weaponPosition);
            SlotState = SlotState.Busy;

            Save();
            return _weapon;
        }
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(_saveKey, GetCurrentLevel);
        }

        private void Load()
        {
            var currentLevel = PlayerPrefs.GetInt(_saveKey, NoSlotLevel);

            if (currentLevel >= 0)
            {
                Spawn(currentLevel);
            }
        }
        #endregion
    }
}
