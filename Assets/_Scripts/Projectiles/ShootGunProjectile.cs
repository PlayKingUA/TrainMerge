using System.Collections.Generic;
using _Scripts.Units;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class ShootGunProjectile : Projectile
    {
        [SerializeField] private ObjectPool projectileParticles;
        [SerializeField] private float fieldOfShooting = 20f;
        
        private List<Zombie> _targetZombies;
        
        public void Init(List<Zombie> targetZombies, int bulletCount, int damage, ObjectPool objectPool)
        {
            _targetZombies = targetZombies;
            ProjectilePool = objectPool;
            LaunchPosition = transform.position;
            UpdateTargetZombie(_targetZombies[0]);
            
            SetDamage(damage);
            
            _flyRoutine = StartCoroutine(FlyToTarget());
            CreateProjectiles(bulletCount);
        }

        private void CreateProjectiles(int bulletCount)
        {
            if (hasMuzzleflare)
                MasterObjectPooler.GetObject(muzzleflarePool.PoolName, transform.position, transform.rotation);

            // straight bullet
            if (bulletCount % 2 > 0)
            {
                CreateProjectile(transform.rotation);
                bulletCount--;
            }
            var yDegrees = transform.rotation.eulerAngles.y;
                     
            var angleStep = fieldOfShooting / bulletCount;
            for (var i = 1; i < bulletCount + 1; i++)
            {
                var  direction = i % 2 == 0 ? 1 : - 1;
                var degrees = direction * angleStep * (i - i % 2);
                CreateProjectile(Quaternion.Euler(0f, yDegrees + degrees, 0f));
            }
        }

        private void CreateProjectile(Quaternion targetRotation)
        {
            if (hasShells)
                MasterObjectPooler.GetObject(shellsPool.PoolName, transform.position, transform.rotation);
            
            var projectile = MasterObjectPooler.GetObjectComponent<ProjectileParticle>(projectileParticles.PoolName,
                transform.position, targetRotation);
            projectile.Init(projectileParticles.PoolName, LifeTime, speed);
        }

        public override void HitZombie(Transform damagePoint = null)
        {
            foreach (var zombie in _targetZombies)
            {
                zombie.GetDamage(Damage);
                if (!hasImpact) continue;
                var transform1 = zombie.ShootPoint;
                MasterObjectPooler.GetObject(impactPool.PoolName, transform1.position, transform1.rotation);
            }
            
            ReturnToPool();
        }
    }
}