using System;
using _Scripts.Game_States;
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

        private const string TutorialProgressKey = "TutorialProgressKey";

        [Inject] private TutorialWindow _tutorialWindow;
        [Inject] private ZombieManager _zombieManager;
        [Inject] private SpeedUpLogic _speedUpLogic;
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private WeaponManager _weaponManager;
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
            
            ChangeState();
        }
        #endregion

        #region State logic
        private void ChangeState()
        {
            switch (_tutorialState)
            {
                case TutorialStates.BuyFirstWeapon:
                    BuyWeapon();
                    break;
                case TutorialStates.StartFirstLevel:
                    StartLevel();
                    break;
                case TutorialStates.BuySecondWeapon:
                    BuyWeapon();
                    break;
                case TutorialStates.MergeWeapons:
                    MergeTutorial();
                    break;
                case TutorialStates.StartSecondLevel:
                    StartLevel();
                    break;
                case TutorialStates.OpenUpgradeMenu:
                    OpenUpgradeMenu();
                    break;
                case TutorialStates.UpgradeDamage:
                    UpgradeDamage();
                    break;
                case TutorialStates.Finished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BuyWeapon()
        {
            _tutorialWindow.SetState(TutorialWindowState.BuyWeapon);

            _weaponManager.OnNewWeapon += () =>
            {
                _tutorialState++;
                ChangeState();
                
                Save();
            };
        }

        private void StartLevel()
        {
            _tutorialWindow.SetState(TutorialWindowState.StartWave);

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
            _tutorialWindow.SetState(TutorialWindowState.TapTutorial);
            Time.timeScale = 0f;

            _speedUpLogic.OnTapCountChanged += () =>
            {
                if (!(_speedUpLogic.EffectPower >= 0.6f)) return;
                
                Time.timeScale = 1f;
                _tutorialWindow.SetState(TutorialWindowState.Nothing);
            };
        }

        private void MergeTutorial()
        {
            
        }
        
        private void OpenUpgradeMenu()
        {
            
        }
        
        private void UpgradeDamage()
        {
            // on upgrade
            _tutorialWindow.SetState(TutorialWindowState.Nothing);
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
