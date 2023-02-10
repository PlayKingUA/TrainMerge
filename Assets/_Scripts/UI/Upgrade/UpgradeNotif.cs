using System.Linq;
using _Scripts.Money_Logic;
using _Scripts.UI.Buttons.Shop_Buttons;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class UpgradeNotif : MonoBehaviour
{
    [SerializeField] private BuyButton[] upgradeButtons;
    [SerializeField] private GameObject notifButton;

    [Inject] private MoneyWallet _moneyWallet;
    
    private void Start()
    {
        _moneyWallet.MoneyCountChanged += f =>
        {
            UpdateNotifState();
        };
        foreach (var button in upgradeButtons)
        {
            button.OnBought += UpdateNotifState;
        }
        Invoke(nameof(UpdateNotifState), 0.1f);
    }
    
    [Button("Update")]
    private void UpdateNotifState()
    {
        var isEnabled = upgradeButtons.Any(button => button.IsEnoughMoney);
        notifButton.SetActive(isEnabled);
    }
}
