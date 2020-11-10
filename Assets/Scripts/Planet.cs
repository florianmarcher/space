using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Transform pivot;
    private float size;
    private float distance;
    private float speed;
    private uint seed;
    private Rand rand;
    private float time_offset;
    private float rotation_speed;
    
    
    
    
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

    public void Init(Transform p, float s, float d, uint seed_)
    {
        size = s;
        distance = d;
        seed = seed_;
        rand = new Rand(seed_);
        speed = (float) (rand.NextDouble() * 0.1 + 0.02);
        time_offset = (float) (rand.NextDouble() * 1000);
        rotation_speed = (float) (rand.NextDouble() * -100);
        
        transform.localScale = Vector3.one * size;
        
        var color = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
        GetComponent<MeshRenderer>().material.color = color;
    }
    
}
