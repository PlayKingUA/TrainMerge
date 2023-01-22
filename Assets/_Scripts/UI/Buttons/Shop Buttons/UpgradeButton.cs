using _Scripts.Shop;
using _Scripts.UI.Upgrade;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class UpgradeButton : BuyButton
    {
        #region Variables
        [Space(10)]
        [SerializeField] private UpgradeStats upgradeStats;
        
        [SerializeField, Range(1f, 10f)] private float maxUpgrade;

        [Inject] private UpgradeMenu _upgradeMenu;
        #endregion
        
        #region Monobehavior Callbaacks
        #endregion

        #region Properties
        public float Coefficient => (float) CurrentLevel / levelsToMaxPrise * maxUpgrade;
        
        #endregion
        
    }
}