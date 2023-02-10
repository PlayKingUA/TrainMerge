using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Money_Logic;
using _Scripts.UI.Buttons.Shop_Buttons;
using UnityEngine;
using Zenject;

public class UpgradeNotif : MonoBehaviour
{
    [SerializeField] private BuyButton[] upgradeButtons;
    [SerializeField] private GameObject notifButton;

    [Inject] private MoneyWallet _moneyWallet;
    
    private void Start()
    {
        _moneyWallet.MoneyCountChanged += UpdateNotifState;
        UpdateNotifState();
    }
    
    private void UpdateNotifState(float moneyCount = 0)
    {
        var isEnabled = upgradeButtons.Any(button => button.IsEnoughMoney);
        notifButton.SetActive(isEnabled);
    }
}
