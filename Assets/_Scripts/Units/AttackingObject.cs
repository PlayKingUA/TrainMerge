using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Units
{
    public class AttackingObject : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int health;
        [SerializeField] private int damage;
        [SerializeField] protected float attackSpeedPerSecond;
        [SerializeField] protected float attackRadius;
        [ShowInInspector, ReadOnly] protected float AttackTimer;
        #endregion

        #region Properties
        public int Health
        {
            protected set => health = value;
            get => health;
        }

        protected virtual float CoolDown => 1f / attackSpeedPerSecond;

        protected virtual int Damage => damage; 
        #endregion
        
        #region Monobehaviour Callbacks
        protected virtual void Start(){}

        protected virtual void Update()
        {
            AttackTimer += Time.deltaTime;
        }
        #endregion
    }
}