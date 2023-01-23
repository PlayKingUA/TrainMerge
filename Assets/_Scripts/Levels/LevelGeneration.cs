using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Levels
{
    public class LevelGeneration : MonoBehaviour
    {
        #region Variables
        [ShowInInspector] private LevelLocation _location;
        [SerializeField] private LevelChunks[] chunks;
        [SerializeField] private Transform positionOne;
        [SerializeField] private Transform positionTwo;
        
        [Inject] private DiContainer _diContainer;
        [Inject] private Train.Train _train;

        private readonly List<Chunk> _createdChunks = new();

        private const int MaxChunksCount = 3;
        #endregion
        
        public void SetLocation(LevelLocation location)
        {
            _location = location;
            
            CreateChunk(positionOne, true);
            CreateChunk(positionTwo, true);
            
            _train.InitMotion(_createdChunks[1]);
        }

        public GameObject CreateChunk(Transform targetPosition, bool isStraight = false)
        {
            var chunk = (isStraight) 
                ? chunks[(int) _location].StraightChunk
                : chunks[(int) _location].RandomChunk();

            var createdChunk = _diContainer.InstantiatePrefabForComponent<Chunk>(chunk, 
                targetPosition.position, targetPosition.rotation, transform);
            if (_createdChunks.Count > 0)
                _createdChunks[^1].nextChunk = createdChunk;
            
            _createdChunks.Add(createdChunk);

            if (_createdChunks.Count > MaxChunksCount)
                DestroyFirstChunk();

            return chunk;
        }

        private void DestroyFirstChunk()
        {
            var removedObject = _createdChunks[0];
            _createdChunks.Remove(removedObject);
            Destroy(removedObject.gameObject);
        }
    }


    [System.Serializable]
    public class LevelChunks
    {
        [SerializeField] private GameObject straightChunk;
        [SerializeField] private GameObject turnChunk;
        [SerializeField] private GameObject turnTwoChunk;

        public GameObject StraightChunk => straightChunk;

        public GameObject RandomChunk()
        {
            var targetChunk = Random.Range(0, 4) switch
            {
                0 => turnChunk,
                1 => turnTwoChunk,
                _ => straightChunk
            };

            return targetChunk;
        }
    }
}
