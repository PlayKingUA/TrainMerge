using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Units
{
    public class ZombieHolder : MonoBehaviour
    {
        #region Variables
        [SerializeField] private List<Zombie> _aliveZombies = new List<Zombie>();
        #endregion
        
        #region Monobehaviour Callbacks
        
        #endregion

        public void AddZombies()
        {
            
        }

        public Zombie GetNearestZombie(Transform fromTransform)
        {
            Zombie targetZombie = null;
            var minDistance = 1e9f;
            
            foreach (var zombie in _aliveZombies)
            {
                var currentDistance = Vector3.Distance(fromTransform.position, zombie.transform.position);
                if (!(currentDistance < minDistance)) continue;
                minDistance = currentDistance;
                targetZombie = zombie;
            }

            return targetZombie;
        }
        
    }
}
