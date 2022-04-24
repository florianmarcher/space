using System;
using UnityEngine;

public class Random : System.Random
{
    public Random(int seed) : base(seed)
    {
    }

    public Random()
    {
    }

    public double NextDouble(double max) => NextDouble() * max;
    public double NextDouble(double min, double max) => NextDouble() * (max - min) + min;

    public float NextFloat(float max) => (float) NextDouble(0, max);
    public float NextFloat() => (float) NextDouble();
    public float NextFloat(float min, float max) => (float) NextDouble(min, max);

    public float NextNormal(float mean, float deviation)
    {
        throw new NotImplementedException();
    }

    #region Vector4

    public Vector4 NextVector4() => NextVector4(Vector4.negativeInfinity, Vector4.positiveInfinity);
    public Vector4 NextVector4(Vector4 max) => NextVector4(Vector4.zero, max);

    public Vector4 NextVector4(Vector4 min, Vector4 max)
    {
        return new Vector4(NextFloat(min.x, max.x), NextFloat(min.y, max.y), NextFloat(min.z, max.z), NextFloat(min.w, max.w));
    }

    public Vector4 NextVector4(float max) => NextVector4(Vector4.zero, Vector4.one * max);
    public Vector4 NextVector4(float min, float max) => NextVector4(Vector4.one * min, Vector4.one * max);

    #endregion
    
    #region Vector3

    public Vector3 NextVector3() => NextVector3(Vector3.negativeInfinity, Vector3.positiveInfinity);
    public Vector3 NextVector3(Vector3 max) => NextVector3(Vector3.zero, max);

    public Vector3 NextVector3(Vector3 min, Vector3 max)
    {
        return new Vector3(NextFloat(min.x, max.x), NextFloat(min.y, max.y), NextFloat(min.z, max.z));
    }

    public Vector3 NextVector3(float max) => NextVector3(Vector3.zero, Vector3.one * max);
    public Vector3 NextVector3(float min, float max) => NextVector3(Vector3.one * min, Vector3.one * max);

    #endregion

    #region Vector2

    public Vector2 NextVector2() => NextVector2(Vector2.negativeInfinity, Vector2.positiveInfinity);
    public Vector2 NextVector2(Vector2 max) => NextVector2(Vector2.zero, max);

    public Vector2 NextVector2(Vector2 min, Vector2 max)
    {
        return new Vector2(NextFloat(min.x, max.x), NextFloat(min.y, max.y));
    }

    public Vector2 NextVector2(float max) => NextVector2(Vector2.zero, Vector2.one * max);
    public Vector2 NextVector2(float min, float max) => NextVector2(Vector2.one * min, Vector2.one * max);

    #endregion
    
    
}
