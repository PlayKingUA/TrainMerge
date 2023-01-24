using _Scripts.Projectiles;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class LaserWeapon : Weapon
    {
        #region Variables
        [Space(10)]
        [SerializeField] private Laser laser;
        #endregion

        #region Monobehavior Callbacks
        protected override void Start()
        {
            base.Start();
            laser.SetDamage(Damage);
        }
        #endregion
        
        #region States

        protected override void IdleState()
        {
            base.IdleState();
            EnableLaser(false);
        }

        protected override void AttackState()
        {
            base.AttackState();
            EnableLaser(CanAttack);
            if (!CanAttack)
                return;
            laser.UpdateTargetPosition(TargetZombie.ShootPoint.position);

            if (AttackTimer < CoolDown)
                return;
            laser.HitZombie(TargetZombie.ShootPoint);
            AttackTimer = 0f;
        }
        #endregion

        private void EnableLaser(bool isEnabled)
        {
            laser.gameObject.SetActive(isEnabled);
        }
    }
}