using _Scripts.Units;
using UnityEngine;

namespace _Scripts.Projectiles
{
    public class Laser : ProjectileBase
    {
        #region Variables
        [SerializeField] private Hovl_Laser laserScript;
        #endregion

        public override void UpdateTargetZombie(Zombie targetZombie)
        {
            laserScript.UpdateLaserTargetPosition(targetZombie.ShootPoint.position);
        }
    }
}