using UnityEngine;

namespace SpaceBodies
{
    public class SpaceBody : MonoBehaviour
    {
        [SerializeField]public int seed;
        [SerializeField]protected float size;
        [SerializeField]protected float rotation_speed;
        [SerializeField]protected float time_offset;
        protected Random random;


        protected void Init(int seed_)
        {
            seed = seed_;
            random = new Random(seed);
            time_offset = (float) (random.NextDouble() * 1000);
        }
        
    }
}
