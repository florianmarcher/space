using System;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

[Serializable]
public class Noise
{

    private readonly OpenSimplexNoise simplex_noise;

    [SerializeField]  private NoiseParameter[] noise_parameters;

    public Noise(int seed, NoiseParameter[] noise_parameters_)
    {
        noise_parameters = noise_parameters_;
        simplex_noise = new OpenSimplexNoise(seed);
    }

    public Noise()
    {
        simplex_noise = new OpenSimplexNoise(new Random().Next());
        noise_parameters = new NoiseParameter[] {new() {enabled = true, frequency = 1, amplitude = 1}};
    }
    public Noise(int seed)
    {
        simplex_noise = new OpenSimplexNoise(seed);
        noise_parameters = new NoiseParameter[] {new() {enabled = true, frequency = 1, amplitude = 1}};
    }

    public double GetNoise(params float[] values) => GetNoise(noise_parameters, values);

    public double GetNoise(IEnumerable<NoiseParameter> parameters, params float[] values) =>
        GetNoise(simplex_noise, parameters, values);


    public static double GetNoise(OpenSimplexNoise noise, IEnumerable<NoiseParameter> parameters, params float[] values)
    {
        Func<float[], double, double> evaluate_noise = values.Length switch
        {
            0 => (_, _) => 0.0,
            1 => (v, f) => noise.Evaluate(v[0] * f, 0),
            2 => (v, f) => noise.Evaluate(v[0] * f, v[1] * f),
            3 => (v, f) => noise.Evaluate(v[0] * f, v[1] * f, v[2] * f),
            _ => (v, f) => noise.Evaluate(v[0] * f, v[1] * f, v[2] * f, v[3] * f),
        };

        return parameters.Aggregate(0.0, (n, p) => p.Evaluate(evaluate_noise, values, n));

        return parameters.Select(p => p.Evaluate(evaluate_noise, values) * 0.01f).Sum();
     //   return parameters.Select(p => evaluate_noise(values, p.frequency) * p.amplitude * 0.01f).Sum();
    }
}
