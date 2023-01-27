using System;
using System.Linq;
using _Scripts.Game_States;
using _Scripts.UI.Buttons;
using _Scripts.UI.Displays;
using _Scripts.UI.Upgrade;
using _Scripts.Units;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Windows
{
    public class ResultsScreen : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject winResults;
        [SerializeField] private GameObject loseResults;
        [Space(10)]
        [SerializeField] private ZombieTable zombieTable;

        [SerializeField] private TextMeshProUGUI rewardText;
        [SerializeField] private MultiplyButton multiplyButton;
        [SerializeField] private TextMeshProUGUI getOnlyButtonText;


        [Inject] private ZombieManager _zombieManager;
        [Inject] private UpgradeMenu _upgradeMenu;
        [Inject] private GameStateManager _gameStateManager;

        #endregion
        
        #region Monobehavior callbacks
        private void Start()
        {
            _gameStateManager.Victory += () => { UpdateResults(true); };
            _gameStateManager.Fail += () => { UpdateResults(false); };
        }
        #endregion

        private void UpdateResults(bool isWin)
        {
            winResults.SetActive(isWin);
            loseResults.SetActive(!isWin);
            
            zombieTable.UpdatePanel(_zombieManager.DeadZombies);

            var reward = (int)(_zombieManager.LostHp * _upgradeMenu.IncomeCoefficient);

            multiplyButton.SetReward(reward);
            rewardText.text = MoneyDisplay.MoneyText(reward);
            getOnlyButtonText.text = "Get only" + MoneyDisplay.MoneyText(reward);
        }
    }
}