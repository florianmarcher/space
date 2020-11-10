using System.Collections;
using SpaceBodies;
using UnityEngine;

public class SpaceGenerator : MonoBehaviour
{
    [SerializeField] private int renderDistance = 10;
    [SerializeField] private int spacing = 10;
    
    [SerializeField] public GameObject planet;
    [SerializeField] public GameObject solar_system;

    public static SpaceGenerator instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(nameof(GeneratePlanets));
    }

    private IEnumerator GeneratePlanets() 
    {
        var element = Vector3Int.one * renderDistance;

        for (element.x = -renderDistance; element.x <= renderDistance; element.x++)
        for (element.y = -renderDistance; element.y <= renderDistance; element.y++)
        for (element.z = -renderDistance; element.z <= renderDistance; element.z++)
        {
            var seed = element.GetHashCode().GetHashCode();
            var random = new Random(seed);

            const float noise_scale = 0.2658473f;
            var noise = Noise.PerlinNoise3D(element.x * noise_scale, element.y * noise_scale, element.z * noise_scale);
            var should_spawn = random.NextFloat() < noise;
            
            if(!should_spawn)
                continue;
            
            var offset = random.NextVector3(-0.5f, 0.5f);

            var new_solar_system = Instantiate(solar_system, (element + offset) * spacing, Quaternion.identity);
            new_solar_system.GetComponent<Solarsystem>().Init( random.Next());
            new_solar_system.transform.parent = transform;
            yield return null;

        }
        
    }

    
    
}
