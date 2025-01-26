using UnityEngine;


public enum PlayerState
{
    None,
    Frozen,
    Dead
}

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
            return realVelocity.magnitude > playerSpeed / 2;
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

    public bool IsAlive
    {
        get
        {
            return currentPlayerState != PlayerState.Dead;
        }
    }

    public ScreenFade ScreenFade => screenFade;

    public static PlayerMovement instance;

    [Header("Movement")]
    
    [SerializeField]
    private float playerSpeed = 5f;
    
    [SerializeField]
    private float floorRateSpeed = 1f;
    [SerializeField]
    private float airRateSpeed = 0.5f;

    [SerializeField]
    private PlayerFootSteps playerFootSteps;

    [SerializeField]
    private GameObject playerFeet;

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

    [Header("UI")]
    [SerializeField]
    private ScreenFade screenFade;

    [SerializeField]
    private PlayerDeathScreen deathScreen;

    private PlayerState currentPlayerState = PlayerState.None;

    private CharacterController playerController;
    private float gravity;

    private Vector3 gravityVelocity = new Vector3();

    private float xRotation = 0f;
    private float currentRateSpeed;
    private Vector3 currentVelocity = Vector3.zero;

    private Vector3 realVelocity;

    private Vector3 positionCameraPos = Vector3.zero;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        instance = this;

        deathScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.IsPaused)
            return;

        switch (currentPlayerState)
        {
            case PlayerState.None:
                Movement();
                break;
            case PlayerState.Frozen:
                break;
            case PlayerState.Dead:
                DeathMovement();
                break;
        }

        if (currentPlayerState != PlayerState.Dead)
            CameraRotation();
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

        if (!IsGrounded)
        {
            gravityVelocity.y += gravity * Time.deltaTime;
            currentRateSpeed = airRateSpeed;
        } 
        else
        {
            currentRateSpeed = floorRateSpeed;
            gravityVelocity = Vector3.zero;
        }

        currentVelocity = Vector3.Lerp(currentVelocity, wishVelocity, currentRateSpeed * Time.deltaTime);

        playerController.Move(currentVelocity * playerSpeed * Time.deltaTime);
        realVelocity = playerController.velocity;
        playerController.Move(gravityVelocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        Vector2 mouseInput = MouseInput;

        xRotation -= mouseInput.y * cameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);
        
        transform.Rotate(Vector3.up * mouseInput.x * cameraSensitivity);
        playerHead.transform.localEulerAngles = Vector3.right * xRotation;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 normal = hit.normal;

        if (hit.gameObject.layer == groundLayer.value)
            return;

        // Checking if the player is colliding with a wall
        if (normal.y == 0)
        {
            // Removing the velocity in the direction of the wall
            currentVelocity -= normal * Vector3.Dot(currentVelocity, normal);
        }
    }

    private void DeathMovement()
    {
        Vector3 cameraPos = playerCamera.transform.position;

        playerCamera.transform.position = Vector3.Lerp(cameraPos, positionCameraPos, Time.deltaTime);
    }

    public void ChangeFootSteps(FootStepsCollection collection)
    {
        playerFootSteps.SetFootStepCollection(collection);
    } 

    public void ResetFootSteps()
    {
        playerFootSteps.ResetFootStepCollection();
    }

    public void Kill(Vector3 moveCameraTo)
    {
        currentPlayerState = PlayerState.Dead;
        playerController.enabled = false;

        positionCameraPos = moveCameraTo;

        deathScreen.gameObject.SetActive(true);
        deathScreen.StartGameOver();
        screenFade.FadeIn();
    }

    public void KillTimeOut()
    {
        Kill(transform.position);
    }
}
