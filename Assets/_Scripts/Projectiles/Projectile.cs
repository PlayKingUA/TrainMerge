using System.Collections;
using _Scripts.Units;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float flightSpeed;

        private Vector3 _launchPosition;
        private Vector3 _targetPosition;

        private int _damage;
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
            if (other.TryGetComponent(out Zombie zombie))
            {
                HitZombie(zombie);
            }
        }
        #endregion

        public void Init(Vector3 targetPosition, int damage, float damageRadius, ObjectPool objectPool)
        {
            _projectilePool = objectPool;
            _launchPosition = transform.position;
            _targetPosition = targetPosition;

            _damageRadius = damageRadius;
            _damage = damage;
            
            _flyRoutine = StartCoroutine(FlyToTarget());
        }

        private void HitZombie(Zombie zombie)
        {
            zombie.GetDamage(_damage);
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