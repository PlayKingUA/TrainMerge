using System.Collections;
using _Scripts.Units;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public sealed class Projectile : ProjectileBase
    {
        #region Variables
        [Space(10)]
        [SerializeField] private float speed;
        [SerializeField] private ObjectPool muzzleflarePool;
        [SerializeField] private ObjectPool shellsPool;
        [SerializeField] private bool hasShells;
        [SerializeField] private bool hasMuzzleflare = true;
        
        private const float LifeTime = 3.0f;

        private Coroutine _flyRoutine;
        #endregion

        #region Monobehavior Callbacks
        protected override void OnTriggerEnter(Collider other)
        {
            HitZombie();
        }
        #endregion

        public override void Init(Vector3 targetPosition, int damage, ObjectPool objectPool)
        {
            base.Init(targetPosition, damage, objectPool);
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
            base.HitZombie(damagePoint);
            ReturnToPool();
        }

        private IEnumerator FlyToTarget()
        {
            float t = 0;
            
            while (true)
            {
                transform.position += Direction * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(Direction);

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