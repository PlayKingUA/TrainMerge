using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Resources
{
    public class PlayerResources : Singleton<PlayerResources>
    {
        [SerializeField] private int startMoneyAmount;
        
        private const string MoneyKey = "Money";
        private static int GetMoney => PlayerPrefs.GetInt(MoneyKey);
        
        public static bool CheckMoneyAmount(int amount) => GetMoney >= amount;
        
        public static void SpendMoney(int amount)
        {
            PlayerPrefs.SetInt(MoneyKey, GetMoney - amount);
            UiManager.Instance.UpdateWalletText(GetMoney);
        }

        private void Start()
        {
            InitMoney();
        }
        
        private void InitMoney()
        {
            if (PlayerPrefs.HasKey(MoneyKey))
            {
                return;
            }
            PlayerPrefs.SetInt(MoneyKey, startMoneyAmount);
            UiManager.Instance.UpdateWalletText(GetMoney);
        }
    }
}