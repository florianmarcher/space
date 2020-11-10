using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solarsystem : MonoBehaviour
{
    public uint seed;
    private GameObject[] planets;
    [SerializeField] private GameObject Planet;

    private Rand rand;
    private float brightness;

    private static readonly int emission = Shader.PropertyToID("_Emission");

    // Start is called before the first frame update
    public void Init(uint seed_)
    {
        seed = seed_;
        rand = new Rand(seed);

        brightness = (float) (rand.NextDouble() * 10f + 2);
        
        transform.localRotation = Quaternion.Euler((float) rand.NextDouble() * 360, (float) rand.NextDouble() * 360, (float) rand.NextDouble() * 360);
        var color = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
        var material = GetComponent<MeshRenderer>().material;
        material.color = color;
        material.SetColor(emission, color);
        var point_light = GetComponentInChildren<Light>();
        point_light.color = color;
        point_light.intensity = brightness;
        
        var planet_count = rand.Next() % 5;
        planets = new GameObject[planet_count];
        for (int i = 0; i < planet_count; i++)
        {
            var size = (float) (rand.NextDouble() * 0.5f + 0.2f);
            var distance = (float)rand.NextDouble() * i * 0.2f + 0.1f;
            
            planets[i] = Instantiate(Planet, transform);
            planets[i].GetComponent<Planet>().Init(transform, size, distance, rand.Next());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
