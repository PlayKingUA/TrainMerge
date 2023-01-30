using _Scripts.Projectiles;
using _Scripts.Units;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class PistolWeapon : Weapon
    {
        #region Variables
        [Space(10)]
        [SerializeField]
        protected Transform shootPoint;
        [SerializeField] protected ObjectPool projectilePool;

        protected MasterObjectPooler _masterObjectPooler;
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

            Fire();
            AttackTimer = 0f;
        }
        #endregion
        
        protected virtual void Fire()
        {
            var bullet =
                _masterObjectPooler.GetObjectComponent<Projectile>(projectilePool.PoolName, shootPoint.position, shootPoint.rotation);

            bullet.Init(TargetZombie, Damage, projectilePool);
        }
    }
}