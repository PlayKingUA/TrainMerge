using System.Collections;
using _Scripts.Levels;
using _Scripts.Money_Logic;
using _Scripts.UI.Displays;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Buttons
{
    public class MultiplyButton : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float restartDelay;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private MultiplierBar multiplierBar;
        
        private Button _button;

        private int _reward;

        [Inject] private MoneyWallet _moneyWallet;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(MultiplyMoney);
            
            multiplierBar.OnValueChanged += () =>
            {
                UpdateAmount((int) (_reward * multiplierBar.GetMultiplayer()));
            };
        }
        #endregion

        public void SetReward(int reward)
        {
            _reward = reward;
            UpdateAmount(reward);
        }
        
        private void MultiplyMoney()
        {
            multiplierBar.StopPointer();
            _moneyWallet.Add((int) (_reward * (multiplierBar.GetMultiplayer() - 1)));
            //show money animation
            StartCoroutine(Restart());
        }

        private void UpdateAmount(int amount)
        {
            amountText.text = MoneyDisplay.MoneyText(amount);
        }
        
        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(restartDelay);
            LevelManager.RestartForce();
        }
    }
}