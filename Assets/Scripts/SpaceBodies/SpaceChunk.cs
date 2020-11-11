using System.Collections;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class SpaceChunk : MonoBehaviour
    {
        private int resolution;
        private Random random;
        private int size;

        public IEnumerator Init(Random random_, int chunk_size, int chunk_resolution)
        {
            random = random_;
            size = chunk_size;
            resolution = chunk_resolution;

            var element = Vector3Int.one;

            for (element.x = 0; element.x < resolution; element.x++)
            for (element.y = 0; element.y < resolution; element.y++)
            for (element.z = 0; element.z < resolution; element.z++)
            {

                const float noise_scale = 0.2658473f;
                var noise = Noise.PerlinNoise3D(element.x * noise_scale, element.y * noise_scale, element.z * noise_scale);
                var should_spawn = random.NextFloat() < noise;
            
                if(!should_spawn)
                    continue;
            
                var offset = random.NextVector3(-0.5f, 0.5f);

                var new_solar_system = Instantiate(SpaceGenerator.instance.solar_system, (element + offset) * size / resolution + transform.position, Quaternion.identity);
                new_solar_system.GetComponent<Solarsystem>().Init( random.Next());
                new_solar_system.transform.parent = transform;
                // yield break;
                yield return null;
            }
        }
    }
}
