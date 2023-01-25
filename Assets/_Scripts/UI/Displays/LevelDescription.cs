using System;
using _Scripts.Levels;
using _Scripts.Units;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI.Displays
{
    public class LevelDescription : MonoBehaviour
    {
        #region text
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private ZombieDisplay usualZombieCount;
        [SerializeField] private ZombieDisplay fastZombieCount;
        [SerializeField] private ZombieDisplay bigZombieCount;

        [Inject] private LevelManager _levelManager;
        #endregion

        private void Start()
        {
            _levelManager.OnLevelLoaded += UpdateText;
        }

        private void UpdateText(Level currentLevel)
        {
            levelText.text = "Level " + currentLevel.index;

            var usualZombie = 0;
            var fastZombie = 0;
            var bigZombie = 0;
            
            foreach (var zombie in currentLevel.Zombies)
            {
                if (zombie.ZombieType == ZombieType.Usual)
                {
                    usualZombie++;
                }
                if (zombie.ZombieType == ZombieType.Fast)
                {
                    fastZombie++;
                }
                if (zombie.ZombieType == ZombieType.Big)
                {
                    bigZombie++;
                }
            }
            usualZombieCount.UpdateCount(usualZombie);
            fastZombieCount.UpdateCount(fastZombie);
            bigZombieCount.UpdateCount(bigZombie);
        }
    }
}