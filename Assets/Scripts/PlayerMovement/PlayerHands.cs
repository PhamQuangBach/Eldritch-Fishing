using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHands : MonoBehaviour
{
    [Header("Weapon sway")]
    [SerializeField]
    private PlayerMovement playerMovement;
    
    [Header("Player camera")]
    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float rotationAmplitude = 5f;

    [Header("Inventory")]
    [SerializeField]
    private List<BaseWeapon> weapons = new();

    [SerializeField]
    private Image playerReticle;

    [SerializeField]
    private Text reticleDescription;

    [SerializeField]
    private Sprite baseReticle;

    [Header("Interaction")]
    [SerializeField]
    private float interactionDistance = 2f;

    [SerializeField]
    private LayerMask interactableLayer;


    private BaseInteractble currentInteractableObject;

    private int currentWeaponSlot = 0;
    private BaseWeapon currentWeapon;

    private Vector3 currentRotation = Vector3.zero;

    private void Start()
    {
        currentWeapon = weapons[currentWeaponSlot];
        currentWeapon.OnEquipInternal();
    }

    private void Update()
    {
        if (GameManager.IsPaused)
            return;

        HandsRotation();
        FallHandRotation();
        GetAttacks();
        ChangeWeapon();
        SearchForInteractable();
        Interact();

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    /// <summary>
    /// Getting mouse buttons input and calling curreunt's weapon attacks
    /// </summary>
    private void GetAttacks()
    {
        if (Input.GetMouseButtonDown(0)){
            currentWeapon.OnPrimaryAttack();
        }
       //Hope this doesnt break anything lol - Martin
       // SEEMS FINE
       // if (Input.GetMouseButtonDown(1))
           // currentWeapon.OnSecondaryAttack();
    }

    /// <summary>
    /// Changing weapon when player presses Tab button
    /// </summary>
    private void ChangeWeapon()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse1))
            return;

        currentWeaponSlot++;

        if (currentWeapon is FishingRod fishingRod)
        {
            fishingRod.ClearLine();
        }

        if (currentWeaponSlot >= weapons.Count)
            currentWeaponSlot = 0;

        BaseWeapon newWeapon = weapons[currentWeaponSlot];

        if (currentWeapon == newWeapon)
            return;

        currentWeapon.OnUnequipInternal();
        currentWeapon = newWeapon;
        currentWeapon.OnEquipInternal();
    } 

    /// <summary>
    /// Ratates hands based on fall velocity
    /// </summary>
    private void FallHandRotation()
    {
        if (playerMovement.IsGrounded)
            return;

        //Debug.Log(playerMovement.Velocity);

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

    /// <summary>
    /// Interact with interactable objects
    /// </summary>
    private void Interact()
    {
        if (!Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButtonDown(0))
            return;

        RaycastHit hit;

        if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactableLayer))
            return;

        if (hit.collider.tag != "Interactable")
            return;

        BaseInteractble interactable = hit.collider.GetComponent<BaseInteractble>();
        interactable.OnInteract(currentWeapon);
    }

    /// <summary>
    /// Search for interactable objects and changing the reticle based on the object
    /// </summary>
    private void SearchForInteractable()
    {
        RaycastHit hit;

        if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactableLayer))
        {
            playerReticle.sprite = baseReticle;
            reticleDescription.text = "";

            return;
        }
        Debug.Log(hit.collider.tag);
        
        if (hit.collider.tag != "Interactable")
        {
            playerReticle.sprite = baseReticle;
            reticleDescription.text = "";

            return;
        }

        BaseInteractble interactable = hit.collider.GetComponent<BaseInteractble>();

        if (interactable.reticleSprite == null)
        {
            playerReticle.sprite = baseReticle;
        }
        
        reticleDescription.text = interactable.objectName;
    }
}