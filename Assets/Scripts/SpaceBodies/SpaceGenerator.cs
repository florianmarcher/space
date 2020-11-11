using System.Collections;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class SpaceGenerator : MonoBehaviour
    {
        [SerializeField] private int renderDistance = 10;
        [SerializeField] private int chunk_resolution = 10;
        [SerializeField] private int chunk_size = 10;
    
        [SerializeField] public GameObject planet;
        [SerializeField] public GameObject solar_system;
        [SerializeField] public GameObject space_chunk;

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
                var chunk = Instantiate(space_chunk, element * chunk_size, Quaternion.identity);
                StartCoroutine(chunk.GetComponent<SpaceChunk>().Init(random, chunk_size, chunk_resolution));
                chunk.transform.parent = transform;
                Log.print("finished " + element);
                // yield break;
                yield return null;
            }

            Log.print("finished generating");
        }
    }
}
