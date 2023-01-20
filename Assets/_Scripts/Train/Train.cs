using System;
using _Scripts.Game_States;
using _Scripts.Interface;
using _Scripts.Slot_Logic;
using _Scripts.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts.Train
{
    [RequireComponent(typeof(TrainMovement))]
    public class Train : MonoBehaviour, IAlive
    {
        #region Variables
        [SerializeField] private Transform zombiePositionFrom;
        [SerializeField] private Transform zombiePositionTo;
        [SerializeField] private float health;
        [SerializeField] private float movementSpeed;

        private TrainMovement _trainMovement;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] private SlotManager _slotManager;
        [Inject] private WeaponManager _weaponManager;

        public bool IsDead { get; private set;}
        [ShowInInspector, ReadOnly] public float MaxHealth { get; private set;}
        public float CurrentHealth { get; private set; }
        
        public event Action HpChanged;
        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            _trainMovement = GetComponent<TrainMovement>();
            //_trainMovement.SetSpeed(movementSpeed);

            UpdateMaxHealth();
            _weaponManager.OnNewWeapon += UpdateMaxHealth;
        }

        private void Update()
        {
            if (IsDead)
            {
                return;
            }
            //_trainMovement.Move();
        }
        #endregion

        #region Zombie Position
        public Vector3 GetTargetZombiePosition(float x)
        {
            var targetPosition = zombiePositionFrom.position;
            targetPosition.x = x;
            return targetPosition;
        }

        public float GetZombiePositionX()
        {
            return Random.Range(zombiePositionFrom.position.x,
                zombiePositionTo.position.x);
        }
        #endregion
        
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