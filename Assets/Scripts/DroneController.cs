
using System.Collections;
using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DroneController : MonoBehaviourPun
{

    public Rigidbody rb;
    public Camera _camera;

    public float moveSpeed = 200;
   
    public Animator animator;
    [Space]
    [Header("Particles")]
    

    [Space]
    [Header("Camera move")]
    public float sensitivity = 100f;   // Mouse sensitivity
    public float maxSpeed = 30f;  // Maximum rotation angle (30 degrees)

    private float xMove = 0f; // Current rotation around the X-axis (up/down)

    private float yMove = 0f; // Current rotation around the Y-axis (left/right)

    [Space]



    float xRotation;
    float vUpDownMove;

    LayerMask layer;

   
    // Start is called before the first frame update
    void Start()
    {
        // Check if the object is controlled by the local player
        if (photonView.IsMine)
        {
            // Lock the cursor for the local player
            Cursor.lockState = CursorLockMode.Locked;


            // Set up the camera for the local player
            _camera = GetComponentInChildren<Camera>();

            // Optionally enable Cinemachine or any other camera-related component
            CinemachineVirtualCamera vCam = GetComponentInChildren<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.enabled = true; // Enable only for local player
            }
            
        
        }
        else
        {
            // Disable the camera for remote players
            Camera remoteCamera = GetComponentInChildren<Camera>();
            if (remoteCamera != null)
            {
                remoteCamera.enabled = false; // Disable for non-local players
            }

            // Optionally disable Cinemachine for remote players
            CinemachineVirtualCamera vCam = GetComponentInChildren<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (photonView.IsMine)
        {
            Movement();

        }

    } 
   

    
   

   

    void Movement()
    {

        float finalXInput = 0;
        float finalYInput = 0;
        vUpDownMove = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        if (Mathf.Abs(x) > 0.1)
        {
            xRotation += x * sensitivity;
        }


        finalXInput = Input.GetAxis("Joystick X") + Input.GetAxis("Mouse X");
        finalYInput = Input.GetAxis("Joystick Y") + Input.GetAxis("Mouse Y");


        if (Mathf.Abs(finalXInput) > 0.05 && !IsGrounded() || Mathf.Abs(finalYInput) > 0.05 && !IsGrounded())
        {
            xMove += finalXInput;
            yMove += finalYInput;

        }
        else if (IsGrounded())
        {
            float XcVelocity = 0;
            float YcVelocity = 0;
            xMove = Mathf.SmoothDamp(xMove, 0, ref XcVelocity, 0.05f);
            yMove = Mathf.SmoothDamp(yMove, 0, ref YcVelocity, 0.05f);

        }




        //Up and Down Movement
        if (vUpDownMove > 0.001)
        {
            rb.AddForce(transform.up * vUpDownMove * moveSpeed * rb.mass, ForceMode.Acceleration);
        }
        else
        {
            // float downwardForce = 2.81f; // Adjust this value as needed
            // rb.AddForce(Vector3.down * downwardForce * rb.mass, ForceMode.Acceleration);

        }





        if (rb.velocity.magnitude > maxSpeed)
        {
            // Limit the velocity to the max speed by normalizing and scaling
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        // Check if Space (keyboard) or Joystick B button is pressed
        if (Input.GetButtonDown("Space"))
        {
            // Reset the X and Z rotation of the drone
            Quaternion currentRotation = transform.rotation;
            yMove = 0;
            xMove = 0;
            transform.rotation = Quaternion.Euler(xMove, currentRotation.eulerAngles.y, yMove);
        }

        rb.MoveRotation(Quaternion.Euler(yMove, xRotation, xMove));
        animator.speed = Mathf.Clamp(rb.velocity.magnitude, 4, 10);


    }
    public Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    bool IsGrounded()
    {

        return Physics.BoxCast(transform.position, boxSize / 2, Vector3.down, out RaycastHit hitInfo, Quaternion.identity, boxCastDistance);
    }
    public float boxCastDistance = 0.2f; // Distance to check for ground contact

    void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.red;

        // Calculate the center of the boxcast based on the current position and cast distance
        Vector3 boxCastCenter = transform.position + Vector3.down * (boxCastDistance / 2);

        // Draw a wireframe box representing the box cast area
        Gizmos.DrawWireCube(boxCastCenter, boxSize);
    }




}
