using System.Collections.Generic;
using _Scripts.Weapons;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<Weapon> weapons;
    #endregion
    
    #region Monobehaviour Callbacks
    private void Start()
    {

    }
    #endregion

    public Weapon CreateWeapon(int level, Transform parent)
    {
        return Instantiate(weapons[level], parent);
    }
}
