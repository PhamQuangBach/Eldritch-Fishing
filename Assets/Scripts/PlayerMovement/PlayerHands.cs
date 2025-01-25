using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHands : MonoBehaviour
{
    [Header("Weapon sway")]
    [SerializeField]
    private PlayerMovement playerMovement;
    
    [Header("Player head")]
    [SerializeField]
    private PlayerHead playerHead;

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
        HandsRotation();
        FallHandRotation();
        GetAttacks();
        ChangeWeapon();

        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, rotationSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    void FixedUpdate(){
        BaseInteractble newInteractableObject = CheckInteractable();

        if (!ReferenceEquals(newInteractableObject, currentInteractableObject)){
            if (currentInteractableObject != null){
               currentInteractableObject.OnDeHighlight(); 
            }

            if (newInteractableObject != null){
                newInteractableObject.OnHighlight();
                playerReticle.sprite = newInteractableObject.reticleSprite;
                reticleDescription.text = newInteractableObject.objectName;
            }
            else{
                playerReticle.sprite = baseReticle;
                reticleDescription.text = "";
            }
        }
        currentInteractableObject = newInteractableObject;
        if (currentInteractableObject != null){
            if (currentInteractableObject.reticleSprite != null){
                playerReticle.sprite = currentInteractableObject.reticleSprite;
            }
            else{
                playerReticle.sprite = baseReticle;
            }
            reticleDescription.text = currentInteractableObject.objectName;
        }
        else{
            playerReticle.sprite = baseReticle;
            reticleDescription.text = "";
        }
    }

    /// <summary>
    /// Getting mouse buttons input and calling curreunt's weapon attacks
    /// </summary>
    private void GetAttacks()
    {
        if (Input.GetMouseButtonDown(0)){
            currentWeapon.OnPrimaryAttack();
        }

        if (Input.GetMouseButtonDown(1))
            currentWeapon.OnSecondaryAttack();
    }

    /// <summary>
    /// Changing weapon when player presses Tab button
    /// </summary>
    private void ChangeWeapon()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
            return;

        currentWeaponSlot++;

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

    

    BaseInteractble CheckInteractable(){
        Vector3 forwardDirection = transform.forward;
        RaycastHit hit;
        if  (Physics.Raycast(transform.position + forwardDirection * 0.5f, forwardDirection, out hit, 10)){
            return hit.collider.gameObject.GetComponent<BaseInteractble>();
        }
        else{
            return null;
        }
    }

}