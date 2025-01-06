using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of the character
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public Transform cameraTransform; // Reference to the camera's transform for pitch rotation

    private float verticalRotation = 0f; // For clamping vertical rotation (pitch)
    private Rigidbody rb;

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the Rigidbody component attached to the character
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");  // Left and right movement
        float moveZ = Input.GetAxis("Vertical");    // Forward and backward movement

        // Create a movement vector based on the input
        Vector3 movement = transform.right * moveX + transform.forward * moveZ;

        // Apply the movement to the character
        rb.MovePosition(transform.position + movement.normalized * moveSpeed * Time.deltaTime);

        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the character on the Y-axis (yaw) based on horizontal mouse movement
        transform.Rotate(Vector3.up * mouseX);

        // Adjust the camera's vertical rotation (pitch) based on vertical mouse movement
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Clamp to avoid looking too far up/down

        // Apply the pitch rotation to the camera
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
