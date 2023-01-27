using System.Collections;
using _Scripts.Units;
using QFSW.MOP2;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public sealed class Projectile : ProjectileBase
    {
        #region Variables
        [Space(10)]
        [SerializeField] private float speed;

        [SerializeField] private bool hasShells;
        [SerializeField, ShowIf(nameof(hasShells))] private ObjectPool shellsPool;
        
        [SerializeField] private bool hasMuzzleflare = true;
        [SerializeField, ShowIf(nameof(hasMuzzleflare))]
        private ObjectPool muzzleflarePool;
        
        [SerializeField] private bool hasImpact = true;
        [SerializeField, ShowIf(nameof(hasImpact))]
        private ObjectPool impactPool;

        private const float LifeTime = 3.0f;
        private Vector3 _direction;

        private Coroutine _flyRoutine;
        #endregion

        #region Monobehavior Callbacks
        protected override void OnTriggerEnter(Collider other)
        {
            HitZombie();
        }
        #endregion

        public override void Init(Zombie targetZombie, int damage, ObjectPool objectPool)
        {
            base.Init(targetZombie, damage, objectPool);
            _flyRoutine = StartCoroutine(FlyToTarget());

            if (hasMuzzleflare)
            {
                MasterObjectPooler.GetObject(muzzleflarePool.PoolName, transform.position, transform.rotation);
            }
            if (hasShells)
            {
                MasterObjectPooler.GetObject(shellsPool.PoolName, transform.position, transform.rotation);
            }
        }

        public override void HitZombie(Transform damagePoint = null)
        {
            if (hasImpact)
            {
                MasterObjectPooler.GetObject(impactPool.PoolName, transform.position, transform.rotation);
            }
            base.HitZombie(damagePoint);
            ReturnToPool();
        }

        private IEnumerator FlyToTarget()
        {
            float t = 0;
            
            while (true)
            {
                if (!TargetZombie.IsDead)
                {
                    _direction = (TargetZombie.ShootPoint.position - LaunchPosition).normalized;
                }

                transform.position += _direction * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(_direction);

                t += Time.deltaTime;
                yield return null;

                if (t < LifeTime) continue;
                ReturnToPool();
                yield break;
            }
        }
        
        protected override void ReturnToPool()
        {
            if(_flyRoutine != null) 
                StopCoroutine(_flyRoutine);
            base.ReturnToPool();
        }
    }
}