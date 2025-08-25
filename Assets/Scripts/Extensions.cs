using UnityEngine;

namespace BrackeysGameJam
{
    public static class Extensions
    {
        public static Vector2 Rotate(this Vector2 v, float angleDegrees)
        {
            float rad = angleDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            return new Vector2(
                v.x * cos - v.y * sin,
                v.x * sin + v.y * cos
            );
        }
    }
}