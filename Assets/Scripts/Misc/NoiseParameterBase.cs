using System;

namespace Misc
{
    [Serializable]
    public enum NoiseType{Normal, Abs}
    
    [Serializable]
    public struct NoiseParameter
    {
        public NoiseType noise_type;
        public bool enabled;
        public float frequency;
        public float amplitude;
        public  double Evaluate(Func<float[], double, double> noise, float[] values, double n = 0)
        {
            if (!enabled) return n;

            return n + noise_type switch
            {
                NoiseType.Normal => noise(values, frequency) * amplitude,
                NoiseType.Abs => Math.Abs(noise(values, frequency)) * amplitude,
                _ => noise(values, frequency) * amplitude
            };

        }
    }
}
