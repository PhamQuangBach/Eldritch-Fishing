using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject flashLight;

    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private Image playerReticle;

    [SerializeField]
    private Sprite[] reticleImages;

    private IInteractable currentInteractableObject;

    private int toolState = 0;
    private bool atFishingSpot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Switch Tool")){
            toolState = (toolState + 1) % 2;

            if (toolState == 0){
                flashLight.SetActive(false);
            }
            if (toolState == 1){
                flashLight.SetActive(true);
            }
        }

        if (Input.GetButtonDown("Fire1") && currentInteractableObject != null){
            currentInteractableObject.OnInteract();
        }
    }

    void FixedUpdate(){
        IInteractable newInteractableObject = CheckInteractable();

        if (!ReferenceEquals(newInteractableObject, currentInteractableObject)){
            if (currentInteractableObject != null){
               currentInteractableObject.OnDeHighlight(); 
            }
            if (newInteractableObject != null){
                newInteractableObject.OnHighlight();
            }
        }
        currentInteractableObject = newInteractableObject;
    }

    IInteractable CheckInteractable(){
        Vector3 forwardDirection = mainCamera.transform.forward;
        RaycastHit hit;
        if  (Physics.Raycast(mainCamera.transform.position, forwardDirection, out hit, 10)){
            return hit.collider.gameObject.GetComponent<IInteractable>();
        }
        else{
            return null;
        }
    }

}
