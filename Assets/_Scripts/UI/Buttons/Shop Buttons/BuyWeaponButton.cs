using _Scripts.Slot_Logic;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Buttons.Shop_Buttons
{
    public class BuyWeaponButton : BuyButton
    {
        #region Variables
        [SerializeField] private int weaponLevel;
        [Inject] private SlotManager _slotManager;
        #endregion

        protected override bool CanBeBought => base.CanBeBought && _slotManager.HasFreePlace();

        protected override void ClickEvent()
        {
            base.ClickEvent();
            _slotManager.CreateNewWeapon(weaponLevel - 1);
        }
    }
}