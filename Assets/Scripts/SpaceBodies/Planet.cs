using System;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEditor;
using UnityEngine;

namespace SpaceBodies
{
    public class Planet : SpaceBody
    {
        [SerializeField] private bool animate_camera;
        
        
        private Transform pivot;
        [SerializeField] private float distance;
        [SerializeField] private float speed;
        [SerializeField] private bool movement_enabled;
        [SerializeField] private int mesh_resolution;

        private int current_mesh_resolution;

        [SerializeField] private MeshFilter mesh;

        [SerializeField, Header("Debug World Position")]
        private Vector3 world_position;

        public Vector3 delta_position;
        public SphereCollider[] colliders;

        [SerializeField] private Noise noise;

        private void Start()
        {
            
            OnDeselectTarget();
        }

        public override void OnDeselectTarget()
        {
            current_mesh_resolution = 3;
            GenerateMesh(mesh.mesh);
        }

        public override void OnSelectTarget()
        {
            current_mesh_resolution = mesh_resolution;
            GenerateMesh(mesh.mesh);
        }

        // Update is called once per frame
        void Update()
        {
            if (movement_enabled)
                Move();
            world_position = transform.position;
        }

        private void Move()
        {
            transform.localPosition = new Vector3(distance * Mathf.Sin(GetTime() * speed),
                distance * Mathf.Cos(GetTime() * speed));
            transform.localRotation = Quaternion.Euler(0, 0, data.rotation_speed * GetTime());
        }

        public void Init(Transform p, float radius, float distance, int seed_)
        {
            base.Init(seed_, radius);
            this.distance = distance;
            pivot = p;
            speed = data.random.NextFloat(0.002f, 0.01f);
            data.rotation_speed = data.random.NextFloat(1, -10);

            transform.localScale = Vector3.one * data.radius;

            var color = data.random.NextVector4(0.5f, 1f);
            var material = GetComponent<MeshRenderer>().material;
            material.color = color;
            material.SetColor(Global.emission, color * 0.1f);

         //   foreach (var sphere_collider in colliders)
         //       sphere_collider.enabled = false;

            transform.localPosition = new Vector3(this.distance * Mathf.Sin(GetTime() * speed),
                this.distance * Mathf.Cos(GetTime() * speed));

            GenerateMesh(mesh.mesh);
        }


        private enum Side {Left, Right, Up, Down, Forward, Back}

        private void GenerateMesh(Mesh m)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var uvs = new List<Vector2>();

            foreach (Side side in Enum.GetValues(typeof(Side)))
            {
                var face = GenerateFace(side);

                var index_offset = vertices.Count;

                vertices.AddRange(face.vertices);
                indices.AddRange(face.indices.Select(i => i + index_offset));

            }
            
            m.Clear();
            m.vertices = vertices.ToArray();
            m.triangles = indices.ToArray();
            m.uv = vertices.Select(v => new Vector2(v.x, v.z)).ToArray();
            m.RecalculateNormals();
        }


        private (List<Vector3> vertices, List<int> indices, List<Vector2> uvs) GenerateFace(Side side)
        {
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            var uvs = new List<Vector2>();

            for (int x = 0; x < current_mesh_resolution; x++)
            for (int y = 0; y < current_mesh_resolution; y++)
            {
                var x_pos = (float) x / (current_mesh_resolution - 1) - 0.5f;
                var y_pos = (float) y / (current_mesh_resolution - 1) - 0.5f;

                const float half = 0.5f;
                var vertex = side switch {
                    Side.Left => new Vector3(-half, -x_pos, -y_pos),
                    Side.Right => new Vector3(half, -x_pos, y_pos),
                    Side.Up => new Vector3(x_pos, half, y_pos),
                    Side.Down => new Vector3(x_pos, -half, -y_pos),
                    Side.Forward => new Vector3(-x_pos, y_pos, half),
                    Side.Back => new Vector3(x_pos, y_pos, -half),
                    _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
                };
                vertex.Normalize();

                var n = (float)(noise.GetNoise(vertex.ToFloatArray()) * 0.01f + 1f);

                vertices.Add(vertex * data.diameter * n);

                if (x == 0 || y == 0)
                    continue;

                indices.AddRange(new[]
                {
                    vertices.Count - 1, vertices.Count - 2, vertices.Count - 1 - current_mesh_resolution,
                    vertices.Count - 2 - current_mesh_resolution, vertices.Count - 1 - current_mesh_resolution, vertices.Count - 2
                });
            }
            
            return (vertices, indices, uvs);
        }


        private void OnValidate()
        {
            colliders = GetComponents<SphereCollider>();
            mesh = GetComponent<MeshFilter>();
            GenerateMesh(GetComponent<MeshFilter>().sharedMesh);
          
          //  data.random = new Random(0);
          //  Init(GameObject.Find("Parent").transform, data.radius, distance, data.seed);
        }
    }
    
    
    
}
