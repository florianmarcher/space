using System;
using Misc;
using SpaceBodies;
using UnityEngine;

public class FloatController : MonoBehaviour, IPlayerController
{
    private Camera camera;

    [SerializeField] private float camera_distance_planet = 10;
    [SerializeField] private float camera_distance_solarsystem = 10;
    [SerializeField] private float movement_lerp_speed = 0.8f;
    [SerializeField] private float mouse_sensitivity = 1;
    [SerializeField] private Rigidbody space_rigidbody;
    private bool is_at_target = false;
    private int ui_index;

    [SerializeField] private SpaceBody target;

    // Start is called before the first frame update
    void Start()
    {
        this.SkipFrame(() =>
        {
            target = FindObjectOfType<Planet>();
            if(target)
                target.OnSelectTarget();
        });
        ui_index = FramesPerSecond.instance.AddNewText("not at target");
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        MoveTowardsTarget();

        if (!target)
        {
            target = FindObjectOfType<Planet>();
            if(target)
                target.OnSelectTarget();
            print($"found new target {target}");
        }

        var target_distance = target is Planet ? camera_distance_planet : camera_distance_solarsystem;

        camera.transform.localPosition =
            Vector3.Lerp(camera.transform.localPosition, Vector3.back * target_distance, 0.8f * Time.deltaTime);
                                                  ;
    }

    private void HandleInput()
    {
        var right = Input.GetAxis("Horizontal");
        var up = Input.GetAxis("Vertical");

        transform.localRotation *= Quaternion.Euler(up, -right, 0);

        if (Input.GetMouseButtonDown(0))
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, float.PositiveInfinity))
                return;

            var planet = hit.collider.GetComponent<SpaceBody>();
            if (!planet)
                return;
            if(target)
                target.OnDeselectTarget();
            
            target = planet;
            is_at_target = false;
            target.OnSelectTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (!target || is_at_target)
            return;

        var direction = target.transform.position - transform.position;

        var movement = direction * movement_lerp_speed;

        if (direction.magnitude < 0.1f)
        {
            is_at_target = true;
            space_rigidbody.transform.position -= target.transform.position;
            space_rigidbody.velocity = Vector3.zero;
            FramesPerSecond.instance.ModifyText("is at target", ui_index);
            return;
        }

        //Set moveDirection to the vertical axis (up and down keys) * speed
        //    var move_direction = new Vector3(0, 0, speed);
        //Transform the vector3 to local space
        //  move_direction = transform.TransformDirection(move_direction);
        //Set the velocity, so you can move
        space_rigidbody.velocity = -movement;
    }


    private void OnValidate()
    {
        camera = GetComponentInChildren<Camera>();
    }

    public void OnEnterSpaceBodyRange(SpaceBody spaceBody)
    {
        throw new NotImplementedException();
    }

    public void OnExitSpaceBodyRange(SpaceBody spaceBody)
    {
        throw new NotImplementedException();
    }
}
