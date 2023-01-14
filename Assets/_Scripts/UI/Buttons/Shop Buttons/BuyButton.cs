using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    [RequireComponent(typeof(Button))]
    public class BuyButton : MonoBehaviour
    {
        #region Variables
        [SerializeField] private ButtonBuyState buttonState;
        [SerializeField] private GameObject[] states;
        private Button _button;
    
        public ButtonBuyState ButtonState => buttonState;
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
            buttonState = ButtonBuyState.BuyWithADs;
            SetUIState(buttonState);
        }

        private void SetMoneyState()
        {
            buttonState = ButtonBuyState.BuyWithMoney;
            SetUIState(buttonState);
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
