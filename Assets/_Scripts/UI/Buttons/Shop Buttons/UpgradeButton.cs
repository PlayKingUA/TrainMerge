using TMPro;
using UnityEngine;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class UpgradeButton : BuyButton
    {
        #region Variables
        [SerializeField, Range(1f, 10f)] private float maxUpgrade;
        [SerializeField] private float startUpgrade;
        [SerializeField] private float upgradeStep;
        [Space(10)] [SerializeField] private TextMeshProUGUI valueBefore;
        [SerializeField] private TextMeshProUGUI moreText;
        [SerializeField] private TextMeshProUGUI valueAfter;
        #endregion

        #region Properties
        public float Coefficient => GetCoefficient(CurrentLevel);

        protected override bool CanBeBought => base.CanBeBought && Coefficient < maxUpgrade;
        #endregion
        
        protected override void ChangeButtonState(float moneyCount)
        {
            if (Coefficient >= maxUpgrade)
            {
                _buttonState = ButtonBuyState.MaxLevel;
                SetUIState(_buttonState);
                return;
            }
            base.ChangeButtonState(moneyCount);
        }
        
        private float GetCoefficient(int level)
        {
            return startUpgrade + upgradeStep * level;
        }

        protected override void ClickEvent()
        {
            base.ClickEvent();
            ChangeButtonState(MoneyWallet.MoneyCount);
        }

        protected override void UpdateText()
        {
            base.UpdateText();
            
            moreText.gameObject.SetActive(CanBeBought);
            valueAfter.gameObject.SetActive(CanBeBought);
            
            valueBefore.text = Coefficient.ToString();
            valueAfter.text = GetCoefficient(CurrentLevel+ 1).ToString();
        }
    }
}