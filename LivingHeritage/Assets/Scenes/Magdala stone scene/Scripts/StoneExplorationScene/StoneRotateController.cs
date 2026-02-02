using UnityEngine;
using UnityEngine.InputSystem;

public class StoneRotateController : MonoBehaviour
{
    public float rotationSpeed = 0.4f;

    [Header("Vertical Rotation Limits")]
    public float minVerticalAngle = -45f;
    public float maxVerticalAngle = 45f;

    private Vector2 lastPosition;
    private bool isDragging = false;
    private float currentVerticalAngle = 0f;

    void Update()
    {
        // ✅ TOUCH (Simulator + Mobile)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!isDragging)
            {
                isDragging = true;
                lastPosition = currentPos;
                return;
            }

            Rotate(currentPos);
        }
        // ✅ MOUSE (Editor)
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 currentPos = Mouse.current.position.ReadValue();

            if (!isDragging)
            {
                isDragging = true;
                lastPosition = currentPos;
                return;
            }

            Rotate(currentPos);
        }
        else
        {
            isDragging = false;
        }
    }

    void Rotate(Vector2 currentPosition)
    {
        Vector2 delta = currentPosition - lastPosition;

        // Horizontal (Y)
        transform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);

        // Vertical (X)
        float verticalRotation = delta.y * rotationSpeed;
        float newAngle = Mathf.Clamp(
            currentVerticalAngle + verticalRotation,
            minVerticalAngle,
            maxVerticalAngle
        );

        float angleDelta = newAngle - currentVerticalAngle;
        transform.Rotate(Vector3.right, angleDelta, Space.Self);
        currentVerticalAngle = newAngle;

        lastPosition = currentPosition;
    }
}
