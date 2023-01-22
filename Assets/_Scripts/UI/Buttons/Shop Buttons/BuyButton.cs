using System;
using _Scripts.Money_Logic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    [RequireComponent(typeof(Button))]
    public class BuyButton : MonoBehaviour
    {
        #region Variables
        [ShowInInspector] private ButtonBuyState _buttonState;
        [SerializeField] private GameObject[] states;
        [SerializeField] private string saveKey = "WeaponPrice";
        [Space(10)]
        [SerializeField] protected int startPrise;
        [SerializeField] protected int maxPrise;
        [SerializeField] protected int levelsToMaxPrise;
        [SerializeField] private TextMeshProUGUI priseText;

        protected int CurrentLevel;
        
        [Inject] private MoneyWallet _moneyWallet;
        #endregion

        #region Properties

        private ButtonBuyState ButtonState => _buttonState;
        protected Button Button { get; private set; }

        private int CurrentPrise => 100;
        #endregion
    
        #region Monobehaviour Callbacks
        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
        }

        protected virtual void Start()
        {
            Load();
            CheckMoney(_moneyWallet.MoneyCount);
        }

        protected virtual void OnEnable()
        {
            _moneyWallet.MoneyCountChanged += CheckMoney;
        }

        protected virtual void OnDisable()
        {
            _moneyWallet.MoneyCountChanged -= CheckMoney;
        }
        #endregion
        
        #region Display
        private void SetButtonState(bool isEnoughMoney)
        {
            _buttonState = isEnoughMoney ? ButtonBuyState.BuyWithADs : ButtonBuyState.BuyWithMoney;
            SetUIState(_buttonState);
        }

        private void SetUIState(ButtonBuyState targetState)
        {
            foreach (var state in states)
            {
                state.SetActive(false);
            }

            states[(int)targetState].SetActive(true);
        }

        private void UpdatePrise()
        {
            priseText.text = CurrentPrise.ToString();
        }
        #endregion

        #region Click
        protected virtual void Click()
        {
            switch (ButtonState)
            {
                case ButtonBuyState.BuyWithMoney:
                    ClickEvent();
                    _moneyWallet.Get(CurrentPrise);
                    break;
                case ButtonBuyState.BuyWithADs:
                    //ToDo show add
                    break;
                case ButtonBuyState.MaxLevel:
                    return;
                default:
                    break;
            }
        }

        protected virtual void ClickEvent()
        {
            CurrentLevel++;
            UpdatePrise();
            Save();
        }
        #endregion
        
        #region Save/Load
        private void Save()
        {
            PlayerPrefs.SetInt(saveKey, CurrentLevel);
        }

        private void Load()
        {
            CurrentLevel = PlayerPrefs.GetInt(saveKey, startPrise);
            UpdatePrise();
        }
        #endregion

        private void CheckMoney(int moneyCount)
        {
            SetButtonState(moneyCount >= CurrentPrise);
        }
    }
}
