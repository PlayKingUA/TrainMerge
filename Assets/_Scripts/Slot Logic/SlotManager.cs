using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Slot_Logic
{
    public class SlotManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Slot[] slots;
        [Space]
        [SerializeField, ReadOnly] private List<Slot> emptySlots;
        [SerializeField, ReadOnly] private List<Slot> busySlots;

        private bool _isTutorialArrows;
        #endregion

        #region Properties
        public bool HasFreePlace() => emptySlots.Count > 0;
        public int WeaponsHealthSum => slots.Sum(slot => slot.WeaponHealth);
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
            
            if (_isTutorialArrows)
            {
                ShowTutorialArrows();
            }
        }

        public void CreateNewWeapon(int targetLevel = 0)
        {
            if (!HasFreePlace())
            {
                return;
            }
            
            foreach (var slot in slots)
            {
                if (slot.SlotState != SlotState.Empty) continue;
                slot.SpawnWeapon(targetLevel, true);
                emptySlots.Remove(slot);
                return;
            }
            
            if (_isTutorialArrows)
            {
                ShowTutorialArrows();
            }
        }
        
        public void RefreshSlots(Slot weaponSLot)
        {
            switch (weaponSLot.SlotState)
            {
                case SlotState.Empty:
                    emptySlots.Add(weaponSLot);
                    busySlots.Remove(weaponSLot);
                    break;
                case SlotState.Busy:
                    emptySlots.Remove(weaponSLot);
                    busySlots.Remove(weaponSLot);
                    busySlots.Add(weaponSLot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_isTutorialArrows)
            {
                ShowTutorialArrows();
            }
        }
        
        public void ShowTutorialArrows(bool isShown = true)
        {
            _isTutorialArrows = isShown;

            foreach (var slot in slots)
            {
                slot.EnablePointer(_isTutorialArrows);
            }
        }
    }
}