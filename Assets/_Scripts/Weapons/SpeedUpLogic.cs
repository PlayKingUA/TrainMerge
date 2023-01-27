using System;
using System.Collections;
using _Scripts.Game_States;
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

        private WaitForSeconds _wait;
        private int _tapsCount;

        private int TapsCount
        {
            get => _tapsCount;
            set
            {
                OnTapCountChanged?.Invoke();
                _tapsCount = value;
            }
        }

        [Inject] private GameStateManager _gameStateManager;

        public event Action OnTapCountChanged;
        #endregion

        #region Properties

        [ShowInInspector, ReadOnly]
        public float CoolDownSpeedUp => Mathf.Min(maxSpeedUp, 1f + (float) TapsCount / tapsToMaxSpeedUp);
        #endregion

        #region Monobehavior Callbacks
        private void Start()
        {
            _wait = new WaitForSeconds(tapDuration);
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)
            &&_gameStateManager.CurrentState == GameState.Battle)
            {
                StartCoroutine(AddTap());
            }
        }
        #endregion

        private IEnumerator AddTap()
        {
            TapsCount++;
            yield return _wait;
            TapsCount--;
        }
    }
}