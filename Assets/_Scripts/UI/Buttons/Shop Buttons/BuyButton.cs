using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    [RequireComponent(typeof(Button))]
    public class BuyButton : MonoBehaviour
    {
        #region Variables
        [ShowInInspector] private ButtonBuyState _buttonState;
        [SerializeField] private GameObject[] states;
        private Button _button;
    
        public ButtonBuyState ButtonState => _buttonState;
        public Button Button => _button;
        #endregion
    
        #region Monobehaviour Callbacks
        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        #endregion
        
        public void SetButtonState(bool isEnoughMoney)
        {
            if (isEnoughMoney)
            {
                SetMoneyState();
            }
            else
            {
                SetADsState();
            }
        }

        private void SetADsState()
        {
            _buttonState = ButtonBuyState.BuyWithADs;
            SetUIState(_buttonState);
        }

        private void SetMoneyState()
        {
            _buttonState = ButtonBuyState.BuyWithMoney;
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
    }
}
