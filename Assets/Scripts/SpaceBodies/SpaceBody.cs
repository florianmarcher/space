using System;
using System.Collections;
using UnityEngine;

namespace SpaceBodies
{
    public struct SpaceBodyData
    {
        public Random random;
        public int seed;
        public float size;
        public float rotation_speed;
        public float time_offset;
    }
    
    
    public abstract class SpaceBody : MonoBehaviour
    {
        protected SpaceBodyData data;


        protected void Init(int seed_, float s)
        {
            data.seed = seed_;
            data.size = s;
            if(data.random == null) data.random = new Random(data.seed);
            data.time_offset = (float) (data.random.NextDouble() * 1000);

            StartCoroutine(Scale(1, 0, data.size));
        }

        private IEnumerator Scale(float duration, float start, float end, Action on_finish = null)
        {
            transform.localScale = Vector3.one * start;
            var factor = start < end ? 1 : -1;

            while (transform.localScale.x < end)
            {
                transform.localScale += data.size / duration * Time.deltaTime * factor * Vector3.one;
                yield return null;
            }
            
            transform.localScale = Vector3.one * data.size;
            
            on_finish?.Invoke();
        }

        public void Destroy()
        {
            transform.parent = SpaceGenerator.instance.transform;
            StartCoroutine(Scale(1, data.size, 0, () => Destroy(gameObject)));
        }

        public virtual Vector3 AddPlanetMovementFactor(Vector3 movement)
        {
            return movement;
        }

        private void OnTriggerEnter(Collider other)
        {
            var space_ship = other.GetComponent<SpaceshipController>();
            if(!space_ship)
                return;
            OnSpaceshipEnter();
            space_ship.OnEnterSpaceBodyRange(this);
        } 
        private void OnTriggerExit(Collider other)
        {
            var space_ship = other.GetComponent<SpaceshipController>();
            if(!space_ship)
                return;
            OnSpaceshipExit();
            space_ship.OnExitSpaceBodyRange(this);
        }

        public float GetSqrPlayerDistance() => transform.position.sqrMagnitude;

        protected float GetTime() => Time.timeSinceLevelLoad + data.time_offset;

        public virtual void OnSpaceshipEnter(){}
        public virtual void OnSpaceshipExit(){}
        public virtual void OnSpaceshipEnterChunk(){}
        public virtual void OnSpaceshipExitChunk(){}
    }
}
