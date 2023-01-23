using UnityEngine;
using Zenject;

namespace _Scripts.Levels
{
    public class Chunk : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform nextChunkPoint;
        
        [Inject] private LevelGeneration _levelGeneration;

        private bool _isCreated;
        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (_isCreated || !other.TryGetComponent(out Train.Train train)) return;
            
            _levelGeneration.CreateChunk(nextChunkPoint);
            _isCreated = true;
        }
    }
}
