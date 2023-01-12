using System;
using _Scripts.Weapons;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform weaponPosition;
    private Weapon _weapon;

    public bool IsEmpty => _weapon == null;

    public void CreateWeapon(Weapon weapon)
    {
        _weapon = weapon;
        Instantiate(weapon, weaponPosition.position, weaponPosition.rotation, weaponPosition);
        // ToDo
    }
}
