using _Scripts.Money_Logic;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Displays
{
    public class MoneyDisplay : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI moneyText;
        
        [Inject] private MoneyWallet _moneyWallet;
        
        private const string Format = "F1";
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _moneyWallet.MoneyCountChanged += Display;
        }
        #endregion
        
        private void Display(float value)
        {
            moneyText.text = MoneyText(value);
        }

        public static string MoneyText(float value)
        {
            var result = value.ToString("F0");
            if (value > 1e9)
            {
                result = (value / 1e9).ToString(Format) + 'B';
            }
            else if (value > 1e6)
            {
                result = (value / 1e6).ToString(Format) + 'M';
            }
            else if (value > 1e4)
            {
                result = (value / 1e3).ToString(Format) + 'k';
            }
            return result;
        }
    }
}