using System.Collections;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public sealed class Projectile : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float speed;

        private const float LifeTime = 3.0f;
        
        private float _damageRadius;

        private Vector3 _launchPosition;
        private Vector3 _targetPosition;

        private float _damage;
        private ObjectPool _projectilePool;

        private MasterObjectPooler _masterObjectPooler;
        
        private Coroutine _flyRoutine;
        #endregion

        #region Monobehavior Callbacks
        private void Start()
        {
            _masterObjectPooler = MasterObjectPooler.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            HitTarget();
        }
        #endregion

        public void Init(Vector3 targetPosition, float damage, float damageRadius, ObjectPool objectPool)
        {
            _projectilePool = objectPool;
            _launchPosition = transform.position;
            _targetPosition = targetPosition;

            _damageRadius = damageRadius;
            _damage = damage;
            
            _flyRoutine = StartCoroutine(FlyToTarget());
        }

        private void HitTarget()
        {
            ReturnToPool();
        }

        private IEnumerator FlyToTarget()
        {
            float t = 0;
            var direction = (_targetPosition - _launchPosition).normalized;
            
            while (true)
            {
                transform.position += direction * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);

                t += Time.deltaTime;
                yield return null;

                if (t < LifeTime) continue;
                ReturnToPool();
                yield break;
            }
        }
        
        private void ReturnToPool()
        {
            if(_flyRoutine != null) 
                StopCoroutine(_flyRoutine);
            _masterObjectPooler.Release(gameObject, _projectilePool.PoolName);
        }
    }
}