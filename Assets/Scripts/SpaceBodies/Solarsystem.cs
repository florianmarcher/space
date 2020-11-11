using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class Solarsystem : SpaceBody
    {
        private GameObject[] planets;

        private float brightness;


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
        
            CreatePlanets();
        }

        private void CreatePlanets()
        {
            
            var planet_count = random.Next(5);
            planets = new GameObject[planet_count];
            for (var i = 0; i < planet_count; i++)
            {
                var planet_size = random.NextFloat(0.1f, 0.5f);
                var distance = 0.03f + random.NextFloat(0.02f * i, 0.02f * (i + 1));
            
                planets[i] = Instantiate(SpaceGenerator.instance.planet, transform);
                planets[i].GetComponent<Planet>().Init(transform, planet_size, distance, random.Next());
            }
        }
    }
}
