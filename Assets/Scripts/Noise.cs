using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Rand
{
	
	public Rand(ulong seed)
	{
		nProcGen = seed;
	}
	private ulong nProcGen;

	public uint Next()
	{
		nProcGen += 0xe120fc15;
		var tmp = nProcGen * 0x4a39b70d;
		var m1 = (tmp >> 32) ^ tmp;
		tmp = m1 * 0x12fad5c9;
		var m2 = (uint) ((tmp >> 32) ^ tmp);
		return m2;
	}

	public double NextDouble()
	{
		var n = Next();
		var ret = n / 1000000000.0f;
		ret %= 1.0f;
		return ret;
	}
}

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
	private Vector2 seed;

	private float[] layer_frequencies;
	private float[] layer_amplitudes;

	private static readonly Vector2[] directions_4 =
		{new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)};

	public Noise(Vector2 seed_, float[] layer_frequencies_, float[] layer_amplitudes_)
	{
		if (layer_amplitudes_.Length != layer_frequencies_.Length)
			throw new ArgumentException("frequencies and amplitudes need to have the same length");
		layer_frequencies = layer_frequencies_;
		layer_amplitudes = layer_amplitudes_;
		seed = seed_;
	}

	public float getNoise2D(Vector2 v)
	{
		return layer_frequencies.Select((t, i) =>
			(Mathf.PerlinNoise(seed.x + v.x * directions_4[i % 4].x * t, seed.y + v.y * directions_4[i % 4].y * t) *
				2 - 1) * layer_amplitudes[i]).Sum();
	}

	public float getNoise2D(Vector2 v, IEnumerable<float> frequencies, float[] amplitudes)
	{
		return frequencies.Select((t, i) =>
			(Mathf.PerlinNoise(seed.x + v.x * directions_4[i % 4].x * t, seed.y + v.y * directions_4[i % 4].y * t) *
				2 - 1) *
			amplitudes[i]).Sum();
	}
	
	public static float PerlinNoise3D(float x, float y, float z)
	{
		float xy = Mathf.PerlinNoise(x, y);
		float xz = Mathf.PerlinNoise(x, z);
		float yz = Mathf.PerlinNoise(y, z);
		float yx = Mathf.PerlinNoise(y, x);
		float zx = Mathf.PerlinNoise(z, x);
		float zy = Mathf.PerlinNoise(z, y);
 
		return (xy + xz + yz + yx + zx + zy) / 6;
	}

}