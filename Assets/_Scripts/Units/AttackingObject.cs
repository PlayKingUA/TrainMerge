using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Units
{
    public class AttackingObject : MonoBehaviour
    {
        #region Variables
        [SerializeField] protected int health;
        [SerializeField] protected int damage;
        [SerializeField] protected float attackSpeedPerSecond;
        [SerializeField] protected float attackRadius;

        [ShowInInspector, ReadOnly] protected float AttackTimer;
        #endregion

        #region Properties
        public int Health => health;
        
        public virtual float GetCoolDown()
        {
            return 1f / attackSpeedPerSecond;
        }

        #endregion
        
        #region Monobehaviour Callbacks
        protected virtual void Start(){}

        protected virtual void Update()
        {
            AttackTimer += Time.deltaTime;
        }
        #endregion
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}