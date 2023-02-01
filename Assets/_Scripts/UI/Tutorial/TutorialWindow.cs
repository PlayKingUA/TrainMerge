using System;
using _Scripts.Tutorial;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Tutorial
{
    public class TutorialWindow : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Button buyWeapon;
        [SerializeField] private GameObject weaponPointer;
        [SerializeField] private Button startWave;
        [SerializeField] private GameObject startWavePointer;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button closeUpgradeButton;
        [SerializeField] private Button[] upgradeButtons;

        [Inject] private TutorialManager _tutorialManager;
        #endregion


        public void SetState(TutorialWindowState tutorialWindowState)
        {
            EnableBaseButtons(false);

            switch (tutorialWindowState)
            {
                case TutorialWindowState.BuyWeapon:
                    EnableBuyButton();
                    break;
                case TutorialWindowState.StartWave:
                    EnableStartWaveButton();
                    break;
                case TutorialWindowState.TapTutorial:
                    EnableTutorialPanel();
                    break;
                case TutorialWindowState.MergeWeapons:
                    break;
                case TutorialWindowState.UpgradePanel:
                    break;
                case TutorialWindowState.UpgradeDamage:
                    break;
                case TutorialWindowState.Nothing:
                    EnableBaseButtons(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tutorialWindowState), tutorialWindowState, null);
            }
            
        }

        private void EnableBaseButtons(bool isEnabled)
        {
            buyWeapon.interactable = isEnabled;
            weaponPointer.SetActive(false);
            startWave.interactable = isEnabled;
            startWavePointer.SetActive(false);
            upgradeButton.interactable = isEnabled;
            closeUpgradeButton.interactable = isEnabled;

            foreach (var button in upgradeButtons)
            {
                button.interactable = isEnabled;
            }
        }

        private void EnableBuyButton()
        {
            buyWeapon.interactable = true;
            weaponPointer.SetActive(true);
        }

        private void EnableStartWaveButton()
        {
            startWave.interactable = true;
            startWavePointer.SetActive(true);
            
            startWave.onClick.AddListener(() =>{startWavePointer.SetActive(false);});
        }

        private void EnableTutorialPanel()
        {
            
        }
    }
}