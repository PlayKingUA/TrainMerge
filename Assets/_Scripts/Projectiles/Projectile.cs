using System.Collections;
using _Scripts.Units;
using DG.Tweening;
using QFSW.MOP2;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class Projectile : ProjectileBase
    {
        #region Variables
        [Space(10)]
        [SerializeField] private float speed;

        [SerializeField] protected bool hasShells;
        [SerializeField, ShowIf(nameof(hasShells))]
        protected ObjectPool shellsPool;
        
        [SerializeField] protected bool hasMuzzleflare = true;
        [SerializeField, ShowIf(nameof(hasMuzzleflare))]
        protected ObjectPool muzzleflarePool;
        
        [SerializeField] protected bool hasImpact = true;
        [SerializeField, ShowIf(nameof(hasImpact))]
        protected ObjectPool impactPool;

        private Tweener _motionTween;

        private const float LifeTime = 3.0f;

        protected Coroutine _flyRoutine;
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

        protected IEnumerator FlyToTarget()
        {
            float t = 0;
            var _direction = (TargetZombie.ShootPoint.position - LaunchPosition).normalized;
            
            while (true)
            {
                if (TargetZombie.IsDead)
                {
                    transform.position += _direction * speed * Time.deltaTime;
                }
                else
                {
                    _motionTween.Kill();
                    _motionTween = transform.DOMove(TargetZombie.ShootPoint.position, speed).SetSpeedBased();
                }

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