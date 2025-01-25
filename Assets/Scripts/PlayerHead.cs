using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour
{

    [SerializeField]
    private Image playerReticle;

    [SerializeField]
    private Text reticleDescription;

    [SerializeField]
    private Sprite baseReticle;

    private BaseInteractble currentInteractableObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(BaseWeapon weapon){
        if (currentInteractableObject != null){
            currentInteractableObject.OnInteract(weapon);
        }
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
