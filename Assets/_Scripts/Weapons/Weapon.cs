using System;
using _Scripts.Game_States;
using _Scripts.Projectiles;
using _Scripts.Slot_Logic;
using _Scripts.Units;
using QFSW.MOP2;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Weapons
{
    public class Weapon : AttackingObject
    {
        #region Variables
        [Space]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private float damageRadius;
        [Space] 
        [SerializeField] private GameObject appearFx;
        [SerializeField] private Transform gunTransform;
        [SerializeField] private Transform shootPoint;
        [Space(10)]
        [SerializeField] private ObjectPool projectilePool;
        [SerializeField] private ObjectPool muzzleflarePool;
        [SerializeField] private ObjectPool shellsPool;
        [SerializeField] private bool hasShells;
        [Space(10)]
        [ShowInInspector, ReadOnly] 
        private WeaponState _currentState;
        [ShowInInspector, ReadOnly] private int _level;

        [Inject] private ZombieManager _zombieManager;
        private MasterObjectPooler _masterObjectPooler;

        private Quaternion _startRotation;
        
        #endregion

        #region Properties
        public int Level => _level;
        public float Health => health;
        public GameObject AppearFx => appearFx;
        #endregion
        
        #region Monobehaviour Callbacks
        protected override void Start()
        {
            _masterObjectPooler =MasterObjectPooler.Instance;
            
            base.Start();
            ChangeState(WeaponState.Idle);
            _startRotation = gunTransform.rotation;
        }

        protected override void Update()
        {
            base.Update();
            UpdateState();
            Rotate();
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

        private void IdleState()
        {
            
        }

        private void AttackState()
        {
            if (AttackTimer < GetCoolDown()) 
                return;

            var targetZombie = _zombieManager.GetNearestZombie(transform);
            if (targetZombie == null) return;
            Fire(targetZombie.transform);
            AttackTimer = 0f;
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

        private void Fire(Transform targetPosition)
        {
            var bullet =
                _masterObjectPooler.GetObjectComponent<Projectile>(projectilePool.PoolName, shootPoint.position, shootPoint.rotation);
            var muzzleflare =
                _masterObjectPooler.GetObject(muzzleflarePool.PoolName, shootPoint.position, shootPoint.rotation);
            if (hasShells)
            {
                _masterObjectPooler.GetObject(shellsPool.PoolName, 
                    shootPoint.position, shootPoint.rotation);
            }

            bullet.Init(targetPosition.position,
                damage, damageRadius, projectilePool);
        }
        
        private void Rotate()
        {
            var targetZombie = _zombieManager.GetNearestZombie(transform);
            var targetRotation = _startRotation;
            
            if (targetZombie != null)
            {
                var direction = targetZombie.transform.position - gunTransform.position;
                var rotateY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, rotateY, 0);
            }

            if (Quaternion.Angle(gunTransform.rotation, targetRotation) == 0) return;
            var t =  Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            gunTransform.rotation = Quaternion.Lerp(gunTransform.rotation,
                targetRotation, t);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(shootPoint.position, damageRadius);
        }
    }
}