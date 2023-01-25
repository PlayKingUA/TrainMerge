using _Scripts.UI.Buttons.Shop_Buttons;
using _Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Upgrade
{
    public class UpgradeMenu : MonoBehaviour
    {
        #region Variables
        [Space(10)]
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button closeUpgradeButton;
        [Space(10)]
        [SerializeField] private UpgradeButton damageUpgrade;
        [SerializeField] private UpgradeButton speedUpgrade;
        [SerializeField] private UpgradeButton incomeUpgrade;
        [Space(10)]
        [SerializeField] private CanvasGroup buyPanel;
        [SerializeField] private CanvasGroup upgradePanel;
        #endregion

        #region Properties
        public float DamageCoefficient => damageUpgrade.Coefficient;
        public float AltSpeedCoefficient => speedUpgrade.Coefficient;
        public float IncomeCoefficient => incomeUpgrade.Coefficient;

        #endregion

        #region Monobehavior Callbacks
        private void Awake()
        {
            upgradeButton.onClick.AddListener(() =>
            {
                ShopWindowSwipe(false);
            });
            closeUpgradeButton.onClick.AddListener(() =>
            {
                ShopWindowSwipe(true);
            });
        }

        #endregion

        private void ShopWindowSwipe(bool isBuyPanel)
        {
            WindowsManager.CanvasGroupSwap(buyPanel, isBuyPanel);
            WindowsManager.CanvasGroupSwap(upgradePanel, !isBuyPanel);
        }
        
    }
}