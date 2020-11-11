using Misc;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 25f;
    [SerializeField] private float accelerationSpeed = 45f;
    [SerializeField] private Transform camera_target;
    [SerializeField] private Transform camera_base;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float cameraSmooth = 4f;
    [SerializeField] private float cameraRotationSmooth = 4f;
    
    private float speed;
    [SerializeField]private Rigidbody r;
    private Quaternion look_rotation;
    private float right_smooth;
    private float up_smooth;
    private Vector3 camera_target_offset;
    
    // Start is called before the first frame update
    void Start()
    {
        // r = GetComponent<Rigidbody>();
        r.useGravity = false;
        look_rotation = transform.rotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        var shift = Input.GetKey(KeyCode.LeftShift);
        
        if(shift)
        {
            speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 10);
            camera_target_offset = transform.TransformDirection(Vector3.back * 0.1f);
        }
        else
        {
            speed = Mathf.Lerp(speed, normalSpeed, Time.deltaTime * 10);
            camera_target_offset = Vector3.zero;
        }

        //Set moveDirection to the vertical axis (up and down keys) * speed
        var move_direction = new Vector3(0, 0, speed);
        //Transform the vector3 to local space
        move_direction = transform.TransformDirection(move_direction);
        //Set the velocity, so you can move
        r.velocity = move_direction * -1;

        //Camera follow
        Transform main_camera_transform;
        (main_camera_transform = mainCamera.transform).position = Vector3.Lerp(mainCamera.transform.position, camera_target.position + camera_target_offset, Time.deltaTime * cameraSmooth);
        mainCamera.transform.rotation = Quaternion.Lerp(main_camera_transform.rotation, camera_target.rotation, Time.deltaTime * cameraSmooth);
        
        float right = 0;
        float up = 0;
        var rotation_z_tmp = 0;
        
        if (Input.GetKey(KeyCode.W))
            up = 1;
        if (Input.GetKey(KeyCode.A))
            right = -1;
        if (Input.GetKey(KeyCode.S))
            up = -1;
        if (Input.GetKey(KeyCode.D))
            right = 1;
        if (Input.GetKey(KeyCode.Q))
            rotation_z_tmp = -1;
        if (Input.GetKey(KeyCode.E))
            rotation_z_tmp = 1;
        // var rotation_z_tmp = Input.GetAxis("Mouse X");
        right_smooth = Mathf.Lerp(right_smooth, right * rotationSpeed, Time.deltaTime * cameraSmooth);
        up_smooth = Mathf.Lerp(up_smooth, up * rotationSpeed, Time.deltaTime * cameraSmooth);
        var local_rotation = Quaternion.Euler(-up_smooth, right_smooth, rotation_z_tmp * -rotationSpeed);
        look_rotation *= local_rotation;
        transform.rotation = look_rotation;


        var mouse_x = Input.GetAxis("Mouse X");
        var mouse_y = Input.GetAxis("Mouse Y");

        var max = 1;
        mouse_x = Mathf.Clamp(mouse_x, -max, max);
        mouse_y = Mathf.Clamp(mouse_y, -max, max);
        
        var camera_rotation = new Vector3(-mouse_y, mouse_x, 0) * 2f;
        Log.print("rot: " + camera_rotation);
        
        if(shift)
            camera_base.localRotation = Quaternion.Lerp(Quaternion.Euler(camera_rotation * 2) * camera_base.localRotation, Quaternion.identity, Time.deltaTime * cameraRotationSmooth);
        else
            camera_base.localRotation = Quaternion.Euler(camera_rotation + camera_base.localRotation.eulerAngles);
    }
}
