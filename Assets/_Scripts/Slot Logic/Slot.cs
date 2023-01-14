using _Scripts.Weapons;
using UnityEngine;

namespace _Scripts.Slot_Logic
{
    public class Slot : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform weaponPosition;
        public SlotState SlotState { get; private set; }
        private Weapon _weapon;
        #endregion
    
        #region Monobehaviour Callbacks
        private void Start()
        {

        }
        #endregion
    

        public void Init(int index)
        {
        
        }

        public void CreateWeapon(Weapon weapon)
        {
            _weapon = weapon;
            Instantiate(weapon, weaponPosition.position, weaponPosition.rotation, weaponPosition);
            // ToDo
        }
    }
}
