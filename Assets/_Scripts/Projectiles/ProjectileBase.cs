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

        protected Vector3 LaunchPosition;
        protected Zombie TargetZombie;
        
        
        private Collider[] _colliders;
        
        [ShowInInspector] private int _damage;
        [SerializeField] private bool isSplash;
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
        
        public virtual void Init(Zombie targetZombie, int damage, ObjectPool objectPool)
        {
            _projectilePool = objectPool;
            LaunchPosition = transform.position;
            UpdateTargetZombie(targetZombie);

            _damageRadius = damageRadius;
            SetDamage(damage);
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        public virtual void UpdateTargetZombie(Zombie targetZombie)
        {
            TargetZombie = targetZombie;
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
                if (_colliders[i] == null || !_colliders[i].TryGetComponent(out Zombie zombie)) continue;
                zombie.GetDamage(_damage);
                if (!isSplash)
                {
                    break;
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