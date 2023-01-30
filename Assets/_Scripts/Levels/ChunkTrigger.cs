using UnityEngine;
using Zenject;

namespace _Scripts.Levels
{
    public class ChunkTrigger : MonoBehaviour
    {
        [SerializeField] private Transform finishPosition;
        
        [Inject] private LevelGeneration _levelGeneration;
        
        private bool _isCreated;

        private void OnTriggerEnter(Collider other)
        {
            if (_isCreated || !other.TryGetComponent(out Train.Train train)) return;
            
            _levelGeneration.CreateChunk(finishPosition);
            _isCreated = true;
        }
    }
}