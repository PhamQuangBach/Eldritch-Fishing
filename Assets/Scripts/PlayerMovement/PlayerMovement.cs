using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float playerSpeed = 5f;
    
    [SerializeField]
    private GameObject playerHead;
    [SerializeField]
    private float cameraSensitivity = 100f;

    [SerializeField]
    private Camera playerCamera;
    
    private CharacterController playerController;
    private float gravity;

    private float xRotation = 0f;

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
        HeadRotation();
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
        Vector3 velocity = BuildWishVelocity() * playerSpeed;

        playerController.Move(velocity * Time.deltaTime);
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
}
