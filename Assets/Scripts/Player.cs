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

    private int toolState = 0;

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



        //if (Input.GetKeyDown())
    }

    void FixedUpdate(){
        GameObject fishingSpot = CheckFishingSpot();
        if  (fishingSpot != null){
            playerReticle.sprite = reticleImages[1];
        }
        else{
            playerReticle.sprite = reticleImages[0];
        }
    }

    GameObject CheckFishingSpot(){
        Vector3 forwardDirection = mainCamera.transform.forward;
        RaycastHit hit;
        if  (Physics.Raycast(mainCamera.transform.position, forwardDirection, out hit, 10, LayerMask.GetMask("FishingSpot"))){
            return hit.collider.gameObject;
        }
        else{
            return null;
        }
    }
}
