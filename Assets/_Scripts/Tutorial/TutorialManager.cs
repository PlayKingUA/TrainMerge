using System;
using System.Collections;
using _Scripts.Game_States;
using _Scripts.Input_Logic;
using _Scripts.Levels;
using _Scripts.Slot_Logic;
using _Scripts.UI.Tutorial;
using _Scripts.Units;
using _Scripts.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        #region Variables
        [ShowInInspector, ReadOnly] private TutorialStates _tutorialState;

        private string _infoText;
        
        private const string TutorialProgressKey = "TutorialProgressKey";

        [Inject] private TutorialWindow _tutorialWindow;
        [Inject] private LevelManager _levelManager;
        [Inject] private ZombieManager _zombieManager;
        [Inject] private SpeedUpLogic _speedUpLogic;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private SlotManager _slotManager;
        [Inject] private DragManager _dragManager;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            Load();
        }

        private void Start()
        {
            if (_tutorialState == TutorialStates.Finished)
            {
                return;
            }

            Invoke(nameof(ChangeState), 0.1f);
        }
        #endregion

        #region State logic
        private void ChangeState()
        {
            switch (_tutorialState)
            {
                case TutorialStates.BuyFirstWeapon:
                    _infoText = "Buy a turret!";
                    BuyWeapon();
                    break;
                case TutorialStates.StartFirstLevel:
                    _infoText = "";
                    StartLevel();
                    break;
                case TutorialStates.BuySecondWeapon:
                    _infoText = "Tap to buy another turret";
                    BuyWeapon();
                    break;
                case TutorialStates.MergeWeapons:
                    _infoText = "Merge your turrets to level them up!";
                    StartCoroutine(MergeTutorial());
                    break;
                case TutorialStates.StartSecondLevel:
                    _infoText = "";
                    StartLevel();
                    break;
                case TutorialStates.OpenUpgradeMenu:
                    _infoText = "Open upgrade menu";
                    OpenUpgradeMenu();
                    break;
                case TutorialStates.UpgradeDamage:
                    _infoText = "Tap to increase your turret's damage!";
                    UpgradeDamage();
                    break;
                case TutorialStates.CloseUpgradeWindow:
                    _infoText = "Tap elsewhere to close Upgrade menu";
                    CloseUpgradeMenu();
                    break;
                case TutorialStates.BuyThirdWeapon:
                    _infoText = "Buy even more turrets!";
                    BuyWeapon();
                    break;
                case TutorialStates.Finished:
                    _infoText = "";
                    FinishTutorial();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BuyWeapon()
        {
            _tutorialWindow.SetState(TutorialWindowState.BuyWeapon, _infoText);

            _tutorialWindow.OnWeaponBuy += BuyWeaponDelegate;
        }
        private void BuyWeaponDelegate()
        {
            NextState(true);
            _tutorialWindow.OnWeaponBuy -= BuyWeaponDelegate;
        }

        private void StartLevel()
        {
            _tutorialWindow.SetState(TutorialWindowState.StartWave, _infoText);

            _gameStateManager.AttackStarted += () =>
            {
                if (_tutorialState == TutorialStates.StartFirstLevel)
                {
                    _zombieManager.LastWaveStarted += TapTutorial;
                }
            };
            
            _gameStateManager.Victory += () =>
            {
                _tutorialState++;
                Save();
            };
        }
        
        private void TapTutorial()
        {
            _infoText = "Tap to increase attack speed of your turrets!";
            _tutorialWindow.SetState(TutorialWindowState.TapTutorial, _infoText);
            Time.timeScale = 0f;

            _speedUpLogic.OnTapCountChanged += TapDelegate;
        }
        private void TapDelegate()
        {
            if (!(_speedUpLogic.EffectPower >= 0.6f)) return;
                
            Time.timeScale = 1f;
            _infoText = "";
            _tutorialWindow.SetState(TutorialWindowState.Nothing, _infoText);
            _speedUpLogic.OnTapCountChanged -= TapDelegate;
        }

        private IEnumerator MergeTutorial()
        {
            // delay for enabling arrow on bought now weapon
            yield return null;
            _tutorialWindow.SetState(TutorialWindowState.MergeWeapons, _infoText);
            _slotManager.ShowTutorialArrows();

            _dragManager.OnMerge += MergeDelegate;
        }
        private void MergeDelegate()
        {
            _slotManager.ShowTutorialArrows(false);
            _tutorialState++;
            ChangeState();
            Save();
            _dragManager.OnMerge -= MergeDelegate;
        }
        
        private void OpenUpgradeMenu()
        {
            _tutorialWindow.SetState(TutorialWindowState.UpgradePanel, _infoText);

            _tutorialWindow.OnUpgradeMenuOpen += UpgradeMenuOpenDelegate;
        }
        private void UpgradeMenuOpenDelegate()
        {
            NextState(false);
            _tutorialWindow.OnUpgradeMenuOpen -= UpgradeMenuOpenDelegate;
        }
        
        private void UpgradeDamage()
        {
            _tutorialWindow.SetState(TutorialWindowState.UpgradeDamage, _infoText);
            _tutorialWindow.OnUpgradeDamage += UpgradeDelegate;
        }
        private void UpgradeDelegate()
        {
            NextState(true);
            _tutorialWindow.OnUpgradeDamage -= UpgradeDelegate;
        }

        private void CloseUpgradeMenu()
        {
            _tutorialWindow.SetState(TutorialWindowState.CLoseUpgradeWindow, _infoText);
            _tutorialWindow.OnUpgradeClose += CLoseUpgradeDelegate;
        }
        private void CLoseUpgradeDelegate()
        {
            NextState(true);
            _tutorialWindow.OnUpgradeClose -= CLoseUpgradeDelegate;
        }

        private void FinishTutorial()
        {
            _tutorialWindow.SetState(TutorialWindowState.Nothing, _infoText);
        }
        
        private void NextState(bool isSaving)
        {
            _tutorialState++;
            ChangeState();
            if (isSaving)
                Save();
        }
        #endregion
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(TutorialProgressKey, (int) _tutorialState);
        }
    
        private void Load()
        {
            _tutorialState = (TutorialStates) PlayerPrefs.GetInt(TutorialProgressKey, 0);
        }
        #endregion
    }
}
