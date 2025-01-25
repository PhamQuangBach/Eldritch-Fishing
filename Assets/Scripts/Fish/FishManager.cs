using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;

    public static int FishCaught { get => instance.fishCaught; }
    private int fishCaught = 0;

    [SerializeField]
    private GameObject[] fishes;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CatchFish(){
        fishCaught += 1;
    }

    public static GameObject GetFishPrefab(int index){
        return instance.fishes[index];
    }
}
