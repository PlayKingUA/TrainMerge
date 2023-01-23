using _Scripts.Projectiles;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class PistolWeapon : Weapon
    {
        #region Variables
        [Space(10)]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private ObjectPool projectilePool;

        private MasterObjectPooler _masterObjectPooler;
        #endregion

        #region Monobehaviour Callbacks
        protected override void Start()
        {
            _masterObjectPooler =MasterObjectPooler.Instance;
            base.Start();
        }
        #endregion

        #region States
        protected override void AttackState()
        {
            base.AttackState();
            if (AttackTimer < CoolDown|| !CanAttack) 
                return;

            Fire(TargetZombie.transform);
            AttackTimer = 0f;
        }
        #endregion
        
        private void Fire(Transform targetPosition)
        {
            var bullet =
                _masterObjectPooler.GetObjectComponent<Projectile>(projectilePool.PoolName, shootPoint.position, shootPoint.rotation);

            bullet.Init(targetPosition.position,
                Damage, projectilePool);
        }
    }
}