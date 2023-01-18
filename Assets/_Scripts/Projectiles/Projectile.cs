using System.Collections;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public sealed class Projectile : MonoBehaviour
    {
        #region Variables
        [Header("Speed")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeedDistance;
        [Space]
        [SerializeField] private AnimationCurve yFlyTrajectory;
        [SerializeField] private float yFlyWeight = 1;

        private float _damageRadius;

        private Vector3 _launchPosition;
        private Vector3 _targetPosition;

        private float _flyTime;
        private float _flySpeed;
        
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
            _targetPosition = targetPosition;
            
            var distanceToTargetPoint = Vector3.Distance(transform.position, _targetPosition);
            var distanceT = distanceToTargetPoint / maxSpeedDistance;
            _flySpeed = Mathf.Lerp(minSpeed, maxSpeed, distanceT);
            _flyTime = distanceToTargetPoint / _flySpeed;

            _damageRadius = damageRadius;
            _damage = damage;
            
            _flyRoutine = StartCoroutine(FlyToTarget());
        }

        private void HitTarget()
        {
            
        }

        private IEnumerator FlyToTarget()
        {
            float t = 0;

            while (true)
            {
                transform.position = GetPositionOnTrajectory(t);
                var trajectoryDirection = (GetPositionOnTrajectory(t + 0.05f) -
                                           transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(trajectoryDirection);

                t += Time.deltaTime;
                yield return null;

                if (t < 1f) continue;
                ReturnToPool();
                yield break;
            }
        }

        private Vector3 GetPositionOnTrajectory(float time)
        {
            Vector3 targetPosition = Vector3.Lerp(_launchPosition, _targetPosition, time);
            targetPosition.y += yFlyTrajectory.Evaluate(time) * yFlyWeight;
            return targetPosition;
        }

        private void ReturnToPool()
        {
            if(_flyRoutine != null) 
                StopCoroutine(_flyRoutine);
            _masterObjectPooler.Release(gameObject, _projectilePool.PoolName);
        }
    }
}