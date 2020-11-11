using System.Collections;
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


        protected void Init(int seed_, float s)
        {
            seed = seed_;
            size = s;
            if(random == null) random = new Random(seed);
            time_offset = (float) (random.NextDouble() * 1000);

            StartCoroutine(ScaleIn(1));
        }

        IEnumerator ScaleIn(float duration)
        {
            transform.localScale = Vector3.zero;

            while (transform.localScale.x < size)
            {
                transform.localScale += size / duration * Time.deltaTime * Vector3.one;
                yield return null;
            }
            
            transform.localScale = Vector3.one * size;
        }
        
    }
}
