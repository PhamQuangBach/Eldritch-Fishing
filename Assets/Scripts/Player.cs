using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject flashLight;

    [SerializeField]
    private GameObject mainCamera;

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
        Vector3 forwardDirection = mainCamera.transform.forward;
        RaycastHit hit;
        if  (Physics.Raycast(mainCamera.transform.position, forwardDirection, out hit, 10, LayerMask.GetMask("FishingSpot"))){
            Debug.Log("akfhoseihfowegfbjowebfvoew");
        }
    }
}
