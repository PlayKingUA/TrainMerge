using System;
using UnityEngine;

namespace _Scripts.Money_Logic
{
    public class MoneyWallet : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float moneyCount;
        private const string SaveKey = "Money";

        public float MoneyCount => moneyCount;
        
        public event Action<float> MoneyCountChanged;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Start()
        {
            Load();
        }
        #endregion

        #region Add/Get
        public void Add(float addAmount)
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
            PlayerPrefs.SetFloat(SaveKey, moneyCount);
        }

        private void Load()
        {
            moneyCount = PlayerPrefs.GetFloat(SaveKey, moneyCount);
            
            MoneyCountChanged?.Invoke(moneyCount);
        }
        #endregion
    }
}