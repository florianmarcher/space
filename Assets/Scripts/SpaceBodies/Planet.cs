using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class Planet : SpaceBody
    {
        private Transform pivot;
        [SerializeField] private float distance;
        [SerializeField] private float speed;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.localPosition = new Vector3(distance * Mathf.Sin((Time.timeSinceLevelLoad + time_offset) * speed), distance * Mathf.Cos((Time.timeSinceLevelLoad + time_offset) * speed));
            transform.localRotation = Quaternion.Euler(0, 0, rotation_speed * Time.timeSinceLevelLoad);
        }

        public void Init(Transform p, float s, float d, int seed_)
        {
            base.Init(seed_, s);
            distance = d;
            seed = seed_;
            pivot = p;
            speed = random.NextFloat(0.002f, 0.01f);
            rotation_speed = random.NextFloat(1, -10);
        
            transform.localScale = Vector3.one * size;
        
            var color = new Color(random.NextFloat(0.5f, 1), random.NextFloat(0.5f, 1), random.NextFloat(0.5f, 1));
            var material = GetComponent<MeshRenderer>().material;
            material.color = color;
            material.SetColor(Global.emission, color * 0.05f);

        }
    
    }
}
