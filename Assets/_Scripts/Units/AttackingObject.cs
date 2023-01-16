using UnityEngine;

namespace _Scripts.Units
{
    public class AttackingObject : MonoBehaviour
    {
        #region Variables
        [SerializeField] private protected float health;
        [SerializeField] private protected int damage;
        [SerializeField] private protected float attackSpeedPerSecond;
        #endregion
        
        #region Monobehaviour Callbacks
        protected virtual void Start(){}
        protected virtual void Update(){}
        #endregion
    }
}