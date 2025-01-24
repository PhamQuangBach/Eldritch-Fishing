using UnityEngine;
using UnityEngine.Rendering;


public class PlayerHeadBobbing : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField]
    private float bobbingApmlitude = 1f;

    [SerializeField]
    private float bobbingSpeed = 1f;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Camera playerCamera;

    [Header("Head Rotation")]
    [SerializeField]
    private float headRotationSpeed = 10f;
    
    [SerializeField]
    private float headRotation = 2f;

    private float currentRotation = 0f;

    private float curTime = 0;

    private Vector3 rotateCameraTo = Vector3.zero;

    private void Update()
    {
        Bobbing();
        HeadRotation();

        Quaternion actualRotation = playerCamera.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(rotateCameraTo);
        playerCamera.transform.localRotation = Quaternion.Slerp(actualRotation, targetRotation, bobbingSpeed * .5f * Time.deltaTime);
    }

    private void Bobbing()
    {
        if (!(playerMovement.IsMoving && playerMovement.IsGrounded))
        {
            rotateCameraTo = Vector3.zero;

            return;
        }

        Vector3 rotation = new Vector3(
            Mathf.Sin(curTime * bobbingSpeed) * bobbingApmlitude, 
            Mathf.Cos(curTime * bobbingSpeed / 2) * bobbingApmlitude,
            0
        );

        rotateCameraTo = rotation;

        curTime += Time.deltaTime;
    }

    private void HeadRotation()
    {
        if (!(playerMovement.IsMoving && playerMovement.IsGrounded))
        {
            currentRotation = Mathf.Lerp(currentRotation, 0, headRotationSpeed * Time.deltaTime);

            return;
        }

        Vector3 inputDir = playerMovement.InputDirection;

        currentRotation = Mathf.Lerp(currentRotation, -inputDir.x * headRotation, headRotationSpeed * Time.deltaTime);

        Vector3 cameraRot = playerCamera.transform.localEulerAngles;

        playerCamera.transform.localEulerAngles = new Vector3(
            cameraRot.x,
            cameraRot.y,
            currentRotation
        );
    }
}