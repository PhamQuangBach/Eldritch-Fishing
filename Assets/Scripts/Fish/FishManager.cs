using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;

    private int fishCaught = 0;

    [SerializeField]
    private GameObject[] fishes;

    [SerializeField]
    private FishShowCase fishShowCase;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FishCaught(){
        fishShowCase.ShowFish(fishes[fishCaught]);
        fishCaught += 1;
    }
}
