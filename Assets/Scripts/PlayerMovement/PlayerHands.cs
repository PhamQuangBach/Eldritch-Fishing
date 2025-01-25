using UnityEngine;


public class PlayerHands : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float rotationAmplitude = 5f;

    private Vector3 currentRotation = Vector3.zero;


    private void Update()
    {
        HandsRotation();
        FallHandRotation();

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, rotationSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }


    /// <summary>
    /// Ratates hands based on fall velocity
    /// </summary>
    private void FallHandRotation()
    {
        if (playerMovement.IsGrounded)
            return;

        Debug.Log(playerMovement.Velocity);

        float xRotation = playerMovement.Velocity.y * 0.1f;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        Vector3 rotation = new Vector3(xRotation, 0, 0);

        currentRotation += rotation;
    }

    /// <summary>
    /// Rotate hands based on mouse input
    /// </summary>
    private void HandsRotation()
    {
        Vector2 mouseInput = playerMovement.MouseInput;
        if (mouseInput.magnitude == 0)
            return;

        Vector3 rotation = new Vector3(mouseInput.y, -mouseInput.x, 0);

        currentRotation += rotation * rotationAmplitude;
    }
}