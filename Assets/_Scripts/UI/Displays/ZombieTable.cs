using System;
using System.Collections.Generic;
using _Scripts.Levels;
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

            UpdatePanel(new ZombieCount(usualZombie, fastZombie, bigZombie));
        }

        public void UpdatePanel(ZombieCount zombieCount)
        {
            usualZombieCount.UpdateCount(zombieCount.UsualZombieCount);
            fastZombieCount.UpdateCount(zombieCount.FastZombieCount);
            bigZombieCount.UpdateCount(zombieCount.BigZombieCount);
        }
    }
}