using System;
using _Scripts.Interface;
using UnityEngine;

namespace _Scripts.Train
{
    [RequireComponent(typeof(TrainMovement))]
    public class Train : MonoBehaviour, IAlive
    {
        #region Variables
        [SerializeField] private float health;
        [SerializeField] private float movementSpeed;

        private TrainMovement _trainMovement;
        
        public bool IsDead { get; private set; }

        public event Action<IAlive> DeadEvent;
        public event Action DamageEvent;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _trainMovement = GetComponent<TrainMovement>();
        }

        private void Update()
        {
            if (IsDead)
            {
                return;
            }
            _trainMovement.Move();
        }

        #endregion

        #region Get Damage\Die
        public void GetDamage(int damagePoint)
        {
            health -= damagePoint;
            DamageEvent?.Invoke();

            if (health <= 0 && !IsDead)
                Die();
        }
        
        public void Die()
        {
            if (IsDead)
                return;

            IsDead = true;

            DeadEvent?.Invoke(this);
        }
        #endregion
    }
}