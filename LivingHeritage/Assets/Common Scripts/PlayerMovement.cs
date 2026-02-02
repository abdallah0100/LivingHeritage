using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform player;

    [Header("Mobile Joystick")]
    public Joystick joystick;

    private CharacterController controller;
    private bool isGrounded = false;
    private float xRotation = 0f;
    private Vector3 velocity;
    private ScreenOrientation orientation = ScreenOrientation.LandscapeLeft;

    // --- Touch Look ---
    private int cameraFingerId = -1;
    private Vector2 lastLookPos;

    void Start()
    {
        Screen.orientation = orientation;

        controller = GetComponent<CharacterController>();
        if (player == null)
            player = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        handleMovement();
    }

    // ------------------- MOVEMENT -------------------
    void handleMovement()
    {
        if (controller == null)
            return;
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x, z;

        // Joystick on device
        x = joystick != null ? joystick.Horizontal : 0f;
        z = joystick != null ? joystick.Vertical : 0f;

        // Keyboard in editor
        x += Input.GetAxis("Horizontal");
        z += Input.GetAxis("Vertical");

        Vector3 move = transform.forward * z + transform.right * x;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ------------------- LOOK (Touch + Mouse) -------------------
    void HandleLook()
    {
        // ---------------- TOUCH LOOK (device only) ----------------
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // If no camera finger assigned yet
                if (cameraFingerId == -1)
                {
                    // Must begin on RIGHT HALF of screen
                    if (touch.phase == TouchPhase.Began &&
                        touch.position.x > Screen.width * 0.5f)
                    {
                        cameraFingerId = touch.fingerId;
                        lastLookPos = touch.position;
                    }
                }

                // If this touch *is* the camera finger
                if (touch.fingerId == cameraFingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 delta = touch.position - lastLookPos;
                        RotateCamera(delta);
                        lastLookPos = touch.position;
                    }

                    // When touch ends, release control
                    if (touch.phase == TouchPhase.Ended ||
                        touch.phase == TouchPhase.Canceled)
                    {
                        cameraFingerId = -1;
                    }
                }
            }

            return; // Skip mouse look when using touch
        }

        // ---------------- MOUSE LOOK (Editor) ----------------
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    void RotateCamera(Vector2 delta)
    {
        float lookX = delta.x * mouseSensitivity * 0.002f;
        float lookY = delta.y * mouseSensitivity * 0.002f;

        // Apply rotation
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * lookX);
    }

    void OnApplicationFocus(bool hasFocus)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
#endif
    }
}
