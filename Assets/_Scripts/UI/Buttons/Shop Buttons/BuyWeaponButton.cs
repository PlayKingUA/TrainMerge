using _Scripts.Slot_Logic;
using Zenject;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class BuyWeaponButton : BuyButton
    {
        #region Variables

        [Inject] private SlotManager _slotManager;

        #endregion

        #region Monobehavior Callbacks

        protected override void Start()
        {
            base.Start();
            Button.onClick.AddListener(() => { });
        }
        #endregion

        protected override void Click()
        {
            if (!_slotManager.HasFreePlace()) return;
            
            base.Click();
        }
    }
}