using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LehmerRng
{
	private const int a = 16807;
	private const int m = 2147483647;
	private const int q = 127773;
	private const int r = 2836;
	private int seed;
	public LehmerRng(int seed)
	{
		if (seed <= 0 || seed == int.MaxValue)
			throw new Exception("Bad seed");
		this.seed = seed;
	}
	public double NextDouble()
	{
		int hi = seed / q;
		int lo = seed % q;
		seed = (a * lo) - (r * hi);
		if (seed <= 0)
			seed = seed + m;
		return (seed * 1.0) / m;
	}
}

public class LinearConRng
{
	private const long a = 25214903917;
	private const long c = 11;
	private long seed;
	public LinearConRng(long seed)
	{
		if (seed < 0)
			throw new Exception("Bad seed");
		this.seed = seed;
	}
	private int next(int bits) // helper
	{
		seed = (seed * a + c) & ((1L << 48) - 1);
		return (int)(seed >> (48 - bits));
	}
	public double NextDouble()
	{
		return (((long)next(26) << 27) + next(27)) / (double)(1L << 53);
	}
}


public class Noise
{
	[Serializable]
	public struct Parameter
	{
		public float frequency;
		public float amplitude;
	}

	private Vector2 seed;

	private Parameter[] noise_parameters;

	private static readonly Vector2[] directions_4 =
		{new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)};

	public Noise(Vector2 seed_, Parameter[] noise_parameters_)
	{
		noise_parameters = noise_parameters_;
		seed = seed_;
	}

	public float GetNoise2D(Vector2 v)
	{
		return noise_parameters.Select((parameters, i) =>
			(Mathf.PerlinNoise(seed.x + v.x * directions_4[i % 4].x * parameters.frequency, seed.y + v.y * directions_4[i % 4].y * parameters.frequency)) * parameters.amplitude).Sum();
	}

	public float GetNoise2D(Vector2 v, IEnumerable<Parameter> parameters)
	{
		return parameters.Select((p, i) =>
			(Mathf.PerlinNoise(seed.x + v.x * directions_4[i % 4].x * p.frequency, seed.y + v.y * directions_4[i % 4].y * p.frequency) *
				2 - 1) *
			p.amplitude).Sum();
	}
	
	public static float PerlinNoise3D(float x, float y, float z)
	{
		var xy = Mathf.PerlinNoise(x, y);
		var xz = Mathf.PerlinNoise(x, z);
		var yz = Mathf.PerlinNoise(y, z);
		var yx = Mathf.PerlinNoise(y, x);
		var zx = Mathf.PerlinNoise(z, x);
		var zy = Mathf.PerlinNoise(z, y);
 
		return (xy + xz + yz + yx + zx + zy) / 6;
	}

}