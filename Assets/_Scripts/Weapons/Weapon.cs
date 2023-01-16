using _Scripts.Projectiles;
using _Scripts.Slot_Logic;
using _Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class Weapon : AttackingObject
    {
        #region Variables
        [Space]
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private float damageArea;
        [Space] 
        [SerializeField] private GameObject appearFx;
        [Space] 
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Projectile projectile;
        [Space(10)]
        [ShowInInspector, ReadOnly]
        private int _level;

        public int Level => _level;
        public float Health => health;
        public GameObject AppearFx => appearFx;
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