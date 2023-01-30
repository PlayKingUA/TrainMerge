using System.Collections.Generic;
using _Scripts.Units;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class ShootGunProjectile : Projectile
    {
        private List<Zombie> _targetZombies;
        
        public void Init(List<Zombie> targetZombies, int bulletCount, int damage, ObjectPool objectPool)
        {
            _targetZombies = targetZombies;
            _projectilePool = objectPool;
            LaunchPosition = transform.position;
            UpdateTargetZombie(_targetZombies[0]);
            
            SetDamage(damage);
            
            _flyRoutine = StartCoroutine(FlyToTarget());
            
            if (hasMuzzleflare)
            {
                MasterObjectPooler.GetObject(muzzleflarePool.PoolName, transform.position, transform.rotation);
            }
            if (!hasShells) return;
            for (var i = 0; i < bulletCount; i++)
            {
                MasterObjectPooler.GetObject(shellsPool.PoolName, transform.position, transform.rotation);
            }
        }

        public override void HitZombie(Transform damagePoint = null)
        {
            foreach (var zombie in _targetZombies)
            {
                zombie.GetDamage(_damage);
                if (!hasImpact) continue;
                var transform1 = zombie.transform;
                MasterObjectPooler.GetObject(impactPool.PoolName, transform1.position, transform1.rotation);
            }
            
            ReturnToPool();
        }
    }
}