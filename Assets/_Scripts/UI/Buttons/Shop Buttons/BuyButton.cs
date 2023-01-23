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

        private Button _button;
        
        protected int CurrentLevel;
        
        [Inject] private MoneyWallet _moneyWallet;
        #endregion

        #region Properties
        private ButtonBuyState ButtonState => _buttonState;

        private int CurrentPrise =>
            (int) (startPrise + (maxPrise - startPrise) * ((float) CurrentLevel / levelsToMaxPrise));

        protected virtual bool CanBeBought => true;
        #endregion
    
        #region Monobehaviour Callbacks
        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Click);
        }

        protected virtual void Start()
        {
            Load();
            CheckMoney(_moneyWallet.MoneyCount);
            
            _moneyWallet.MoneyCountChanged += CheckMoney;
        }

        protected virtual void OnDisable()
        {
            _moneyWallet.MoneyCountChanged -= CheckMoney;
        }
        #endregion
        
        #region Display
        private void ChangeButtonState(int moneyCount)
        {
            _buttonState = (moneyCount >= CurrentPrise)
                ? ButtonBuyState.BuyWithMoney
                : ButtonBuyState.BuyWithADs;
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

        protected virtual void UpdateText()
        {
            priseText.text = CurrentPrise.ToString();
        }
        #endregion

        #region Click
        private void Click()
        {
            if (!CanBeBought)
                return;
            
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
            UpdateText();
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
            CurrentLevel = PlayerPrefs.GetInt(saveKey);
            UpdateText();
        }
        #endregion

        private void CheckMoney(int moneyCount)
        {
            ChangeButtonState(moneyCount);
        }
    }
}
