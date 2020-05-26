using System;
using UnityEngine;

namespace Extensions {
    public static class Vector2Extension {
        public static Vector2 Rotate(this Vector2 v, float degrees) {
            return Quaternion.Euler(0, 0, degrees) * v;
        }

        public static float Project(this Vector2 v, Vector2 target) {
            //return v.magnitude * Mathf.Cos(Mathf.Deg2Rad * Vector2.Angle (v, target));
            var projection = (v.x * target.x + v.y * target.y) / target.magnitude;
            return projection;
        }

        public static Vector2 Midpoint(Vector2 v1, Vector2 v2) {
            return new Vector2((v1.x + v2.x) / 2f, (v1.y + v2.y) / 2f);
        }

        public static Vector2 Flatten(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 Explode(this Vector2 v) {
            return new Vector3(v.x, v.y, 0);
        }

        public static float QuadrantAngle(this Vector2 v) {
            var angle = Mathf.Atan(Mathf.Abs(v.y / v.x));

            if (v.x >= 0 && v.y >= 0)
                return angle;
            else if (v.x < 0 && v.y >= 0)
                return Mathf.PI - angle;
            else if (v.x <= 0 && v.y < 0)
                return Mathf.PI + angle;
            else
                return -angle;
        }
    }
}