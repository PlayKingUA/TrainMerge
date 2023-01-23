using TMPro;
using UnityEngine;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class UpgradeButton : BuyButton
    {
        #region Variables
        [SerializeField, Range(1f, 10f)] private float maxUpgrade;
        [Space(10)] [SerializeField] private TextMeshProUGUI valueBefore;
        [SerializeField] private TextMeshProUGUI moreText;
        [SerializeField] private TextMeshProUGUI valueAfter;
        #endregion

        #region Properties
        public float Coefficient => GetCoefficient(CurrentLevel);

        protected override bool CanBeBought => CurrentLevel < levelsToMaxPrise;
        #endregion
        
        private float GetCoefficient(int level)
        {
            return 1f + (float) level / levelsToMaxPrise * (maxUpgrade - 1f);
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