using System;
using _Scripts.UI.Buttons.Shop_Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Tutorial
{
    public class TutorialWindow : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject tutorialTextObject;
        [SerializeField] private TextMeshProUGUI tutorialText;
        [Space(10)]
        [SerializeField] private BuyWeaponButton buyWeapon;
        [SerializeField] private GameObject weaponPointer;
        [SerializeField] private Button startWave;
        [SerializeField] private GameObject startWavePointer;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private GameObject upgradeButtonPointer;
        [SerializeField] private Button closeUpgradeButton;
        [SerializeField] private UpgradeButton[] upgradeButtons;
        [SerializeField] private GameObject upgradeDamagePointer;

        public event Action OnWeaponBuy;
        public event Action OnUpgradeMenuOpen;
        public event Action OnUpgradeDamage;
        public event Action OnUpgradeClose;
        #endregion

        private void Awake()
        {
            buyWeapon.OnBought += () =>
            {
                OnWeaponBuy?.Invoke();
            };
        }

        public void SetState(TutorialWindowState tutorialWindowState, string infoText)
        {
            EnableBaseButtons(false);
            EnableTutorialText(infoText);

            switch (tutorialWindowState)
            {
                case TutorialWindowState.BuyWeapon:
                    EnableBuyButton();
                    break;
                case TutorialWindowState.StartWave:
                    EnableStartWaveButton();
                    break;
                case TutorialWindowState.TapTutorial:
                    break;
                case TutorialWindowState.MergeWeapons:
                    break;
                case TutorialWindowState.UpgradePanel:
                    EnableUpgradeButton();
                    break;
                case TutorialWindowState.UpgradeDamage:
                    EnableUpgradeDamage();
                    break;
                case TutorialWindowState.CLoseUpgradeWindow:
                    EnableCloseUpgradeWindowButton();
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
            buyWeapon.SetInteractable(isEnabled);
            weaponPointer.SetActive(false);
            startWave.interactable = isEnabled;
            startWavePointer.SetActive(false);
            upgradeButton.interactable = isEnabled;
            upgradeButtonPointer.SetActive(false);
            closeUpgradeButton.interactable = isEnabled;

            foreach (var button in upgradeButtons)
            {
                button.SetInteractable(isEnabled);
            }
            upgradeDamagePointer.SetActive(false);
        }

        private void EnableBuyButton()
        {
            buyWeapon.SetInteractable(true);
            weaponPointer.SetActive(true);
        }

        private void EnableStartWaveButton()
        {
            startWave.interactable = true;
            startWavePointer.SetActive(true);
            
            startWave.onClick.AddListener(() =>{startWavePointer.SetActive(false);});
        }

        private void EnableUpgradeButton()
        {
            upgradeButton.interactable = true;
            upgradeButtonPointer.SetActive(true);
            
            upgradeButton.onClick.AddListener(() =>
            {
                OnUpgradeMenuOpen?.Invoke();
            });
        }

        private void EnableUpgradeDamage()
        {
            upgradeDamagePointer.SetActive(true);
            upgradeButtons[0].SetInteractable(true);
            upgradeButtons[0].OnBought += () =>
            {
                OnUpgradeDamage?.Invoke();
            };
        }

        private void EnableCloseUpgradeWindowButton()
        {
            closeUpgradeButton.interactable = true;
            
            closeUpgradeButton.onClick.AddListener(() =>
            {
                OnUpgradeClose?.Invoke();
            });
        }

        private void EnableTutorialText(string text)
        {
            tutorialTextObject.SetActive(text.Length > 0);
            tutorialText.text = text;
        }
    }
}