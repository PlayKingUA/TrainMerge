using System;
using UnityEngine;

namespace _Scripts.Units
{
    public sealed class ZombieAnimationManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Animator animator;

        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _victoryHash = Animator.StringToHash("Victory");

        private UnitState _previousAnimState;
        #endregion
        
        #region Monobehaviour Callbacks


        #endregion
        
        public void SetAnimation(UnitState state)
        {
            _previousAnimState = state;
            animator.CrossFade(GetHash(state), 0.3f, 0);
        }

        private int GetHash(UnitState state)
        {
            var hash = state switch
            {
                UnitState.Idle => _idleHash,
                UnitState.Run => _runHash,
                UnitState.Attack => _attackHash,
                UnitState.Victory => _victoryHash,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
            return hash;
        }
        
        public void DisableAnimator()
        {
            animator.enabled = false;
        }
    }
}