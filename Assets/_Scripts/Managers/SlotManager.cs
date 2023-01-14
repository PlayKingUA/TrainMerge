using System;
using System.Collections.Generic;
using _Scripts.Slot_Logic;
using _Scripts.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers
{
    public class SlotManager : MonoBehaviour
    {
        #region Variables
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

            foreach (var slot in slots)
            {
                slot.Init();

                switch (slot.SlotState)
                {
                    case SlotState.Empty:
                        emptySlots.Add(slot);
                        break;
                    case SlotState.Busy:
                        busySlots.Add(slot);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void CreateNewWeapon(int targetLevel = 0)
        {
            var index = Random.Range(0, emptySlots.Count);
            var targetSlot = emptySlots[index];
            
            targetSlot.Spawn(targetLevel);
            emptySlots.Remove(targetSlot);
        }
    }
}