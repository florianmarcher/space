using System;

namespace Misc
{

    public abstract class Range
    {
        protected float a;
        protected float b;

        protected Range(float a_, float b_)
        {
            a = a_;
            b = b_;
        }

        public abstract float GetRandom();
    }

    public class UniformRange : Range
    {
        public UniformRange(float min, float max) : base(min, max) { }

        public override float GetRandom() => UnityEngine.Random.Range(a, b);
    }
    public class NormalRange : Range
    {
        public NormalRange(float mean, float sigma) : base(mean, sigma) { }

        public override float GetRandom()
        {
            var rand = UnityEngine.Random.value;
            if (rand < 0.5f)
                return (float) (0.5f * Math.Pow(2 * rand, b) + a);
            return (float) (1 - Math.Pow(2 - 2 * rand, b) * 0.5f + a);
        }
    }
}