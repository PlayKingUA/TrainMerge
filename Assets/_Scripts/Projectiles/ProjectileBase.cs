using _Scripts.Units;
using QFSW.MOP2;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class ProjectileBase : MonoBehaviour
    {
        #region Variables
        [SerializeField] private protected float damageRadius;
        
        private protected Vector3 LaunchPosition;
        private protected Vector3 TargetPosition;
        private protected Vector3 Direction;
        
        private Collider[] _colliders;
        
        private int _damage;
        private float _damageRadius;

        private protected MasterObjectPooler MasterObjectPooler;
        private ObjectPool _projectilePool;

        #endregion
        
        #region Monobehavior Callbacks
        protected virtual void Awake()
        {
            MasterObjectPooler = MasterObjectPooler.Instance;
        }
        protected virtual void Start(){}
        protected virtual  void OnTriggerEnter(Collider other){}
        #endregion
        
        public virtual void Init(Vector3 targetPosition, int damage, ObjectPool objectPool)
        {
            _projectilePool = objectPool;
            LaunchPosition = transform.position;
            UpdateTargetPosition(targetPosition);

            Direction = (TargetPosition - LaunchPosition).normalized;

            _damageRadius = damageRadius;
            _damage = damage;
        }

        public virtual void UpdateTargetPosition(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
            TargetPosition.y = LaunchPosition.y;
        }
        
        protected  virtual void ReturnToPool()
        {
            MasterObjectPooler.Release(gameObject, _projectilePool.PoolName);
        }
        
        public virtual void HitZombie(Transform damagePoint = null)
        {
            if (damagePoint == null) damagePoint = transform;
            
            _colliders = Physics.OverlapSphere(damagePoint.position, _damageRadius);

            for (var i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i] != null && _colliders[i].TryGetComponent(out Zombie zombie))
                {
                    zombie.GetDamage(_damage);
                }
            }
        }
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}