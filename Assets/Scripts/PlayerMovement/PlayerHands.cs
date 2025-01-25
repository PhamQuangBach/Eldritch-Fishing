using System.Collections.Generic;
using UnityEngine;


public class PlayerHands : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float rotationAmplitude = 5f;

    [Header("Inventory")]
    [SerializeField]
    private List<BaseWeapon> weapons = new();

    private BaseWeapon currentWeapon;
    

    private Vector3 currentRotation = Vector3.zero;

    private void Start()
    {
        currentWeapon = weapons[0];
        currentWeapon.OnEquipInternal();
    }


    private void Update()
    {
        HandsRotation();
        FallHandRotation();
        GetAttacks();

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, rotationSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }


    /// <summary>
    /// Getting mouse buttons input and calling curreunt's weapon attacks
    /// </summary>
    private void GetAttacks()
    {
        if (Input.GetMouseButtonDown(0))
            currentWeapon.OnPrimaryAttack();

        if (Input.GetMouseButtonDown(1))
            currentWeapon.OnSecondaryAttack();
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