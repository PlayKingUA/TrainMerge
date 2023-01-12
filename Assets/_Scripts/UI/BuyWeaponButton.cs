using _Scripts.Platforms;
using _Scripts.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class BuyWeaponButton : MonoBehaviour
    {
        [SerializeField] private int weaponCost;
        [SerializeField] private TextMeshProUGUI priceText;
        private Button _buyWeaponButton;

        private void Start()
        {
            _buyWeaponButton = GetComponent<Button>();
            _buyWeaponButton.onClick.AddListener(BuyButtonListener);
            UpdateButton();
        }

        private void UpdateButton()
        {
            _buyWeaponButton.interactable = PlayerResources.CheckMoneyAmount(weaponCost);
            priceText.text = weaponCost.ToString();
        }

        private void BuyButtonListener()
        {
            PlatformsManager.Instance.CreateNewWeapon();
            PlayerResources.SpendMoney(MoneyPrice);
            UpdateButton();
        }

        private int MoneyPrice => weaponCost;
    }
}