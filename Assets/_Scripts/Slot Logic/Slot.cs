using System;
using _Scripts.Game_States;
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

        [Space]
        [SerializeField, Range(0.5f, 2f)]
        private float scaleEffect;
        [SerializeField] private float scaleEffectDuration;
        
        private Tween _motionTween;
        private Tween _scaleTween;
        
        [Inject] private GameStateManager _gameStateManager;
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
        public int WeaponHealth => (_weapon) ? _weapon.Health : 0;
        #endregion

        #region Monobehavior Callbacks
        private void Start()
        {
            _gameStateManager.AttackStarted += StartLevel;
        }
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
            MoveWeaponToPosition();
            _slotManager.RefreshSlots(this);
            _weapon.transform.SetParent(weaponPosition);
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

            SpawnWeapon(targetLevel, true);
            BounceWeapon();
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

        private void StartLevel()
        {
            meshRenderer.gameObject.SetActive(false);
            if (_weapon != null)
            {
                _weapon.ChangeState(WeaponState.Attack);
            }
        }
        
        public Weapon SpawnWeapon(int level, bool showFx = false)
        {
            SetWeaponToSlot(_weaponManager.CreateWeapon(level, weaponPosition));
            _weapon.AppearFx.SetActive(showFx);
            return _weapon;
        }

        private void MoveWeaponToPosition()
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

        private void BounceWeapon()
        {
            _scaleTween.Kill();
            _scaleTween = weaponPosition.
                DOScale(weaponPosition.localScale * scaleEffect, scaleEffectDuration / 2)
                .SetLoops(2, LoopType.Yoyo);
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
