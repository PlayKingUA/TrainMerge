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

        [Inject] private GameStateManager _gameStateManager;
        #endregion

        #region Properties

        [ShowInInspector, ReadOnly]
        public float CoolDownSpeedUp => Mathf.Min(maxSpeedUp, 1f + (float) _tapsCount / tapsToMaxSpeedUp);
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
            _tapsCount++;
            yield return _wait;
            _tapsCount--;
        }
    }
}