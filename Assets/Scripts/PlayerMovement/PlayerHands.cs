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

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, rotationSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    private void HandsRotation()
    {
        Vector2 mouseInput = playerMovement.MouseInput;
        if (mouseInput.magnitude == 0)
            return;

        Vector3 rotation = new Vector3(mouseInput.y, -mouseInput.x, 0);

        currentRotation += rotation * rotationAmplitude;
    }
}