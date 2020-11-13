using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    [DebuggerDisplay("position={position}, planet count={space_bodies.Count}")]
    public class SpaceChunk : MonoBehaviour
    {
        private int resolution;
        private Random random;
        private int size;
        public Vector3Int position;

        private readonly List<SpaceBody> space_bodies = new List<SpaceBody>();

        public IEnumerator Init(Random random_, int chunk_size, int chunk_resolution, Vector3Int pos)
        {
            random = random_;
            size = chunk_size;
            resolution = chunk_resolution;
            position = pos;
            
            /*var color = new Color(random.NextFloat(0.5f, 1), random.NextFloat(0.5f, 1), random.NextFloat(0.5f, 1));
            var mat = GetComponentInChildren<MeshRenderer>().material;
            mat.color = color;
            mat.SetColor(Global.emission, color);
            yield break;//*/

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
                var script = new_solar_system.GetComponent<Solarsystem>();
                script.Init( random.Next());
                space_bodies.Add(script);
                new_solar_system.transform.parent = transform;
                // yield break;
                yield return null;
            }
        }


        public void UpdateChunk(Random new_random, Vector3Int new_position, Vector3Int generator_location)
        {
            // Log.print($"move chunk from {position} to {new_position}");
            foreach (var space_body in space_bodies)
            {
                space_body.Destroy();
            }
            space_bodies.Clear();

            transform.localPosition = new_position * size;
            StartCoroutine(Init(new_random, size, resolution, new_position - generator_location));

        }

        public void UpdatePosition(Vector3Int chunk_location)
        {
            // Log.print($"updated chunk pos from {position} to {(position + chunk_location)}, chunk loc={chunk_location}");
            transform.localPosition = (position + chunk_location) * size;
        }
    }
}
