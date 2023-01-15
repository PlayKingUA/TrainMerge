using _Scripts.Slot_Logic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        #region Variables
        [ShowInInspector, ReadOnly]
        private int _level;

        public int Level => _level;
        #endregion

        public void SetLevel(int level)
        {
            _level = level;
        }

        public void ReturnToPreviousPos(Slot previousSlot)
        {
            previousSlot.Refresh(this, previousSlot);
        }
    }
}