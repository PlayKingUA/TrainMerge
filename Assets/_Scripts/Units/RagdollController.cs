using UnityEngine;

namespace _Scripts.Units
{
    public class RagdollController : MonoBehaviour
    {
        #region Variables
        private Collider _mainCollider;
        private Collider[] _colliders;
        private Rigidbody[] _rigidbodies;

        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            _mainCollider = GetComponent<Collider>();
            
            _colliders = GetComponentsInChildren<Collider>();
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            EnableRagdoll(false);
        }
        #endregion

        public void EnableRagdoll(bool isEnabled)
        {
            foreach (var collider1 in _colliders)
            {
                collider1.enabled = isEnabled;
            }
            foreach (var rigidbody1 in _rigidbodies)
            {
                rigidbody1.isKinematic = !isEnabled;
            }
            
            _mainCollider.enabled = !isEnabled;
        }
    }
}
