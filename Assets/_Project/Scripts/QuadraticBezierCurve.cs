using UnityEngine;

namespace gmtk_gamejam
{
    public class QuadraticBezierCurve
    {
        private Vector2 p0;
        private Vector2 p1;
        private Vector2 p2;

        public Vector3 P0 => p0;
        public Vector3 P1 => p1;
        public Vector3 P2 => p2;

        public QuadraticBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
        }
        public QuadraticBezierCurve(Vector2 start, Vector2 end)
        {
            this.p0 = start;
            this.p2 = end;
            Vector2 midPoint = start + (end - start) / 2f;
            Vector2 normal = Vector3.Cross(Vector3.forward, (end - start).normalized);
            this.p1 = midPoint + normal.normalized * 2f;
        }

        public Vector2 GetPos(float t)
        {
            Vector2 pos = (1f - t) * (1f - t) * p0 + 2 * (1f - t) * t * p1 + t * t * p2;
            return pos;
        }
        public Vector2 GetTangent(float t)
        {
            Vector2 tangent = 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
            return tangent;
        }
    }
}