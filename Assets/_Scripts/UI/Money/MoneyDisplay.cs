using System;
using _Scripts.Money_Logic;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Money
{
    public class MoneyDisplay : MonoBehaviour
    {
        #region Variables
        [SerializeField] private TextMeshProUGUI moneyText;
        
        [Inject] private MoneyWallet _moneyWallet;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            _moneyWallet.MoneyCountChanged += Display;
        }
        #endregion
        
        private void Display(int value)
        {
            moneyText.text = value.ToString();
        }
    }
}