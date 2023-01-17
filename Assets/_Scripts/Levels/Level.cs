using System.Collections.Generic;
using _Scripts.Units;
using UnityEngine;

namespace _Scripts.Levels
{
    [CreateAssetMenu(fileName ="Level", menuName = "Data/Level")]
    public class Level : ScriptableObject
    {
        #region Variables
        [SerializeField] private LevelLocation levelLocation;
        [SerializeField] private Vector2 timeBetweenZombie;
        [SerializeField] private List<Zombie> zombies;

        public LevelLocation LevelLocation => levelLocation;
        public Vector2 TimeBetweenZombie => timeBetweenZombie;
        public Queue<Zombie> Zombies => new(zombies);
        #endregion
    }
}