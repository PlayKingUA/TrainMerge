using System;
using UnityEngine;

namespace _Scripts.Money_Logic
{
    public class MoneyWallet : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int moneyCount;
        private const string SaveKey = "Money";

        public int MoneyCount => moneyCount;
        
        public event Action<int> MoneyCountChanged;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            Load();
        }
        #endregion

        #region Add/Get
        public void Add(int addAmount)
        {
            moneyCount += addAmount;
            MoneyCountChanged?.Invoke(moneyCount);
            
            Save();
        }

        public void Get(int getAmount)
        {
            moneyCount -= getAmount;
            MoneyCountChanged?.Invoke(moneyCount);

            Save();
        }
        #endregion
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(SaveKey, moneyCount);
        }

        private void Load()
        {
            moneyCount = PlayerPrefs.GetInt(SaveKey, moneyCount);
            
            MoneyCountChanged?.Invoke(moneyCount);
        }
        #endregion
    }
}