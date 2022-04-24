using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class Solarsystem : SpaceBody
    {
        private readonly List<Planet> planets = new();

        private float brightness;

        private SphereCollider sphere_collider;
        private SphereCollider sphere_trigger;

        // Start is called before the first frame update
        public void Init(int seed_)
        {
            data.random = new Random(seed_);
            base.Init(seed_, data.random.NextFloat(10, 40));
            brightness = data.random.NextFloat(450, 800);
            
            transform.localRotation = Quaternion.Euler(data.random.NextVector3(Vector3.one * 360));
            var color = data.random.NextVector3(0.7f, 1).ToColor();
            var material = GetComponent<MeshRenderer>().material;
            material.color = color;
            material.SetColor(Global.emission, color);
            var point_light = GetComponentInChildren<Light>();
            point_light.color = color;
            point_light.intensity = brightness;
        
            var colliders = GetComponents<SphereCollider>();
            sphere_collider = colliders.FirstOrDefault(c => !c.isTrigger);
            sphere_trigger = colliders.FirstOrDefault(c => c.isTrigger);
            Debug.Assert(sphere_collider != null, nameof(sphere_collider) + " == null");
            Debug.Assert(sphere_trigger != null, nameof(sphere_trigger) + " == null");
         //   sphere_collider.enabled = false;
         //   sphere_trigger.enabled = false;
        }

        private void CreatePlanets()
        {
            
            var planet_count = data.random.Next(5);
            for (var i = 0; i < planet_count; i++)
            {
                var planet_size = data.random.NextFloat(0.5F, 0.25F);
                var distance = 1.5F + data.random.NextFloat(1F * i, 1F * (i + 1));
            
                var new_planet = Instantiate(SpaceGenerator.instance.planet, transform).GetComponent<Planet>();
                new_planet.Init(transform, planet_size, distance, data.random.Next());
                planets.Add(new_planet);
            }
        }

        public override void OnSpaceshipEnterChunk()
        {
          //  sphere_trigger.enabled = true;
            CreatePlanets();
        }
        
        public override void OnSpaceshipExitChunk()
        {
     //       sphere_trigger.enabled = false;
            foreach (var planet in planets)
            {
                planet.Destroy();
            }
            planets.Clear();
        }

        public override void OnSelectTarget()
        {
            sphere_trigger.enabled = false;
        }

        public override void OnDeselectTarget()
        {
            sphere_trigger.enabled = true;
        }

        public override List<SpaceBody> Children() => new(planets);
    }
}
