using System;
using System.Collections;
using System.Collections.Generic;
using SpaceBodies;
using UnityEngine;

namespace Misc
{
    public static class PersistantData
    {
        public static class SpaceTransform
        {
            public static Vector3 position;
            public static Quaternion rotation;
        }
    }


    public static class Global
    {
        //a few helpful defines and functions
        public static Vector2Int error_2 = new(int.MinValue, int.MinValue);
        public static Vector3Int error_3 = new(int.MinValue, int.MinValue, int.MinValue);
        public static readonly Vector2Int[] directions_4 = {Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down};
        
        public static readonly Vector3Int[] directions_6 = {Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down, Vector3Int.forward, Vector3Int.back};

        public static readonly Vector2Int[] directions_8 =
        {
            Vector2Int.left, Vector2Int.left + Vector2Int.up,
            Vector2Int.up, Vector2Int.up + Vector2Int.right,
            Vector2Int.right, Vector2Int.right + Vector2Int.down,
            Vector2Int.down, Vector2Int.down + Vector2Int.left
        };

        public const float tolerance = 0.001f;

        public static readonly int emission = Shader.PropertyToID("_EmissionColor");


        public static void SkipFrame(this MonoBehaviour mb, Action action)
        {
            mb.StartCoroutine(_SkipFrame(action));
        }

        private static IEnumerator _SkipFrame(Action action)
        {
            yield return 0;
            action();
        }

        public static void ExecuteAfterSeconds(this MonoBehaviour mb, Action action, float seconds)
        {
            mb.StartCoroutine(_ExecuteAfterSeconds(action, seconds));
        }

        private static IEnumerator _ExecuteAfterSeconds(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        public static float Angle(this Vector3 v) => Mathf.Atan2(v.x, v.z);

        public static Vector3 Multiply(this Vector3 v1, Vector3 v2) =>
            new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);

        public static Vector3 RotateDegrees(this Vector3 v, float? degrees) => v.RotateRadians(degrees * Mathf.Deg2Rad);

        public static Vector3 RotateRadians(this Vector3 v, float? radians)
        {
            if (!radians.HasValue)
                return v;
            var ca = Mathf.Cos(-radians.Value);
            var sa = Mathf.Sin(-radians.Value);
            return new Vector3(ca * v.x - sa * v.z, 0, sa * v.x + ca * v.z);
        }
        
        public static Vector3 Rotate(this Vector3 v, Vector3 current, Vector3 target) => v.Rotate(target.normalized - current.normalized);
        public static Vector3 Rotate(this Vector3 v, Vector3 target) => Quaternion.Euler(target * 90) * v;


        public static Vector2 Map(this Vector2 v, Vector2 from_source, Vector2 to_source, Vector2 from_target,
            Vector2 to_target)
        {
            return new Vector2(v.x.Map(from_source.x, to_source.x, from_target.x, to_target.x),
                v.y.Map(from_source.y, to_source.y, from_target.y, to_target.y));
        }

        public static Vector2 Map01(this Vector2 v, Vector2 from_source, Vector2 to_source)
        {
            return new Vector2(v.x.Map(from_source.x, to_source.x), v.y.Map(from_source.y, to_source.y));
        }

        public static T GetRandom<T>(this List<T> list, float[] weights = null)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T GetRandom<T>(this T[] list, float[] weights = null)
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }

        public static float Map(this float value, float from_source, float to_source, float from_target = 0,
            float to_target = 1)
        {
            return (value - from_source) / (to_source - from_source) * (to_target - from_target) + from_target;
        }


        public static Color ToColor(this Vector3 v) => new(v.x, v.y, v.z);

        public static float[] ToFloatArray(this Vector2 v) => new[] {v.x, v.y};
        public static float[] ToFloatArray(this Vector3 v) => new[] {v.x, v.y, v.z};
        public static float[] ToFloatArray(this Vector4 v) => new[] {v.x, v.y, v.z, v.w};
    }

    public class Constants
    {
        // Gravitational Constant. Units dyne-cm^2/g^2
        public const double gravitational_constant = 6.6720e-08;
    }


    public class SpaceBodyIterator : IEnumerable<SpaceBody>, IEnumerator<SpaceBody>
    {
        private readonly Stack<IEnumerator<ISpaceBodyParent>> space_body_enumerator = new();

        public SpaceBodyIterator(List<SpaceChunk> space_bodies)
        {
            space_body_enumerator.Push(space_bodies.GetEnumerator());
        }

        public IEnumerator<SpaceBody> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            while (true)
            {
                if (space_body_enumerator.Count == 0)
                    return false;
                while (space_body_enumerator.Peek().MoveNext())
                {
                    var space_body = space_body_enumerator.Peek().Current;
                    if (space_body is null || space_body.Children().Count == 0)
                        return true;
                    space_body_enumerator.Push(space_body.Children().GetEnumerator());
                }

                if (space_body_enumerator.Count == 1)
                    return false;

                space_body_enumerator.Pop();
            }
        }

        public void Reset()
        {
            while (space_body_enumerator.Count > 1)
                space_body_enumerator.Pop();
        }

        public SpaceBody Current => space_body_enumerator.Peek().Current as SpaceBody;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}
