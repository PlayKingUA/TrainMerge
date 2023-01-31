using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Levels
{
    [CreateAssetMenu(fileName ="Level", menuName = "Data/Level")]
    public class Level : ScriptableObject
    {
        #region Variables
        [SerializeField] private LevelLocation levelLocation;
        [SerializeField] private List<Wave> zombiesWaves;
        [HideInInspector] public int index;

        public LevelLocation LevelLocation => levelLocation;
        public List<Wave> ZombiesWaves => new List<Wave>(zombiesWaves);
        #endregion

        [ShowInInspector, ReadOnly]
        public ZombieCount ZombieCount => new (
            zombiesWaves.SelectMany(wave => wave.subWaves).Sum(subWave => subWave.ZombieCount.UsualZombieCount),
            zombiesWaves.SelectMany(wave => wave.subWaves).Sum(subWave => subWave.ZombieCount.FastZombieCount),
            zombiesWaves.SelectMany(wave => wave.subWaves).Sum(subWave => subWave.ZombieCount.BigZombieCount));
    }

    [Serializable]
    public class Wave
    {
        public List<SubWave> subWaves;
        [SerializeField] private float timeBetweenWaves = 4f;

        public float TimeBetweenWaves => timeBetweenWaves;
        
        [Serializable]
        public class SubWave
        {
            [SerializeField] private float timeBetweenZombie = 0.5f;
            [SerializeField] private float timeBetweenWaves = 2f;
            [SerializeField] private ZombieCount zombieCount;

            public float TimeBetweenWaves => timeBetweenWaves;
            public float TimeBetweenZombie => timeBetweenZombie;
            public ZombieCount ZombieCount => zombieCount;
        }
    }
    
    [Serializable]
    public class ZombieCount
    {
        [SerializeField] private int usualZombieCount;
        [SerializeField] private int fastZombieCount;
        [SerializeField] private int bigZombieCount;
        public int UsualZombieCount => usualZombieCount;
        public int FastZombieCount => fastZombieCount;
        public int BigZombieCount => bigZombieCount;

        public ZombieCount(int usualZombieCount, int fastZombieCount, int bigZombieCount)
        {
            this.usualZombieCount = usualZombieCount;
            this.fastZombieCount = fastZombieCount;
            this.bigZombieCount = bigZombieCount;
        }
    }
}