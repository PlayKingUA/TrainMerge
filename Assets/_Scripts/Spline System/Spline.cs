using System;
using System.Collections.Generic;
using _Scripts.Levels;
using _Scripts.Train;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Spline_System
{
    public class Spline : MonoBehaviour
    {
        #region Variables
        [ShowInInspector, ReadOnly]
        private List<Anchor> _anchorList = new();
        private float _splineLength;
        private const float StepSize = .05f;

        public event Action OnLengthChanged;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _anchorList.Add(new Anchor());
        }

        #endregion

        public Vector3 GetPositionAt(float distance)
        {
            var t = distance / _splineLength;
            var tFull = t * (_anchorList.Count - 1);
            var anchorIndex = Mathf.FloorToInt(tFull);
            var tAnchor = tFull - anchorIndex;

            var anchorA = _anchorList[anchorIndex + 0];
            var anchorB = _anchorList[anchorIndex + 1];
            
            return Bezier.GetPoint(anchorA.position, anchorA.handlePosition, anchorB.position, tAnchor);
        }

        public float GetCurrentProgress(Vector3 fromPoint) {
            var progress = 0f;
            var minDistance = 1e9f;

            for (float passedDistance = 0; passedDistance < _splineLength; passedDistance += StepSize)
            {
                var point = GetPositionAt(passedDistance);
                var distance = Vector3.Distance(fromPoint, point);
                
                if (distance > minDistance) continue;
                progress = passedDistance;
                minDistance = distance;
            }

            return progress;
        }

        private float GetSplineLength() {
            var splineLength = 0f;
            var lastPosition = _anchorList[0].position;

            for (var passedDistance = StepSize; passedDistance < _splineLength; passedDistance += StepSize) {
                splineLength += Vector3.Distance(lastPosition, GetPositionAt(passedDistance));
                lastPosition = GetPositionAt(passedDistance);
            }
            splineLength += Vector3.Distance(lastPosition, _anchorList[^1].position);

            return splineLength;
        }
        
        #region Edit Spline
        /*
        public void AddAnchor(Chunk chunk)
        {
            _anchorList.Remove(_anchorList[^1]);
            
            
            _anchorList.Add(new Anchor {
                position = chunk.StartPosition,
                handlePosition = chunk.BezierPoint2
            });
            _anchorList.Add(new Anchor {
                position = chunk.FinishPosition,
                handlePosition = chunk.FinishPosition
            });
            
            _splineLength = GetSplineLength();
        }
        */

        public void RemoveFirstChunkAnchors()
        {
            _anchorList.Remove(_anchorList[0]);
            _splineLength = GetSplineLength();
            OnLengthChanged?.Invoke();
        }
        #endregion

        private void OnDrawGizmos()
        {
            if (_anchorList.Count == 0)
                return;
            
            for (float passedDistance = 0; passedDistance < _splineLength; passedDistance += StepSize) {
                var pos = GetPositionAt(passedDistance);
                Gizmos.DrawLine(pos, pos + Vector3.up);
            }
        }
    }
    
    [Serializable]
    public class Anchor {
        public Vector3 position;
        public Vector3 handlePosition;
    }
}