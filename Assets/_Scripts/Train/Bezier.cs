using UnityEngine;

namespace _Scripts.Train
{
    public static class Bezier
    {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var p01 = Vector3.Lerp(p0, p1, t);
            var p12 = Vector3.Lerp(p1, p2, t);
            
            return Vector3.Lerp(p01, p12, t);
        }
    }

}