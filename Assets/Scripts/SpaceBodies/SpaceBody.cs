using System;
using System.Collections;
using UnityEngine;

namespace SpaceBodies
{
    public abstract class SpaceBody : MonoBehaviour
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

            StartCoroutine(Scale(1, 0, size));
        }

        IEnumerator Scale(float duration, float start, float end, Action on_finish = null)
        {
            transform.localScale = Vector3.one * start;
            var factor = start < end ? 1 : -1;

            while (transform.localScale.x < end)
            {
                transform.localScale += size / duration * Time.deltaTime * factor * Vector3.one;
                yield return null;
            }
            
            transform.localScale = Vector3.one * size;
            
            on_finish?.Invoke();
        }

        public void Destroy()
        {
            transform.parent = SpaceGenerator.instance.transform;
            StartCoroutine(Scale(1, size, 0, () => Destroy(gameObject)));
        }

        public virtual Vector3 AddPlanetMovementFactor(Vector3 movement)
        {
            return movement;
        }

        public virtual void OnSpaceshipEnter(){}
        public virtual void OnSpaceshipExit(){}
        public virtual void OnSpaceshipEnterChunk(){}
        public virtual void OnSpaceshipExitChunk(){}
    }
}
