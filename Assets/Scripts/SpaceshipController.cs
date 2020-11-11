using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 25f;
    [SerializeField] private float accelerationSpeed = 45f;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float cameraSmooth = 4f;
    
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
        
        
        if(Input.GetKey(KeyCode.LeftShift))
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
        (main_camera_transform = mainCamera.transform).position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position + camera_target_offset, Time.deltaTime * cameraSmooth);
        mainCamera.transform.rotation = Quaternion.Lerp(main_camera_transform.rotation, cameraPosition.rotation, Time.deltaTime * cameraSmooth);
        
        float right = 0;
        float up = 0;
        
        if (Input.GetKey(KeyCode.W))
            up = 1;
        if (Input.GetKey(KeyCode.A))
            right = -1;
        if (Input.GetKey(KeyCode.S))
            up = -1;
        if (Input.GetKey(KeyCode.D))
            right = 1;
        /*if (Input.GetKey(KeyCode.Q))
            rotation_z_tmp = rotation_sensitivity;
        if (Input.GetKey(KeyCode.E))
            rotation_z_tmp = -rotation_sensitivity;*/
        var rotation_z_tmp = Input.GetAxis("Mouse X");
        right_smooth = Mathf.Lerp(right_smooth, right * rotationSpeed, Time.deltaTime * cameraSmooth);
        up_smooth = Mathf.Lerp(up_smooth, up * rotationSpeed, Time.deltaTime * cameraSmooth);
        var local_rotation = Quaternion.Euler(-up_smooth, right_smooth, rotation_z_tmp * -rotationSpeed);
        look_rotation *= local_rotation;
        transform.rotation = look_rotation;

    }
}
