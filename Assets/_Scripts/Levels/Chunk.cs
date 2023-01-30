using _Scripts.Train;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Levels
{
    public class Chunk : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform bezierPoint2;
        [SerializeField] private Transform finishPosition;

        [HideInInspector] public Chunk nextChunk;

        [SerializeField, ReadOnly] private float length;
        private const int SegmentsCount = 100;

        public float Length => length;
        #endregion

        #region Properties
        public Vector3 GetPoint(float t) => Bezier.GetPoint(startPosition.position, 
            bezierPoint2.position,
            finishPosition.position, t);
        #endregion

        public float GetCurrentProgress(Vector3 fromPoint)
        {
            var progress = 0f;
            var minDistance = 1e9f;

            for (var i = 0; i < SegmentsCount + 1; i++) {
                var t = (float) i / SegmentsCount;
                var point = GetPoint(t);

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
                var point = GetPoint(t);
                length += Vector3.Distance(previousPoint, point);
                previousPoint = point;
            }
        }
        
        private void OnDrawGizmos() {
            const int segmentsCount = 30;

            for (var i = 0; i < segmentsCount + 1; i++) {
                var t = (float) i / segmentsCount;
                var point = GetPoint(t);
                Gizmos.DrawLine(point, point + Vector3.up);
            }
        }
    }
}
