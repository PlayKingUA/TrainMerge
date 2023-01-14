using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Input_Logic
{
    public class InputHandler : MonoBehaviour
    {
        #region Variables
        private Vector3 _lastFrameTouchPosition = Vector3.zero;
        public bool IsTouchStarted { get; private set; }
        public bool IsTouchReleased { get; private set;}
        public Vector3 TouchPosition => IsTouchStarted ? Input.mousePosition : _lastFrameTouchPosition;
        #endregion

        #region Monobehaviour Callbacks
        private void Update()
        {
            if (IsTouchStarted == false)
            {
                return;
            }

            _lastFrameTouchPosition = TouchPosition;
        }
        #endregion
        
        #region Dragging
        public void StartDragging(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            IsTouchStarted = true;
            IsTouchReleased = false;
        }

        public void Dragging(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
        }

        public void EndDragging(InputAction.CallbackContext context)
        {
            if (!context.canceled) return;
            
            IsTouchStarted = false;
            IsTouchReleased = true;
        }
        #endregion
    }
}