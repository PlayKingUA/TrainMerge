using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Scripts.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int maxWeaponLevel;
        [SerializeField] private List<Weapon> weapons;

        [Inject] private DiContainer _diContainer;
        public int MaxWeaponLevel => maxWeaponLevel - 1;
        
        public event Action OnNewWeapon;
        #endregion
    
        #region Monobehaviour Callbacks
        private void OnValidate()
        {
            maxWeaponLevel = Mathf.Min(maxWeaponLevel, weapons.Count);
        }
        #endregion

        public Weapon CreateWeapon(int level, Transform parent)
        {
            var weapon = _diContainer.InstantiatePrefabForComponent<Weapon>(weapons[level], parent);
            weapon.SetLevel(level);
            StartCoroutine(UpgradeTrainHealth());
            return weapon;
        }

        private IEnumerator UpgradeTrainHealth()
        {
            yield return new WaitForEndOfFrame();
            OnNewWeapon?.Invoke();
        }
    }
}
