using System;
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
        
        public event Action GetWeaponFromSlotEvent;
        #endregion

        #region Properties
        public int GetCurrentLevel => (_weapon) ? _weapon.Level : NoSlotLevel;

        #endregion
    
        #region Monobehaviour Callbacks
        private void Start()
        {

        }
        #endregion

        #region Slot Logic
        public void SetWeaponToSlot(Weapon weapon)
        {
            _weapon = weapon;
            SlotState = SlotState.Busy;
            
            Save();
        }

        public Weapon GetWeaponFromSlot()
        {
            GetWeaponFromSlotEvent?.Invoke();
            return _weapon;
        }

        public void ClearSlot()
        {
            _weapon = null;
            SlotState = SlotState.Empty;
            
            Save();
        }

        public void Upgrade()
        {
            //ToDo
        }
        #endregion
        
        public void Init()
        {
            _saveKey += transform.GetSiblingIndex();
            Load();
        }

        public Weapon SpawnWeapon(int level)
        {
            SetWeaponToSlot(_weaponManager.CreateWeapon(level, weaponPosition));
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
                SpawnWeapon(currentLevel);
            }
        }
        #endregion
    }
}
