using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int maxWeaponLevel;
        [SerializeField] private List<Weapon> weapons;

        public int MaxWeaponLevel => maxWeaponLevel - 1;
        #endregion
    
        #region Monobehaviour Callbacks
        private void OnValidate()
        {
            maxWeaponLevel = Mathf.Min(maxWeaponLevel, weapons.Count);
        }
        #endregion

        public Weapon CreateWeapon(int level, Transform parent)
        {
            var weapon = Instantiate(weapons[level], parent);
            weapon.SetLevel(level);
            return weapon;
        }
    }
}
