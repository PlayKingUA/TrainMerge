using System;
using System.Collections;
using _Scripts.Game_States;
using _Scripts.Interface;
using _Scripts.Levels;
using _Scripts.Slot_Logic;
using _Scripts.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Train
{
    [RequireComponent(typeof(ChunkMovement))]
    public class Train : MonoBehaviour, IAlive
    {
        #region Variables
        [SerializeField] private float health;
        [SerializeField] private float speedForDistance;
        [SerializeField] private float speedUp;
        [Space(10)] 
        [SerializeField] private float menuSpeed;
        [SerializeField] private float gameSpeed;

        private ChunkMovement _chunkMovement;
        private float _startPlayTime;
        
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private SpeedUpLogic _speedUpLogic;
        [Inject] private SlotManager _slotManager;
        [Inject] private WeaponManager _weaponManager;

        [ShowInInspector, ReadOnly] public float MaxHealth { get; private set;}
        public float CurrentHealth { get; private set; }
        public bool IsDead { get; private set;}
        public float TrainSpeed => _chunkMovement.MovementSpeed;
        
        public event Action HpChanged;
        public event Action<float> DistanceChanged;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _chunkMovement = GetComponent<ChunkMovement>();
        }

        private void Start()
        {
            UpdateMaxHealth();
            _weaponManager.OnNewWeapon += UpdateMaxHealth;
            
            _gameStateManager.AttackStarted += () =>
            {
                _startPlayTime = Time.time;
                _chunkMovement.SetSpeed(gameSpeed);
            };
            _gameStateManager.Fail += () =>
            {
                _chunkMovement.ChangeState(false);
            };

            var startMotionSpeed = gameSpeed;
            _speedUpLogic.OnTapCountChanged += () =>
            {
                _chunkMovement.SetSpeed(startMotionSpeed +
                                        (startMotionSpeed * speedUp - startMotionSpeed) * _speedUpLogic.EffectPower);
            };

            StartCoroutine(StartMotion());
        }

        private void Update()
        {
            if (_gameStateManager.CurrentState == GameState.Battle)
            {
                DistanceChanged?.Invoke((Time.time - _startPlayTime) * speedForDistance);
            }
        }
        #endregion

        private IEnumerator StartMotion()
        {
            // wait for init
            yield return new WaitForSeconds(0.1f);
            _chunkMovement.SetSpeed(menuSpeed);
            _chunkMovement.ChangeState(true);
        }
        
        public void InitMotion(Chunk firstChunk)
        {
            _chunkMovement.Init(firstChunk);
        }
        
        private void UpdateMaxHealth()
        {
            MaxHealth = health + _slotManager.WeaponsHealthSum;
            CurrentHealth = MaxHealth;
            HpChanged?.Invoke();
        }
        
        #region Get Damage\Die
        public void GetDamage(int damagePoint)
        {
            CurrentHealth -= damagePoint;
            CurrentHealth = Mathf.Max(0, CurrentHealth - damagePoint);
            HpChanged?.Invoke();

            if (CurrentHealth <= 0 && !IsDead)
                Die();
        }
        
        public void Die()
        {
            if (IsDead)
                return;

            IsDead = true;
            _gameStateManager.ChangeState(GameState.Fail);
        }
        #endregion
    }
}