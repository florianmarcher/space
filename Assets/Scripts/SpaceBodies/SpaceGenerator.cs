using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using Space.Scripts;
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

        [SerializeField] private Vector3Int generator_location;
        private readonly List<SpaceChunk> chunks = new List<SpaceChunk>();

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
            foreach (Vector3Int element in new SpiralIterator(renderDistance))
            {
                var seed = (element - generator_location).GetHashCode().GetHashCode();
                var random = new Random(seed);
                var chunk = Instantiate(space_chunk, element * chunk_size, Quaternion.identity);
                var script = chunk.GetComponent<SpaceChunk>();
                chunks.Add(script);
                StartCoroutine(script.Init(random, chunk_size, chunk_resolution, element));
                chunk.transform.parent = transform;
                Log.Print("finished " + element);
                // yield break;
                yield return null;
            }

            Log.Print("finished generating");
        }

        private void Update()
        {
            var tmp_pos = transform.position / chunk_size;
            var new_pos = new Vector3Int((int) tmp_pos.x, (int) tmp_pos.y, (int) tmp_pos.z);
            if (new_pos == Vector3Int.zero)
                return;
            generator_location += new_pos;
            Log.Print("Update Chunks");
            UpdateChunks();
            transform.position -= new_pos * chunk_size;
        }
        
        private void UpdateChunks()
        {
            var to_remove = chunks.Where(chunk => !IsInRenderDistance(chunk)).ToArray();
            var element = Vector3Int.one;
            var i = 0;
            
            for (element.x = -renderDistance; element.x <= renderDistance; element.x++)
            for (element.y = -renderDistance; element.y <= renderDistance; element.y++)
            for (element.z = -renderDistance; element.z <= renderDistance; element.z++)
            {
                var chunk = chunks.FirstOrDefault(c => c.position == element - generator_location);
                if(chunk != default)
                {
                    if(element.sqrMagnitude < 2.5)
                        chunk.OnSpaceShipEnter();
                    else
                        chunk.OnSpaceShipExit();
                    
                    chunk.UpdatePosition(generator_location);
                    continue;
                }                
                var seed = (element - generator_location).GetHashCode().GetHashCode();
                var random = new Random(seed);

                chunk = to_remove[i++];
                chunk.UpdateChunk(random, element, generator_location);
            }
        }

        private bool IsInRenderDistance(SpaceChunk chunk)
        {
            return chunk.position.x >= -generator_location.x - renderDistance && chunk.position.x <= -generator_location.x + renderDistance && 
                   chunk.position.y >= -generator_location.y - renderDistance && chunk.position.y <= -generator_location.y + renderDistance && 
                   chunk.position.z >= -generator_location.z - renderDistance && chunk.position.z <= -generator_location.z + renderDistance;
        }
    }
}
