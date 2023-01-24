using System;
using _Scripts.Interface;
using _Scripts.Levels;
using _Scripts.Money_Logic;
using _Scripts.Train;
using _Scripts.UI.Upgrade;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Units
{
    [RequireComponent(typeof(ChunkMovement), typeof(ZombieAnimationManager))]
    public sealed class Zombie : AttackingObject, IAlive
    {
        #region Variables
        [Space]
        [SerializeField] private int reward;
        [Space]
        [SerializeField] private Color damageColor;
        [Space]
        [ShowInInspector, ReadOnly] private UnitState _currentState;

        private ZombieAnimationManager _zombieAnimationManager;
        private ChunkMovement _chunkMovement;
        [Inject] private Train.Train _train;
        [Inject] private MoneyWallet _moneyWallet;
        [Inject] private UpgradeMenu _upgradeMenu;

        private Material[] _materials;
        private Tween[] _damageTweens;
        private const float DamageAnimationDuration = 0.1f;

        public bool IsDead { get; private set; }

        public event Action<int> GetDamageEvent;
        public event Action<Zombie> DeadEvent;
        #endregion

        #region Properties
        private bool CanAttack => Vector3.Distance(transform.position, _train.transform.position) < attackRadius;

        private int Reward => (int) (reward * _upgradeMenu.IncomeCoefficient);
        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            _zombieAnimationManager = GetComponent<ZombieAnimationManager>();
            _materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
            _damageTweens = new Tween[_materials.Length];
            
            _chunkMovement = GetComponent<ChunkMovement>();
        }

        protected override void Start()
        {
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
            if (_currentState == UnitState.Attack)
            {
                _chunkMovement.SetSpeed(_train.TrainSpeed);
            }
            else
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
                case UnitState.Run:
                    RunState();
                    break;
                case UnitState.Attack:
                    AttackState();
                    break;
                case UnitState.Idle:
                case UnitState.Victory:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RunState()
        {
            if (!CanAttack) return;
            ChangeState(UnitState.Attack);
        }

        private void AttackState()
        {
            if (AttackTimer < CoolDown || !CanAttack) 
                return;

            _zombieAnimationManager.SetAnimation(_currentState);
            AttackTimer = 0f;
        }
        #endregion

        public void InitMotion(Chunk firstChunk)
        {
            _chunkMovement.Init(firstChunk);
            _chunkMovement.ChangeState(true);
        }
        
        public void Attack()
        {
            _train.GetDamage(Damage);
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
            _moneyWallet.Add(Reward);
            gameObject.SetActive(false);
        }
        #endregion
    }
}
