using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    
    [SerializeField]
    private float playerSpeed = 5f;
    
    [SerializeField]
    private float floorRateSpeed = 1f;
    [SerializeField]
    private float airRateSpeed = 0.5f;

    [Header("Head Rotation")]
    [SerializeField]
    private GameObject playerHead;
    
    [SerializeField]
    private float cameraSensitivity = 100f;
    
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float groundCheckDistance = 0.4f;

    [SerializeField]
    private Camera playerCamera;
    
    private CharacterController playerController;
    private float gravity;

    private float xRotation = 0f;
    private float currentRateSpeed;
    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Cursor.visible)
            return;

        Debug.DrawRay(transform.position, currentVelocity);

        Movement();
        HeadRotation();
    }

    private bool isGrounded()
    {
        Vector3 origin = transform.position - Vector3.down * groundCheckDistance * 0.5f;
        Vector3 dir = Vector3.down * groundCheckDistance;

        Debug.DrawRay(origin, dir, Color.red);

        return Physics.Raycast(origin, dir, groundCheckDistance, groundLayer);
    }

    private Vector3 BuildWishVelocity()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 wishVelocity = transform.forward * vertical + transform.right * horizontal;
        wishVelocity.Normalize();

        return wishVelocity;
    }

    private void Movement()
    {
        Vector3 wishVelocity = BuildWishVelocity();

        Vector3 velocity = playerController.velocity;

        if (!isGrounded())
        {
            velocity.y -= gravity * Time.deltaTime;

            playerController.SimpleMove(velocity);

            currentRateSpeed = airRateSpeed;
        } 
        else
        {
            currentRateSpeed = floorRateSpeed;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, wishVelocity, currentRateSpeed * Time.deltaTime);

        playerController.Move(currentVelocity * playerSpeed * Time.deltaTime);
    }

    private void HeadRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY * cameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);
        
        transform.Rotate(Vector3.up * mouseX * cameraSensitivity);
        playerCamera.transform.localEulerAngles = Vector3.right * xRotation;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 normal = hit.normal;

        // Checking if the player is colliding with a wall
        if (normal.y == 0)
        {
            // Removing the velocity in the direction of the wall
            currentVelocity -= normal * Vector3.Dot(currentVelocity, normal);
        }
    }
}
