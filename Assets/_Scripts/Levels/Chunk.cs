using _Scripts.Train;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Scripts.Levels
{
    public class Chunk : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform bezierPoint2;
        [SerializeField] private Transform bezierPoint3;
        [SerializeField] private Transform finishPosition;

        [HideInInspector] public Chunk nextChunk; 
        
        [Inject] private LevelGeneration _levelGeneration;

        [SerializeField, ReadOnly] private float length;
        private const int SegmentsCount = 100;

        private bool _isCreated;

        public float Length => length;
        #endregion

        #region Properties
        public Vector3 GetPoint(float t) => Bezier.GetPoint(startPosition.position, 
            bezierPoint2.position,
            bezierPoint3.position,
            finishPosition.position, t);

        public Vector3 GetFirstDerivative(float t) => Bezier.GetFirstDerivative(startPosition.position, 
            bezierPoint2.position,
            bezierPoint3.position,
            finishPosition.position, t);
        #endregion
        
        
        public float GetCurrentProgress(Vector3 fromPoint)
        {
            var progress = 0f;
            var minDistance = 1e9f;

            for (var i = 0; i < SegmentsCount + 1; i++) {
                var t = (float) i / SegmentsCount;
                var point = Bezier.GetPoint(startPosition.position, bezierPoint2.position, bezierPoint3.position,
                    finishPosition.position, t);

                var distance = Vector3.Distance(fromPoint, point);
                if (!(distance < minDistance)) continue;
                progress = t;
                minDistance = distance;
            }

            return progress;
        }

        [Button("Calculate Length")]
        private void CalculateLength()
        {
            length = 0;
            
            var previousPoint = startPosition.position;

            for (var i = 1; i < SegmentsCount + 1; i++) {
                var t = (float) i / SegmentsCount;
                var point = Bezier.GetPoint(startPosition.position, bezierPoint2.position, bezierPoint3.position,
                    finishPosition.position, t);
                length += Vector3.Distance(previousPoint, point);
                previousPoint = point;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_isCreated || !other.TryGetComponent(out Train.Train train)) return;
            
            _levelGeneration.CreateChunk(finishPosition);
            _isCreated = true;
        }
        
        private void OnDrawGizmos() {
            const int segmentsCount = 30;
            var previousPoint = startPosition.position;

            for (var i = 0; i < segmentsCount + 1; i++) {
                var t = (float) i / segmentsCount;
                var point = Bezier.GetPoint(startPosition.position, bezierPoint2.position, bezierPoint3.position,
                    finishPosition.position, t);
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}
