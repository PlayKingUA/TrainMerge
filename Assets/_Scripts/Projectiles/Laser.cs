using UnityEngine;

namespace _Scripts.Projectiles
{
    public class Laser : ProjectileBase
    {
        #region Variables
        [SerializeField] private Hovl_Laser laserScript;
        #endregion

        public override void UpdateTargetPosition(Vector3 targetPosition)
        {
            base.UpdateTargetPosition(targetPosition);
            laserScript.UpdateLaserTargetPosition(TargetPosition);
        }
    }
}