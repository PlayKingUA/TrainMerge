using System;
using _Scripts.Slot_Logic;
using _Scripts.Units;
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
        [ShowInInspector, ReadOnly] 
        private WeaponState _currentState;
        [ShowInInspector, ReadOnly] private int _level;

        [Inject] private ZombieManager _zombieManager;
        [Inject] private SpeedUpLogic _speedUpLogic;

        private Quaternion _startRotation;
        [ShowInInspector, ReadOnly]private protected Zombie TargetZombie;
        
        #endregion

        #region Properties
        public int Level => _level;
        public GameObject AppearFx => appearFx;

        private protected bool CanAttack => TargetZombie != null &&
                                  Vector3.Distance(transform.position, TargetZombie.transform.position) <= attackRadius;

        public override float GetCoolDown()
        {
            return base.GetCoolDown() / _speedUpLogic.CoolDownSpeedUp;
        }

        #endregion
        
        #region Monobehaviour Callbacks
        protected override void Start()
        {
            base.Start();
            ChangeState(WeaponState.Idle);
            _startRotation = gunTransform.rotation;
        }

        protected override void Update()
        {
            base.Update();
            UpdateState();
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
                var direction = TargetZombie.transform.position - gunTransform.position;
                var rotateY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, rotateY, 0);
            }

            if (Quaternion.Angle(gunTransform.rotation, targetRotation) == 0) return;
            var t =  Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            gunTransform.rotation = Quaternion.Lerp(gunTransform.rotation,
                targetRotation, t);
        }

        private void UpdateTargetZombie()
        {
            TargetZombie = _zombieManager.GetNearestZombie(transform);
        }
    }
}