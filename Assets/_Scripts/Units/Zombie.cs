using System;
using _Scripts.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Units
{
    [RequireComponent(typeof(UnitMovement))]
    public sealed class Zombie : AttackingObject, IAlive
    {
        #region Variables
        [Space]
        [SerializeField] private float movementSpeed;
        [Space]
        [SerializeField] private float reward;
        [Space]
        [ShowInInspector, ReadOnly] private UnitState _currentState;

        private UnitMovement _unitMovement;

        public bool IsDead { get; private set; }
        public float Reward => reward;
        
        public event Action<Zombie> DeadEvent;
        public event Action DamageEvent;
        #endregion

        #region Monobehaviour Callbacks
        protected override void Start()
        {
            _unitMovement = GetComponent<UnitMovement>();
            _unitMovement.SetSpeed(movementSpeed);
            ChangeState(UnitState.Run);
            base.Start();
        }

        protected override void Update()
        {
            UpdateState();
        }
        #endregion
        
        #region States Logic
        public void ChangeState(UnitState newState)
        {
            if (IsDead)
                return;

            _currentState = newState;
        }

        private void UpdateState()
        {
            if (IsDead)
                return;

            switch (_currentState)
            {
                case UnitState.Idle:
                    IdleState();
                    break;
                case UnitState.Run:
                    RunState();
                    break;
                case UnitState.Attack:
                    AttackState();
                    break;
                case UnitState.Victory:
                    VictoryState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private  void IdleState()
        {
            _unitMovement.StopMove();
        }

        private void RunState()
        {
            _unitMovement.Move();

            if (!CanAttack()) return;
            ChangeState(UnitState.Attack);
            _unitMovement.StopMove();
        }

        private void AttackState()
        {
            _unitMovement.Move();
        }

        private  void VictoryState()
        {
            _unitMovement.StopMove();
        }
        #endregion

        private bool CanAttack()
        {
            // ToDo
            return false;
        }
        
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
            gameObject.SetActive(false);
        }
        #endregion
    }
}
