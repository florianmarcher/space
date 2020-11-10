using System;
using UnityEngine;

public class Random : System.Random
{
	public Random(int seed) : base(seed){}

	public double NextDouble(double max) => NextDouble() * max;
	public double NextDouble(double min, double max) => NextDouble() * (max - min) + min;

	public float NextFloat(float max) => (float) NextDouble(0, max);
	public float NextFloat() => (float) NextDouble();
	public float NextFloat(float min, float max) => (float) NextDouble(min, max);

	public float NextNormal(float mean, float deviation)
	{
		throw new NotImplementedException();
	}

	public Vector3 NextVector3() => NextVector3(Vector3.negativeInfinity, Vector3.positiveInfinity);
	public Vector3 NextVector3(Vector3 max) => NextVector3(Vector3.zero, max);
	public Vector3 NextVector3(Vector3 min, Vector3 max)
	{
		return new Vector3(NextFloat(min.x, max.x), NextFloat(min.y, max.y), NextFloat(min.z, max.z));
	}
	public Vector3 NextVector3(float max) => NextVector3(Vector3.zero, Vector3.one * max);
	public Vector3 NextVector3(float min, float max) => NextVector3(Vector3.one * min, Vector3.one * max);
}