using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    // Rotation limits
    public float xLimit = 45f;  // Limit for horizontal (yaw) movement (+/- degrees)
    public float yLimit = 30f;  // Limit for vertical (pitch) movement (+/- degrees)

    private Vector3 initialRotation; 
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        // Store the initial camera rotation
        initialRotation = transform.eulerAngles;
        yaw = initialRotation.y;
        pitch = initialRotation.x;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Update yaw (horizontal rotation) and clamp it
        yaw += mouseX;
        yaw = Mathf.Clamp(yaw, initialRotation.y - xLimit, initialRotation.y + xLimit);

        // Update pitch (vertical rotation) and clamp it
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, initialRotation.x - yLimit, initialRotation.x + yLimit);

        // Apply the clamped rotation to the camera
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
