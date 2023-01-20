using System;
using _Scripts.Shop;
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
        [SerializeField] private CanvasGroup buyPanel;
        [SerializeField] private CanvasGroup upgradePanel;
        
        private const string DamageSaveKey = "DamageLevel";
        private const string AltSpeedSaveKey = "AltSpeedLevel";
        private const string IncomeSaveKey = "IncomeLevel";
        
        private int _damageLevel;
        private int _altSpeedLevel;
        private int _incomeLevel;
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
        
        public void UpgradeStat(UpgradeStats upgradeStats)
        {
            switch (upgradeStats)
            {
                case UpgradeStats.Damage:
                    _damageLevel++;
                    break;
                case UpgradeStats.AltSpeed:
                    _altSpeedLevel++;
                    break;
                case UpgradeStats.Income:
                    _incomeLevel++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(upgradeStats), upgradeStats, null);
            }

            Save();
        }
        
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(DamageSaveKey, _damageLevel);
            PlayerPrefs.SetInt(AltSpeedSaveKey, _altSpeedLevel);
            PlayerPrefs.SetInt(IncomeSaveKey, _incomeLevel);
        }

        private void Load()
        {
            _damageLevel = PlayerPrefs.GetInt(DamageSaveKey);
            _altSpeedLevel = PlayerPrefs.GetInt(AltSpeedSaveKey);
            _incomeLevel = PlayerPrefs.GetInt(IncomeSaveKey);
        }
        #endregion

    }
}