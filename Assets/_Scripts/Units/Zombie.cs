using System;
using System.Collections;
using _Scripts.Game_States;
using _Scripts.Helpers;
using _Scripts.Interface;
using _Scripts.Levels;
using _Scripts.Train;
using DG.Tweening;
using QFSW.MOP2;
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
        [SerializeField] private float hpPerLevel;
        [SerializeField] private float dmgPerLevel;
        [Space(10)]
        [SerializeField] private ZombieType zombieType;
        [SerializeField] private Transform shootPoint;
        [Space]
        [SerializeField] private Color damageColor;
        [SerializeField] private ObjectPool damageText;
        [Space] 
        [SerializeField] private float climbDuration = 0.7f;
        [SerializeField] private float speedOnTrain = 3f;
        [Space]
        [ShowInInspector, ReadOnly] private UnitState _currentState;

        private ZombieAnimationManager _zombieAnimationManager;
        private ChunkMovement _chunkMovement;
        private RagdollController _ragdollController;
        private MasterObjectPooler _masterObjectPooler;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private Train.Train _train;
        [Inject] private CoinsAnimation _coinsAnimation;
        

        private Material[] _materials;
        private Tween[] _damageTweens;
        private const float DamageAnimationDuration = 0.15f;

        public bool IsDead { get; private set; }

        public event Action<int> GetDamageEvent;
        public event Action<Zombie> DeadEvent;
        #endregion

        #region Properties
        public Transform ShootPoint => shootPoint;
        public ZombieType ZombieType => zombieType;
        
        public int StartHp(int currentLevel) => (int) (Health + (currentLevel - 1) * hpPerLevel);

        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _masterObjectPooler = MasterObjectPooler.Instance;
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
            if (!other.TryGetComponent(out Train.Train train)) return;
            
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
                StartCoroutine(ClimbTrain());
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
            if (_gameStateManager.CurrentState == GameState.Fail)
            {
                ChangeState(UnitState.Victory);
            }
            
            if (AttackTimer < CoolDown) 
                return;

            _zombieAnimationManager.SetAnimation(_currentState);
            AttackTimer = 0f;
        }

        private IEnumerator ClimbTrain()
        {
            _chunkMovement.ChangeState(false);
            
            var targetPosition = transform.position;
            targetPosition.y = _train.ClimbingHeight.position.y;
            var tween = transform.DOMove(targetPosition, climbDuration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(climbDuration);
            tween.Kill();
            _zombieAnimationManager.SetAnimation(UnitState.Run);
            targetPosition += transform.forward * 10f;
            transform.DOMove(targetPosition, speedOnTrain).SetSpeedBased();
        }
        #endregion

        public void Init(Chunk firstChunk, float deltaX, float speedMultiplier)
        {
            _chunkMovement.Init(firstChunk);
            _chunkMovement.ChangeState(true);
            _chunkMovement.SetSpeed(_chunkMovement.MovementSpeed * speedMultiplier);
            
            transform.GetChild(0).localPosition = new Vector3(deltaX, 0, 0);
            var boxCollider = GetComponent<BoxCollider>();
            var center = boxCollider.center;
            boxCollider.center = new Vector3(deltaX, center.y, center.z);

            Health = (int) (Health + (_levelManager.CurrentLevel - 1) * hpPerLevel);
            Damage = (int) (Damage + (_levelManager.CurrentLevel - 1) * dmgPerLevel);
        }
        
        public void Attack()
        {
            Debug.Log("Attack with damage " + Damage);
            _train.GetDamage(Damage);
        }
        
        #region Get Damage\Die
        public void GetDamage(int damagePoint)
        {
            if (IsDead)
                return;
            
            var healthBefore = Health;
            Health = Mathf.Max(0, Health - damagePoint);
            var damage = healthBefore - Health;
            GetDamageEvent?.Invoke(damage);
            CreateDamageText(damage);
            _coinsAnimation.CollectCoins(shootPoint, damage);

            if (Health <= 0)
                Die();
            
            for (var i = 0; i < _materials.Length; i++)
            {
                _damageTweens[i].Rewind();
                _damageTweens[i].Kill();
                _damageTweens[i] = _materials[i].DOColor(damageColor, "_Color", DamageAnimationDuration)
                    .SetLoops(2, LoopType.Yoyo);
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

        private void CreateDamageText(int damage)
        {
            var text =
                _masterObjectPooler.GetObjectComponent<DamageText>(damageText.PoolName, shootPoint.position, transform.rotation);
            text.SetText(damage.ToString());
        }
        #endregion
    }
}
