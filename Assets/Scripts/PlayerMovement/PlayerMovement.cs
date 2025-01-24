using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public Vector3 Velocity
    {
        get
        {
            return realVelocity;
        }
    }

    public bool IsMoving
    {
        get
        {
            return playerController.velocity.magnitude > playerSpeed / 2;
        }
    }

    public bool IsGrounded
    {
        get
        {
            Vector3 origin = playerFeet.transform.position;
            Vector3 dir = Vector3.down * groundCheckDistance;

            Debug.DrawRay(origin, dir, Color.red);

            return Physics.Raycast(origin, dir, groundCheckDistance, groundLayer);
        }
    }

    public Vector3 InputDirection
    {
        get
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            return new Vector3(horizontal, 0, vertical);
        }
    }

    public Vector2 MouseInput
    {
        get
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            return new Vector2(mouseX, mouseY);
        }
    }

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
    private Camera playerCamera;

    [SerializeField]
    private float cameraSensitivity = 100f;
    
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private float groundCheckDistance = 0.4f;

    [Header("Slope")]
    [SerializeField]
    private GameObject playerFeet;

    [SerializeField]
    private float slopeDistance = 5f;

    private CharacterController playerController;
    private float gravity;

    private float xRotation = 0f;
    private float currentRateSpeed;
    private Vector3 currentVelocity = Vector3.zero;

    private Vector3 realVelocity;

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

        Movement();
        CameraRotation();
        PlayerSlope();
    }

    private Vector3 BuildWishVelocity()
    {
        Vector3 wishVelocity = transform.forward * InputDirection.z + transform.right * InputDirection.x;
        wishVelocity.Normalize();

        return wishVelocity;
    }

    private void Movement()
    {
        Vector3 wishVelocity = BuildWishVelocity();

        Vector3 velocity = playerController.velocity;

        if (!IsGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;

            playerController.SimpleMove(velocity);

            realVelocity = playerController.velocity;

            currentRateSpeed = airRateSpeed;
        } 
        else
        {
            currentRateSpeed = floorRateSpeed;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, wishVelocity, currentRateSpeed * Time.deltaTime);

        playerController.Move(currentVelocity * playerSpeed * Time.deltaTime);
    }

    private void CameraRotation()
    {
        Vector2 mouseInput = MouseInput;

        xRotation -= mouseInput.y * cameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);
        
        transform.Rotate(Vector3.up * mouseInput.x * cameraSensitivity);
        playerHead.transform.localEulerAngles = Vector3.right * xRotation;
    }

    private void PlayerSlope()
    {
        RaycastHit hit;

        if (!Physics.Raycast(playerFeet.transform.position, Vector3.down, out hit, slopeDistance, groundLayer))
            return;

        Vector3 normal = hit.normal;

        if (normal.y < 0.9f)
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, normal).normalized;
            Vector3 slopeVelocity = slopeDirection * playerSpeed * Time.deltaTime;
            Vector3 velocity = playerController.velocity;
            velocity += slopeVelocity;

            playerController.SimpleMove(velocity);
        }
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
