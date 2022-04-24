using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    [Serializable]
    public struct SpaceBodyData
    {
        public Random random;
        public int seed;
        public float radius;
        public float rotation_speed;
        public float time_offset;
        public string name;

        public float diameter => radius * 2;

        //new
        public float density;
        public float tilt;

        public float volume;
        public float mass;
    }

    public interface ISpaceBodyParent
    {
        List<SpaceBody> Children();
    }


    public abstract class SpaceBody : MonoBehaviour, ISpaceBodyParent
    {
        [SerializeField] protected SpaceBodyData data;


        protected void Init(int seed_, float radius)
        {
            data.seed = seed_;
            data.radius = radius;
            data.random ??= new Random(data.seed);
            data.time_offset = (float) (data.random.NextDouble() * 1000);


            data.volume = 4f * Mathf.PI * data.radius * data.radius * data.radius / 3f;
            data.mass = data.density * data.volume;

            data.name = $"{data.seed}: {data.radius}";

            StartCoroutine(Scale(1, 0, data.radius));
        }

        private IEnumerator Scale(float duration, float start, float end, Action on_finish = null)
        {
            transform.localScale = Vector3.one * start;
            var factor = start < end ? 1 : -1;

            while (transform.localScale.x < end)
            {
                transform.localScale += data.radius / duration * Time.deltaTime * factor * Vector3.one;
                yield return null;
            }

            transform.localScale = Vector3.one * data.radius;

            on_finish?.Invoke();
        }

        public void Destroy()
        {
            transform.parent = SpaceGenerator.instance.transform;
            StartCoroutine(Scale(1, data.radius, 0, () => Destroy(gameObject)));
        }

        public virtual Vector3 AddPlanetMovementFactor(Vector3 movement)
        {
            return movement;
        }

        private void OnTriggerEnter(Collider other)
        {
            var space_ship = other.GetComponent<IPlayerController>();
            if (space_ship == null)
                return;
            OnSelectTarget();
            space_ship.OnEnterSpaceBodyRange(this);
        }

        private void OnTriggerExit(Collider other)
        {
            var space_ship = other.GetComponent<IPlayerController>();
            if (space_ship == null)
                return;
            OnDeselectTarget();
            space_ship.OnExitSpaceBodyRange(this);
        }

        public float GetSqrPlayerDistance() => transform.position.sqrMagnitude;


        public Vector3 GetGravityInfluence() => GetGravityInfluence(Vector3.zero);

        public virtual Vector3 GetGravityInfluence(Vector3 destiny)
        {
            return destiny.normalized * -(float) (Constants.gravitational_constant * data.mass) /
                   (data.radius * data.radius);
        }


        protected float GetTime() => Time.timeSinceLevelLoad + data.time_offset;

        public virtual void OnSelectTarget()
        {
        }

        public virtual void OnDeselectTarget()
        {
        }

        public virtual void OnSpaceshipEnterChunk()
        {
        }

        public virtual void OnSpaceshipExitChunk()
        {
        }

        public virtual List<SpaceBody> Children() => new();

        public override string ToString() => GetType() + " " + data.name;
    }
}
