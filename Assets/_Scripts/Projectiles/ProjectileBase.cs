using _Scripts.Units;
using QFSW.MOP2;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class ProjectileBase : MonoBehaviour
    {
        #region Variables
        [SerializeField] private protected float damageRadius;
        
        private Vector3 _launchPosition;
        private Vector3 _targetPosition;
        private protected Vector3 Direction;
        
        private Collider[] _colliders;
        
        [ShowInInspector]private int _damage;
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
            _launchPosition = transform.position;
            UpdateTargetPosition(targetPosition);

            Direction = (_targetPosition - _launchPosition).normalized;

            _damageRadius = damageRadius;
            SetDamage(damage);
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        public virtual void UpdateTargetPosition(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
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