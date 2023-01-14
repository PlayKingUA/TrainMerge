using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Slot_Logic;
using _Scripts.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers
{
    public class SlotManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Weapon firstLevelWeapon;
        [SerializeField] private Slot[] slots;
        [Space]
        [SerializeField] private List<Slot> emptySlots;
        [SerializeField] private List<Slot> busySlots;
        #endregion

        #region Properties
        public bool HasFreePlace() => emptySlots.Count > 0;

        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            Init();
        }
        #endregion

        private void Init()
        {
            emptySlots.Clear();
            busySlots.Clear();

            for (var i = 0; i < slots.Length; i++)
            {
                slots[i].Init(i);

                switch (slots[i].SlotState)
                {
                    case SlotState.Empty:
                        emptySlots.Add(slots[i]);
                        break;
                    case SlotState.Busy:
                        busySlots.Add(slots[i]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void CreateNewWeapon()
        {
            var index = Random.Range(0, emptySlots.Count);
            var targetSlot = emptySlots[index];
            
            targetSlot.CreateWeapon(firstLevelWeapon);
            emptySlots.Remove(targetSlot);
        }
    }
}