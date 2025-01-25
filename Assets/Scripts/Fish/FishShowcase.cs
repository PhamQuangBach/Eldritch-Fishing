using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class FishShowCase : MonoBehaviour
{
    [SerializeField]
    private GameObject showcaseCanvas;

    [SerializeField]
    private TMP_Text fishName;

    [SerializeField]
    private TMP_Text fishDescription;

    private GameObject fishObject;

    private float timer = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fishObject!= null){
            fishObject.transform.Rotate(new Vector3(0, Time.deltaTime * 10, 0));
            timer -= Time.deltaTime;
            if (timer < 0 && Input.GetMouseButtonDown(0)){
                HideFish();
            }
        }
    }

    public void ShowFish(GameObject fish){
        showcaseCanvas.SetActive(true);
        fishObject = Instantiate(fish, transform);
        fishObject.transform.localScale = Vector3.one * 0.08f;
        Fish currentFish  = fishObject.GetComponent<Fish>();
        fishName.text = currentFish.fishName;
        fishDescription.text = currentFish.fishDescription;
        timer = 1;
    }

    public void HideFish(){
        showcaseCanvas.SetActive(false);
        Destroy(fishObject);
    }
}
