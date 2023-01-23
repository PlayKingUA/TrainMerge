using _Scripts.Slot_Logic;
using Zenject;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class BuyWeaponButton : BuyButton
    {
        #region Variables
        [Inject] private SlotManager _slotManager;
        #endregion

        protected override bool CanBeBought => _slotManager.HasFreePlace();

        protected override void ClickEvent()
        {
            base.ClickEvent();
            _slotManager.CreateNewWeapon();
        }
    }
}