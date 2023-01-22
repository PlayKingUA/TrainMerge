using System;
using _Scripts.Interface;
using _Scripts.Money_Logic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Units
{
    [RequireComponent(typeof(UnitMovement), typeof(ZombieAnimationManager))]
    public sealed class Zombie : AttackingObject, IAlive
    {
        #region Variables
        [Space]
        [SerializeField] private float movementSpeed;
        [Space]
        [SerializeField] private int reward;
        [Space]
        [SerializeField] private Color damageColor;
        [Space]
        [ShowInInspector, ReadOnly] private UnitState _currentState;

        private ZombieAnimationManager _zombieAnimationManager;
        private UnitMovement _unitMovement;
        [Inject] private Train.Train _train;
        [Inject] private MoneyWallet _moneyWallet;

        private Material[] _materials;
        private Tween[] _damageTweens;
        private const float DamageAnimationDuration = 0.1f;

        public bool IsDead { get; private set; }

        public event Action<int> GetDamageEvent;
        public event Action<Zombie> DeadEvent;
        #endregion

        #region Properties
        private bool CanAttack => _unitMovement.DistanceFromTarget < attackRadius;
        #endregion

        #region Monobehaviour Callbacks
        protected override void Start()
        {
            _zombieAnimationManager = GetComponent<ZombieAnimationManager>();
            _materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
            _damageTweens = new Tween[_materials.Length];
                
            _unitMovement = GetComponent<UnitMovement>();
            _unitMovement.SetSpeed(movementSpeed);
            
            ChangeState(UnitState.Run);
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            UpdateState();
        }
        #endregion
        
        #region States Logic
        public void ChangeState(UnitState newState)
        {
            if (IsDead)
                return;

            _currentState = newState;
            if (_currentState != UnitState.Attack)
            {
                _zombieAnimationManager.SetAnimation(_currentState);
            }
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

            if (!CanAttack) return;
            ChangeState(UnitState.Attack);
        }

        private void AttackState()
        {
            _unitMovement.Move();
            if (AttackTimer < GetCoolDown() || !CanAttack) 
                return;

            _zombieAnimationManager.SetAnimation(_currentState);
            AttackTimer = 0f;
        }

        private  void VictoryState()
        {
            _unitMovement.StopMove();
        }
        #endregion

        public void Attack()
        {
            _train.GetDamage(damage);
        }
        
        #region Get Damage\Die
        public void GetDamage(int damagePoint)
        {
            var healthBefore = health;
            health = Mathf.Max(0, health -damagePoint);
            GetDamageEvent?.Invoke(healthBefore - health);

            if (health <= 0 && !IsDead)
                Die();
            else
            {
                if (_damageTweens[0] != null && _damageTweens[0].active)
                    return;
                for (var i = 0; i < _materials.Length; i++)
                {
                    _damageTweens[i].Kill();
                    _damageTweens[i] = _materials[i].DOColor(damageColor, "_Color", DamageAnimationDuration)
                        .SetLoops(2, LoopType.Yoyo);
                }
            }
        }

        public void Die()
        {
            if (IsDead)
                return;

            IsDead = true;

            DeadEvent?.Invoke(this);
            _moneyWallet.Add(reward);
            gameObject.SetActive(false);
        }
        #endregion
    }
}
