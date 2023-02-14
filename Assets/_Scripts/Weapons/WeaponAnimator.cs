using System;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class WeaponAnimator : MonoBehaviour
    {
        private Animator _animator;

        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _deathHash = Animator.StringToHash("Death");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAnimation(WeaponState state)
        {
            _animator.CrossFade(GetHash(state), 0.3f);
        }

        private int GetHash(WeaponState state)
        {
            var hash = state switch
            {
                WeaponState.Idle => _idleHash,
                WeaponState.Attack => _attackHash,
                WeaponState.Death => _deathHash,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
            return hash;
        }
    }
}