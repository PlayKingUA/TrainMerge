using UnityEngine;

namespace _Scripts.Units
{
    public class AttackingObject : MonoBehaviour
    {
        #region Variables
        [SerializeField] private protected int health;
        [SerializeField] private protected int damage;
        [SerializeField] private protected float attackSpeedPerSecond;

        protected float AttackTimer;
        #endregion

        #region Properties
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
    }
}