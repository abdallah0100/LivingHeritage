using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class MobileFPSController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float gravity = -9.8f;

    [Header("Look")]
    public float lookSensitivity = 0.2f;

    [Header("References")]
    public Joystick joystick;
    public Camera cam;

    PlayerControls controls;
    CharacterController controller;

    Vector2 lookDelta;
    float pitch;
    float verticalVelocity;

    bool hasMovedOnce = false; // 🔑 THE KEY FIX

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        controls.Camera.Look.performed += ctx =>
        {
            if (!IsTouchOverUI())
                lookDelta = ctx.ReadValue<Vector2>();
            else
                lookDelta = Vector2.zero;
        };

        controls.Camera.Look.canceled += _ => lookDelta = Vector2.zero;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        LookAround();
        MovePlayer();
    }

    // ---------------- MOVE ----------------
    void MovePlayer()
    {
        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        Vector3 moveDir =
            transform.forward * v +
            transform.right * h;

        // 🔒 DO NOT apply gravity until player actually moved once
        if (hasMovedOnce)
        {
            if (controller.isGrounded)
                verticalVelocity = -2f;
            else
                verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity =
            moveDir * moveSpeed +
            Vector3.up * verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        // Mark that first real movement happened
        if (!hasMovedOnce && moveDir.sqrMagnitude > 0.001f)
            hasMovedOnce = true;
    }

    // ---------------- LOOK ----------------
    void LookAround()
    {
        if (IsTouchOverUI())
            return;

        Vector2 delta = lookDelta * lookSensitivity;

        transform.Rotate(Vector3.up * delta.x);

        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // ---------------- UI BLOCK CHECK ----------------
    bool IsTouchOverUI()
    {
        if (Touchscreen.current == null)
            return false;

        foreach (var touch in Touchscreen.current.touches)
        {
            if (!touch.press.isPressed) continue;

            if (EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
                return true;
        }
        return false;
    }
}
