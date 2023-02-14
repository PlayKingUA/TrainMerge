using System;
using _Scripts.Game_States;
using _Scripts.Slot_Logic;
using _Scripts.UI.Upgrade;
using _Scripts.Units;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Weapons
{
    public class Weapon : AttackingObject
    {
        #region Variables
        [Space] 
        [SerializeField] private GameObject appearFx;
        [SerializeField] private GameObject destroyFx;
        [SerializeField] private Transform gunTransform;
        [Space(10)]
        [ShowInInspector, ReadOnly] private WeaponState _currentState;
        [ShowInInspector, ReadOnly] private int _level;
        [Space(10)]
        [SerializeField] private MeshRenderer gunRenderer;
        [SerializeField] private Material transparentMaterial;
        [SerializeField] private Color DestoyredColor;

        [Inject] private GameStateManager _gameStateManager;
        [Inject] protected ZombieManager ZombieManager;
        [Inject] private UpgradeMenu _upgradeMenu;
        [Inject] private SpeedUpLogic _speedUpLogic;

        protected WeaponAnimator WeaponAnimator;
        private Quaternion _startRotation;
        private Material _gunMaterial;
        private Tweener _tween;

        private float _maxShakeStrength = 0.05f;
        private float _destoryColorChangeDuration = 0.35f;
        
        [ShowInInspector, ReadOnly] private protected Zombie TargetZombie;
        #endregion

        #region Properties
        public int Level => _level;
        public GameObject AppearFx => appearFx;

        private protected bool CanAttack => TargetZombie != null &&
                                            Vector3.Distance(transform.position, TargetZombie.transform.position) <=
                                            attackRadius;

        protected override float CoolDown =>
            base.CoolDown / _speedUpLogic.CoolDownSpeedUp / _upgradeMenu.AltSpeedCoefficient;
        
        protected override int Damage => (int) (base.Damage * _upgradeMenu.DamageCoefficient);

        #endregion
        
        #region Monobehaviour Callbacks
        protected override void Start()
        {
            base.Start();
            WeaponAnimator = GetComponent<WeaponAnimator>();
            
            ChangeState(WeaponState.Idle);
            _startRotation = gunTransform.rotation;

            _gunMaterial = gunRenderer.material;

            _speedUpLogic.OnTapCountChanged += Shake;

            _gameStateManager.Fail += DestroyWeapon;
        }

        protected override void Update()
        {
            base.Update();
            UpdateState();
        }

        private void OnDisable()
        {
            _speedUpLogic.OnTapCountChanged -= Shake;
            _tween.Kill();
        }

        #endregion
        
        #region States Logic
        public void ChangeState(WeaponState newState)
        {
            _currentState = newState;
        }
        
        private void UpdateState()
        {
            switch (_currentState)
            {
                case WeaponState.Idle:
                    IdleState();
                    break;
                case WeaponState.Attack:
                    AttackState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void IdleState()
        {
            WeaponAnimator.SetAnimation(WeaponState.Idle);
        }

        protected virtual void AttackState()
        {
            Rotate();
        }
        #endregion

        public void SetLevel(int level)
        {
            _level = level;
        }

        #region Motion
        public void ReturnToPreviousPos(Slot previousSlot)
        {
            previousSlot.Refresh(this, previousSlot);
        }

        private void Rotate()
        {
            UpdateTargetZombie();
            
            if (TargetZombie == null)
                return;
            
            var direction = TargetZombie.ShootPoint.position - gunTransform.position;
            var rotateY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var targetRotation = Quaternion.Euler(0, rotateY, 0);

            if (Quaternion.Angle(gunTransform.rotation, targetRotation) == 0) return;
            var t =  Mathf.Clamp(Time.deltaTime * 10, 0f, 0.99f);
            gunTransform.rotation = Quaternion.Lerp(gunTransform.rotation,
                targetRotation, t);
        }

        private void Shake()
        {
            var targetColor = new Color(_speedUpLogic.EffectPower, 0, 0);
            _gunMaterial.SetColor("_EmissionColor", targetColor);
            _gunMaterial.EnableKeyword("_EmissionColor");
            
            _tween.Rewind();
            _tween.Kill();
            if (_speedUpLogic.EffectPower != 0)
            {
                _tween = transform.
                    DOShakePosition(_speedUpLogic.EffectDuration, _speedUpLogic.EffectPower * _maxShakeStrength)
                    .SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
            }
        }
        #endregion

        public void SetGreenColor(bool isGreen)
        {
            gunRenderer.material = isGreen ? transparentMaterial : _gunMaterial;
        }

        private void DestroyWeapon()
        {
            if (destroyFx == null)
                return;
            /*destroyFx.SetActive(true);
            gunRenderer.material.DOColor(DestoyredColor, _destoryColorChangeDuration);*/
            WeaponAnimator.SetAnimation(WeaponState.Death);
        }
        
        private void UpdateTargetZombie()
        {
            TargetZombie = ZombieManager.GetNearestZombie(transform);
        }
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}