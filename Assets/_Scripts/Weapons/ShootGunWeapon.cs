using System.Collections.Generic;
using _Scripts.Projectiles;
using _Scripts.Units;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class ShootGunWeapon : PistolWeapon
    {
        #region Variables
        [SerializeField] private int bulletCount;
        [SerializeField, Range(1f, 10f)] private int damageCoefficient;

        private List<Zombie> _targetZombies;
        private int _targetDamage;
        #endregion

        #region Properties
        private float TargetCoefficient => 1f + CoefficientPerEmptyAim * (bulletCount - _targetZombies.Count);

        private float CoefficientPerEmptyAim => (damageCoefficient - 1f) / (bulletCount - 1);
        #endregion

        protected override void Fire()
        {
            GetAims();
            if (_targetZombies.Count == 0)
                return;
            
            var bullet =
                _masterObjectPooler.GetObjectComponent<ShootGunProjectile>(projectilePool.PoolName, shootPoint.position, shootPoint.rotation);

            bullet.Init(_targetZombies, bulletCount, (int) (Damage * TargetCoefficient), projectilePool);
        }

        private void GetAims()
        {
            ZombieManager.AliveZombies.Sort((x, y) =>
            {
                var position = transform.position;
                var distanceToA = Vector3.Distance(position, x.transform.position);
                var distanceToB = Vector3.Distance(position, y.transform.position);
                return (int) (distanceToA - distanceToB);
            });

            _targetZombies = new List<Zombie>();
            foreach (var zombie in ZombieManager.AliveZombies)
            {
                var distance = Vector3.Distance(transform.position, 
                    zombie.transform.position);
                if (distance < attackRadius)
                    _targetZombies.Add(zombie);
            }
        }
    }
    
    
}