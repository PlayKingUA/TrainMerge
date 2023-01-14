using _Scripts.Slot_Logic;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private int _level;

        public int Level => _level;

        private Vector2 _inputPosition;

        public bool CanUpgrade(Weapon weapon)
        {
            //ToDo
            return false;
        }

        public void ReturnToPreviousPos(Slot previousSlot)
        {
            previousSlot.Refresh(this, previousSlot);
        }
    }
}