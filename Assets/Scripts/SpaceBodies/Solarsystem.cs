using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class Solarsystem : SpaceBody
    {
        private List<Planet> planets = new List<Planet>();

        private float brightness;

        private SphereCollider sphere_collider;
        private SphereCollider sphere_trigger;

        // Start is called before the first frame update
        public void Init(int seed_)
        {
            random = new Random(seed_);
            base.Init(seed_, random.NextFloat(500, 2000));
            brightness = random.NextFloat(450, 800);
            
            transform.localRotation = Quaternion.Euler(random.NextVector3(Vector3.one * 360));
            var color = new Color(random.NextFloat(0.7f, 1), random.NextFloat(0.7f, 1), random.NextFloat(0.7f, 1));
            var material = GetComponent<MeshRenderer>().material;
            material.color = color;
            material.SetColor(Global.emission, color);
            var point_light = GetComponentInChildren<Light>();
            point_light.color = color;
            point_light.intensity = brightness;
        
            var colliders = GetComponents<SphereCollider>();
            sphere_collider = colliders.FirstOrDefault(c => !c.isTrigger);
            sphere_trigger = colliders.FirstOrDefault(c => c.isTrigger);
            Debug.Assert(sphere_collider != null, nameof(sphere_collider) + " != null");
            Debug.Assert(sphere_trigger != null, nameof(sphere_trigger) + " != null");
            sphere_collider.enabled = false;
            sphere_trigger.enabled = false;
        }

        private void CreatePlanets()
        {
            
            var planet_count = random.Next(5);
            for (var i = 0; i < planet_count; i++)
            {
                var planet_size = random.NextFloat(0.1f, 0.5f);
                var distance = 0.03f + random.NextFloat(0.02f * i, 0.02f * (i + 1));
            
                var new_planet = Instantiate(SpaceGenerator.instance.planet, transform).GetComponent<Planet>();
                new_planet.Init(transform, planet_size, distance, random.Next());
                planets.Add(new_planet);
            }
        }

        public override Vector3 AddPlanetMovementFactor(Vector3 movement)
        {
            return movement * 0.5f;
        }

        public override void OnSpaceshipEnter()
        {
            sphere_collider.enabled = true;
            foreach (var planet in planets)
            {
                planet.SetCollisionEnabled(true);
            }
        }

        public override void OnSpaceshipExit()
        {
            sphere_collider.enabled = false;
            foreach (var planet in planets)
            {
                planet.SetCollisionEnabled(false);
            }
        }

        public override void OnSpaceshipEnterChunk()
        {
            sphere_trigger.enabled = true;
            CreatePlanets();
        }
        
        public override void OnSpaceshipExitChunk()
        {
            sphere_trigger.enabled = false;
            foreach (var planet in planets)
            {
                planet.Destroy();
            }
            planets.Clear();
        }
    }
}
