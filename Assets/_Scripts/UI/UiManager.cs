using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class UiManager : Singleton<UiManager>
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        public void UpdateWalletText(int money)
        {
            moneyText.text = money.ToString();
        }
    }
}