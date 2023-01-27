using System;
using _Scripts.Game_States;
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
    [RequireComponent(typeof(ChunkMovement), 
        typeof(ZombieAnimationManager), 
        typeof(RagdollController))]
    public sealed class Zombie : AttackingObject, IAlive
    {
        #region Variables
        [Space]
        [SerializeField] private ZombieType zombieType;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private int reward;
        [Space]
        [SerializeField] private Color damageColor;
        [Space]
        [ShowInInspector, ReadOnly] private UnitState _currentState;

        private ZombieAnimationManager _zombieAnimationManager;
        private ChunkMovement _chunkMovement;
        private RagdollController _ragdollController;
        [Inject] private GameStateManager _gameStateManager;
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
        public Transform ShootPoint => shootPoint;
        public ZombieType ZombieType => zombieType;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _zombieAnimationManager = GetComponent<ZombieAnimationManager>();
            _ragdollController = GetComponent<RagdollController>();
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
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Train.Train train)
                || _gameStateManager.CurrentState != GameState.Battle) return;
            
            ChangeState(UnitState.Attack);
            transform.parent = _train.transform;
            _chunkMovement.ChangeState(false);
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

            if (_currentState == UnitState.Victory)
            {
                transform.parent = null;
                _chunkMovement.ChangeState(false);
            }
        }

        private void UpdateState()
        {
            if (IsDead)
                return;

            if (_currentState == UnitState.Attack)
            {
                AttackState();
            }
        }

        private void AttackState()
        {
            if (AttackTimer < CoolDown) 
                return;

            _zombieAnimationManager.SetAnimation(_currentState);
            AttackTimer = 0f;
        }
        #endregion

        public void InitMotion(Chunk firstChunk, float deltaX)
        {
            _chunkMovement.Init(firstChunk);
            _chunkMovement.ChangeState(true);
            
            transform.GetChild(0).localPosition = new Vector3(deltaX, 0, 0);
            var boxCollider = GetComponent<BoxCollider>();
            var targetCenter = boxCollider.center;
            targetCenter.x = deltaX;
            boxCollider.center = targetCenter;
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
            
            transform.parent = null;
            _chunkMovement.ChangeState(false);
            
            _zombieAnimationManager.DisableAnimator();
            _ragdollController.EnableRagdoll(true);
        }
        #endregion
    }
}
