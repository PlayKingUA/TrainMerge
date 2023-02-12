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

        private ZombieCount _zombieCount;
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
            _zombieCount = zombieCount;
            
            usualZombieCount.UpdateCount(_zombieCount.UsualZombieCount);
            fastZombieCount.UpdateCount(_zombieCount.FastZombieCount);
            bigZombieCount.UpdateCount(_zombieCount.BigZombieCount);
        }

        public void RemoveZombie(ZombieType zombieType)
        {
            var usualZombie = _zombieCount.UsualZombieCount;
            var fastZombie = _zombieCount.FastZombieCount;
            var bigZombie = _zombieCount.BigZombieCount;
            
            switch (zombieType)
            {
                case ZombieType.Usual:
                    usualZombie--;
                    break;
                case ZombieType.Fast:
                    fastZombie--;
                    break;
                case ZombieType.Big:
                    bigZombie--;
                    break;
                case ZombieType.CountTypes:
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            UpdatePanel(new ZombieCount(usualZombie, fastZombie, bigZombie));
        }
    }
}