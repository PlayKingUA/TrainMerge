using System;
using _Scripts.Weapons;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Scripts.Slot_Logic
{
    public sealed class Slot : MonoBehaviour
    {
        #region Variables
        [SerializeField] private MeshRenderer meshRenderer;
        [Space(10)]
        [SerializeField] private Color empty;
        [SerializeField] private Color freePlace;
        [SerializeField] private Color enabledUpgrade;
        [SerializeField] private Color disabledUpgrade;
        [SerializeField] private Color busy;
        [Space]
        [SerializeField] private Transform weaponPosition;
        [SerializeField] private float motionTime;

        private Tween _motionTween;
        
        [Inject] private WeaponManager _weaponManager;
        [Inject] private SlotManager _slotManager;
        public SlotState SlotState { get; private set; }
        private Weapon _weapon;
        
        private string _saveKey = "weaponLevel";
        private static readonly int MaterialColorId = Shader.PropertyToID("_Color");
        private const int NoSlotLevel = -1;
        #endregion

        #region Properties
        private int GetCurrentLevel => (_weapon) ? _weapon.Level : NoSlotLevel;
        #endregion

        #region Slot Logic
        private void SetWeaponToSlot(Weapon weapon)
        {
            _weapon = weapon;
            SlotState = SlotState.Busy;
            ChangeColor();
            
            Save();
        }

        public Weapon GetWeaponFromSlot()
        {
            return _weapon;
        }

        public void ClearSlot()
        {
            _weapon = null;
            SlotState = SlotState.Empty;
            ChangeColor();
            _slotManager.RefreshSlots(this);
            
            Save();
        }

        public void SetWeaponWithMotion(Weapon weapon)
        {
            SetWeaponToSlot(weapon);
            MoveToPosition();
            _slotManager.RefreshSlots(this);
        }
        
        public void Refresh(Weapon weapon, Slot previousSlot)
        {
            switch (SlotState)
            {
                case SlotState.Busy when CanUpgrade(weapon):
                    Upgrade(weapon);
                    return;
                case SlotState.Busy when !CanUpgrade(weapon):
                    // swap
                    _weapon.ReturnToPreviousPos(previousSlot);
                    ClearSlot();
                    SetWeaponWithMotion(weapon);
                    return;
                case SlotState.Empty:
                    SetWeaponWithMotion(weapon);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void Upgrade(Weapon weapon)
        {
            var targetLevel = weapon.Level + 1;
            Destroy(weapon.gameObject);
            Destroy(_weapon.gameObject);

            SpawnWeapon(targetLevel);
        }

        private bool CanUpgrade(Weapon weapon)
        {
            return _weapon.Level < _weaponManager.MaxWeaponLevel 
                && _weapon.Level == weapon.Level;
        }
        #endregion
        
        public void Init()
        {
            _saveKey += transform.GetSiblingIndex();
            Load();
            ChangeColor();
        }

        public Weapon SpawnWeapon(int level)
        {
            SetWeaponToSlot(_weaponManager.CreateWeapon(level, weaponPosition));
            return _weapon;
        }

        private void MoveToPosition()
        {
            _motionTween.Kill();
            _motionTween = _weapon.transform.DOMove(weaponPosition.position, motionTime);
        }
        
        public void ChangeColor(Weapon weapon = null)
        {
            var color = empty;
            if (_weapon != null)
            {
                if (weapon == null)
                {
                    color = busy;
                }
                else
                {
                    color = CanUpgrade(weapon) ? enabledUpgrade : disabledUpgrade;
                }
            }
            else if (weapon != null)
            {
                color = freePlace;
            }
            meshRenderer.material.SetColor(MaterialColorId, color);
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
