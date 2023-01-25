using System;
using System.Collections.Generic;
using _Scripts.Units;
using UnityEngine;

namespace _Scripts.UI.Displays
{
    public class ZombieTable : MonoBehaviour
    {
        #region Variables
        [SerializeField] private ZombieDisplay usualZombieCount;
        [SerializeField] private ZombieDisplay fastZombieCount;
        [SerializeField] private ZombieDisplay bigZombieCount;
        #endregion

        public void UpdatePanel(IEnumerable<Zombie> zombies)
        {
            var usualZombie = 0;
            var fastZombie = 0;
            var bigZombie = 0;
            
            foreach (var zombie in zombies)
            {
                switch (zombie.ZombieType)
                {
                    case ZombieType.Usual:
                        usualZombie++;
                        break;
                    case ZombieType.Fast:
                        fastZombie++;
                        break;
                    case ZombieType.Big:
                        bigZombie++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            usualZombieCount.UpdateCount(usualZombie);
            fastZombieCount.UpdateCount(fastZombie);
            bigZombieCount.UpdateCount(bigZombie);
        }
    }
}