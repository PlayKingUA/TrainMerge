﻿using System;
using _Scripts.Money_Logic;
using _Scripts.Slot_Logic;
using _Scripts.UI.Buttons.Shop_Buttons;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        #region Variables
        [SerializeField] private BuyButton weaponButton;
        [SerializeField] private BuyButton damageUpgradeButton;
        [SerializeField] private BuyButton altSpeedUpgradeButton;
        [SerializeField] private BuyButton incomeUpgradeButton;
        [Space]
        [SerializeField] private TextMeshProUGUI weaponPriceText;
        [SerializeField] private int startPrice;
        [SerializeField] private int deltaPrice;

        [Inject] private SlotManager _slotManager;
        [Inject] private MoneyWallet _moneyWallet;

        private const string SaveKey = "WeaponPrice";
        
        private int _weaponPrice;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            weaponButton.Button.onClick.AddListener(BuyWeapon);
            CheckMoney(_moneyWallet.MoneyCount);
            
            _moneyWallet.MoneyCountChanged += CheckMoney;
        }
        
        private void OnDisable()
        {
            _moneyWallet.MoneyCountChanged -= CheckMoney;
        }
        #endregion
        
        #region Buy Logic
        private void BuyWeapon()
        {
            if (!_slotManager.HasFreePlace()) return;
            
            if (weaponButton.ButtonState == ButtonBuyState.BuyWithMoney)
            {
                _moneyWallet.Get(_weaponPrice);

                _slotManager.CreateNewWeapon();

                _weaponPrice += deltaPrice;
                weaponPriceText.text = _weaponPrice.ToString();

                Save();
            }
            else if(weaponButton.ButtonState == ButtonBuyState.BuyWithADs)
            {
                //ToDo show add
            }
        }
        #endregion
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(SaveKey, _weaponPrice);
        }

        private void Load()
        {
            _weaponPrice = PlayerPrefs.GetInt(SaveKey, startPrice);

            weaponPriceText.text = _weaponPrice.ToString();
        }
        #endregion
        
        private void CheckMoney(int moneyCount)
        {
            weaponButton.SetButtonState(moneyCount >= _weaponPrice);
            damageUpgradeButton.SetButtonState(moneyCount >= _weaponPrice);
            altSpeedUpgradeButton.SetButtonState(moneyCount >= _weaponPrice);
            incomeUpgradeButton.SetButtonState(moneyCount >= _weaponPrice);
        }
    }
}