using System;
using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class Planet : SpaceBody
    {
        private Transform pivot;
        [SerializeField] private float distance;
        [SerializeField] private float speed;

        public Vector3 delta_position;
        public SphereCollider[] colliders;

        private void Awake()
        {
            colliders = GetComponents<SphereCollider>();
        }

        // Update is called once per frame
        /*void Update()
        {
            transform.localPosition = new Vector3(distance * Mathf.Sin((Time.timeSinceLevelLoad + time_offset) * speed), distance * Mathf.Cos((Time.timeSinceLevelLoad + time_offset) * speed));
            transform.localRotation = Quaternion.Euler(0, 0, rotation_speed * Time.timeSinceLevelLoad);
        }*/

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
            material.SetColor(Global.emission, color * 0.1f);

            foreach (var sphere_collider in colliders)
                sphere_collider.enabled = false;

            transform.localPosition = new Vector3(distance * Mathf.Sin((Time.timeSinceLevelLoad + time_offset) * speed), distance * Mathf.Cos((Time.timeSinceLevelLoad + time_offset) * speed));
        }

        public override Vector3 AddPlanetMovementFactor(Vector3 movement)
        {
            return movement * 0.05f;
        }

        public void SetCollisionEnabled(bool new_enabled)
        {
            
            foreach (var sphere_collider in colliders)
                sphere_collider.enabled = new_enabled;
            transform.GetChild(0).gameObject.SetActive(new_enabled);
        }
    }
}
