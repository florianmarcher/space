using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private int renderDistance = 10;
    [SerializeField] private int spacing = 10;
    [SerializeField] private Object sphere;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(GeneratePlanets));
    }

    IEnumerator GeneratePlanets() 
    {
        Vector3Int element = Vector3Int.one * renderDistance;

        for (element.x = -renderDistance; element.x <= renderDistance; element.x++)
        for (element.y = -renderDistance; element.y <= renderDistance; element.y++)
        for (element.z = -renderDistance; element.z <= renderDistance; element.z++)
        {
            var tmp = (long)element.x << 32;
            var seed = (ulong) Math.Abs(((element.x + renderDistance) << 8) | ((element.y + renderDistance) << 24) | ((long)(element.z + renderDistance) << 40) + 1);
            // Debug.Log("seed for " + element + " is " + seed);
            var random = new Rand(seed);

            var noise_scale = 0.2658473f;
            var noise = Noise.PerlinNoise3D(element.x * noise_scale, element.y * noise_scale, element.z * noise_scale);
            var should_spawn = (random.Next() % 10) / 10.0f < noise;
            
            
            if(!should_spawn)
                continue;
            
            var offset = new Vector3((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble()) - Vector3.one * 0.5f;

            // Debug.Log(offset);
            var solar_system = (GameObject)Instantiate(sphere, (element + offset) * spacing, Quaternion.identity);
            solar_system.GetComponent<Solarsystem>().Init(random.Next());
            solar_system.transform.localScale = Vector3.one * 30;
            solar_system.transform.parent = transform;
            yield return null;

        }
        
    }

    
    
}
