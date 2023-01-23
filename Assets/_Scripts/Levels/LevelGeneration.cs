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

        private readonly List<GameObject> _createdChunks = new();

        private const int MaxChunksCount = 3;
        #endregion
        
        public void SetLocation(LevelLocation location)
        {
            _location = location;
            
            CreateChunk(positionOne, true);
            CreateChunk(positionTwo, true);
        }

        public GameObject CreateChunk(Transform targetPosition, bool isStraight = false)
        {
            var chunk = (isStraight) 
                ? chunks[(int) _location].StraightChunk
                : chunks[(int) _location].RandomChunk();

            _createdChunks.Add(_diContainer.InstantiatePrefab(chunk, 
                targetPosition.position, targetPosition.rotation, transform));

            if (_createdChunks.Count > MaxChunksCount)
            {
                DestroyFirstChunk();
            }

            return chunk;
        }

        private void DestroyFirstChunk()
        {
            var removedObject = _createdChunks[0];
            _createdChunks.Remove(removedObject);
            Destroy(removedObject);
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
            var targetChunk = Random.Range(0, 2) switch
            {
                0 => turnChunk,
                1 => turnTwoChunk,
                _ => straightChunk
            };

            return targetChunk;
        }
    }
}
