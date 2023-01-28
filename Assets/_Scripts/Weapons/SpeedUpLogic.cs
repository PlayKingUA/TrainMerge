using System;
using System.Collections;
using _Scripts.Game_States;
using _Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Weapons
{
    public class SpeedUpLogic : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int tapsToMaxSpeedUp = 10;
        [SerializeField] private float maxSpeedUp = 2f;
        [SerializeField] private float tapDuration = 3f;
        [SerializeField] private GameObject notification;

        private WaitForSeconds _wait;
        private int _tapsCount;
        private bool _isEnabled;
        
        [Inject] private ZombieManager _zombieManager;
        [Inject] private GameStateManager _gameStateManager;

        public event Action OnTapCountChanged;
        #endregion

        #region Properties

        [ShowInInspector, ReadOnly]
        public float CoolDownSpeedUp =>  1f + EffectPower * (maxSpeedUp - 1f);
        public float EffectPower => Mathf.Clamp((float) _tapsCount / tapsToMaxSpeedUp, 0f, 1f);
        #endregion

        #region Monobehavior Callbacks
        private void Start()
        {
            _wait = new WaitForSeconds(tapDuration);
            _zombieManager.LastWaveStarted += ()=> { EnableTaps(true); };

            _gameStateManager.Victory += () => { EnableTaps(false); };
            _gameStateManager.Fail += () => { EnableTaps(false); };
        }
        
        private void Update()
        {
            if (!_isEnabled)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(AddTap());
            }
        }
        #endregion

        private IEnumerator AddTap()
        {
            _tapsCount++;
            OnTapCountChanged?.Invoke();
            yield return _wait;
            _tapsCount--;
            OnTapCountChanged?.Invoke();
        }

        private void EnableTaps(bool isEnabled)
        {
            _isEnabled = isEnabled;
            notification.SetActive(isEnabled);
        }
    }
}