using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject flashLight;

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
    }
}
