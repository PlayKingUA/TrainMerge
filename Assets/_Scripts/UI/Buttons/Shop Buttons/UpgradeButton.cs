using _Scripts.Shop;
using UnityEngine;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class UpgradeButton : BuyButton
    {
        #region Variables
        [SerializeField] private UpgradeStats upgradeStats;
        
        #endregion
        
        private void UpgradeIncome()
        {
            if (ButtonState == ButtonBuyState.BuyWithMoney)
            {
                //_moneyWallet.Get(_weaponPrice);
                
            }
            else
            {
                //ToDo show add
            }
        }

        private void Upgrade()
        {
            //UpgradeStat(upgradeStats);
        }
        
    }
}