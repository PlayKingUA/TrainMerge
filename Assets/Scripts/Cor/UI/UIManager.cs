using UnityEngine;

namespace PlayKing.Cor
{
    public class UIManager : MonoBehaviour
    {
        #region Singelton

        public static UIManager Instance;

       private void Awake()
       {
            Instance = this;
       }

        #endregion

        [Header("Screens")]
        [SerializeField] GameObject joystickScreen;
        [SerializeField] GameObject startScreen;
        [SerializeField] GameObject settingsScreen;
        [SerializeField] GameObject moneyScreen;
        [SerializeField] GameObject leaderboardScreen;
        [SerializeField] GameObject loseScreen;
        [SerializeField] GameObject winScreen;

        public void JoystickScreen(bool isActive)
        {
            joystickScreen.SetActive(isActive);   
        }

        public void StartScreen(bool isActive)
        {
            startScreen.SetActive(isActive);   
        }

        public void SettingsScreen(bool isActive)
        {
            settingsScreen.SetActive(isActive);
        }

        public void MoneyScreen(bool isActive)
        {
            moneyScreen.SetActive(isActive);
        }

        public void LeaderboardScreen(bool isActive)
        {
            leaderboardScreen.SetActive(isActive);   
        }

        public void LoseScreen(bool isActive)
        {
            loseScreen.SetActive(isActive);   
        }

        public void WinScreen(bool isActive)
        {
            winScreen.SetActive(isActive);   
        }
    }
}
