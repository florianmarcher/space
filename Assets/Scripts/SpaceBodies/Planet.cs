using Misc;
using UnityEngine;

namespace SpaceBodies
{
    public class Planet : SpaceBody
    {
        private Transform pivot;
        [SerializeField] private float distance;
        [SerializeField] private float speed;
        [SerializeField] private bool movement_enabled;

        public Vector3 delta_position;
        public SphereCollider[] colliders;

        private void Awake()
        {
            colliders = GetComponents<SphereCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (movement_enabled)
                Move();
        }

        private void Move()
        {
            transform.localPosition = new Vector3(distance * Mathf.Sin(GetTime() * speed),
                distance * Mathf.Cos(GetTime() * speed));
            transform.localRotation = Quaternion.Euler(0, 0, data.rotation_speed * GetTime());
        }

        public void Init(Transform p, float s, float d, int seed_)
        {
            base.Init(seed_, s);
            distance = d;
            pivot = p;
            speed = data.random.NextFloat(0.002f, 0.01f);
            data.rotation_speed = data.random.NextFloat(1, -10);

            transform.localScale = Vector3.one * data.radius;

            var color = data.random.NextVector4(0.5f, 1f);
            var material = GetComponent<MeshRenderer>().material;
            material.color = color;
            material.SetColor(Global.emission, color * 0.1f);

            foreach (var sphere_collider in colliders)
                sphere_collider.enabled = false;

            transform.localPosition = new Vector3(distance * Mathf.Sin(GetTime() * speed),
                distance * Mathf.Cos(GetTime() * speed));
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
