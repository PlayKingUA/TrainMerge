using System;
using _Scripts.Slot_Logic;
using _Scripts.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Input_Logic
{
    public class DragManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float yWeaponPosition;

        [Space, SerializeField, ReadOnly] private Weapon selectedWeapon;
        [SerializeField, ReadOnly] private DragState currentDragState;
        [SerializeField, ReadOnly] private Slot previousSlot;

        [Inject] private InputHandler _inputHandler;
        #endregion
        
        #region Monobehaviour Callbacks
        private void Update()
        {
            switch (currentDragState)
            {
                case DragState.Empty:
                    StartDragging();
                    break;
                case DragState.Busy:
                    Drag();
                    break;
                case DragState.FinishDragging:
                    EndDragging();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion


        #region Drag Logic
        private void StartDragging()
        {
            if (!_inputHandler.IsTouchStarted) return;
            //ToDo check game state
            
            var ray = CastRay();
            if (ray.collider == null) return;
               
            if (!ray.collider.TryGetComponent(out Slot weaponSlot)
                || weaponSlot.SlotState == SlotState.Empty)
            {
                return;
            }

            selectedWeapon = weaponSlot.GetWeaponFromSlot();
            previousSlot = weaponSlot;
            weaponSlot.ClearSlot();

            currentDragState = DragState.Busy;
        }

        private void Drag()
        {
            var currentPosition = new Vector3(_inputHandler.TouchPosition.x,
                _inputHandler.TouchPosition.y, 
                Camera.main.WorldToScreenPoint(selectedWeapon.transform.position).z);

            var worldPosition = Camera.main.ScreenToWorldPoint(currentPosition);
            selectedWeapon.transform.position = new Vector3(worldPosition.x, yWeaponPosition, worldPosition.z);

            if (!_inputHandler.IsTouchReleased) return;
            var ray = CastRay();

            if (ray.collider != null)
            {
                if (!ray.collider.TryGetComponent(out Slot unitSlot))
                {
                    //previousSlot.Return(selectedGroup);
                    currentDragState = DragState.FinishDragging;
                    return;
                }

                //unitSlot.Refresh(selectedGroup, previousSlot);
                currentDragState = DragState.FinishDragging;
            }
            else
            {
                //previousSlot.Return(selectedGroup);
                currentDragState = DragState.FinishDragging;
            }
        }

        private void EndDragging()
        {
            selectedWeapon = null;
            currentDragState = DragState.Empty;
        }
        #endregion
        
        private RaycastHit CastRay()
        {
            var screenMousePosFar = new Vector3(
                _inputHandler.TouchPosition.x,
                _inputHandler.TouchPosition.y,
                Camera.main.farClipPlane);
            var screenMousePosNear = new Vector3(
                _inputHandler.TouchPosition.x,
                _inputHandler.TouchPosition.y,
                Camera.main.nearClipPlane);

            var worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
            var worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out var hit,
                Mathf.Infinity, layerMask);

            return hit;
        }
    }
}
