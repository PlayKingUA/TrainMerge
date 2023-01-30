﻿using System;
using _Scripts.Slot_Logic;
using _Scripts.UI.Upgrade;
using _Scripts.Units;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Weapons
{
    public class Weapon : AttackingObject
    {
        #region Variables
        [Space] 
        [SerializeField] private GameObject appearFx;
        [SerializeField] private Transform gunTransform;
        [Space(10)]
        [ShowInInspector, ReadOnly] private WeaponState _currentState;
        [ShowInInspector, ReadOnly] private int _level;

        [Inject] protected ZombieManager _zombieManager;
        [Inject] private UpgradeMenu _upgradeMenu;
        [Inject] private SpeedUpLogic _speedUpLogic;

        private Quaternion _startRotation;
        private Material _gunMaterial;
        private Tweener _tween;

        private float _maxShakeStrength = 0.05f;
        
        [ShowInInspector, ReadOnly] private protected Zombie TargetZombie;
        #endregion

        #region Properties
        public int Level => _level;
        public GameObject AppearFx => appearFx;

        private protected bool CanAttack => TargetZombie != null &&
                                            Vector3.Distance(transform.position, TargetZombie.transform.position) <=
                                            attackRadius;

        protected override float CoolDown =>
            base.CoolDown / _speedUpLogic.CoolDownSpeedUp / _upgradeMenu.AltSpeedCoefficient;
        
        protected override int Damage => (int) (base.Damage * _upgradeMenu.DamageCoefficient);

        #endregion
        
        #region Monobehaviour Callbacks
        protected override void Start()
        {
            base.Start();
            ChangeState(WeaponState.Idle);
            _startRotation = gunTransform.rotation;

            _gunMaterial = gunTransform.GetComponent<MeshRenderer>().material;

            _speedUpLogic.OnTapCountChanged += Shake;
        }

        protected override void Update()
        {
            base.Update();
            UpdateState();
        }

        private void OnDisable()
        {
            _speedUpLogic.OnTapCountChanged -= Shake;
            _tween.Kill();
        }

        #endregion
        
        #region States Logic
        public void ChangeState(WeaponState newState)
        {
            _currentState = newState;
        }
        
        private void UpdateState()
        {
            switch (_currentState)
            {
                case WeaponState.Idle:
                    IdleState();
                    break;
                case WeaponState.Attack:
                    AttackState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void IdleState()
        {
        }

        protected virtual void AttackState()
        {
            Rotate();
        }
        #endregion

        public void SetLevel(int level)
        {
            _level = level;
        }

        #region Motion
        public void ReturnToPreviousPos(Slot previousSlot)
        {
            previousSlot.Refresh(this, previousSlot);
        }

        private void Rotate()
        {
            UpdateTargetZombie();
            var targetRotation = _startRotation;
            
            if (TargetZombie != null)
            {
                var direction = TargetZombie.ShootPoint.position - gunTransform.position;
                var rotateY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, rotateY, 0);
            }

            if (Quaternion.Angle(gunTransform.rotation, targetRotation) == 0) return;
            var t =  Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            gunTransform.rotation = Quaternion.Lerp(gunTransform.rotation,
                targetRotation, t);
        }

        private void Shake()
        {
            var targetColor = new Color(_speedUpLogic.EffectPower, 0, 0);
            _gunMaterial.SetColor("_EmissionColor", targetColor);
            _gunMaterial.EnableKeyword("_EmissionColor");
            
            _tween.Rewind();
            _tween.Kill();
            if (_speedUpLogic.EffectPower != 0)
            {
                _tween = transform.
                    DOShakePosition(_speedUpLogic.EffectDuration, _speedUpLogic.EffectPower * _maxShakeStrength)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
        #endregion

        private void UpdateTargetZombie()
        {
            TargetZombie = _zombieManager.GetNearestZombie(transform);
        }
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}