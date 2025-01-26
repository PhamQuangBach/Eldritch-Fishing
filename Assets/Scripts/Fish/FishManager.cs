using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;

    public static int FishCaught { get => instance.fishCaught; }
    public int fishGoal = 5;
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

    public void CatchFish()
    {
        fishCaught += 1;

        if (fishCaught == fishGoal)
        {
            Debug.Log("WIN!!!");
            Monster.instance.StopMovement();
            //ACTIVATE WIN instance
        }

    }

    public static GameObject GetFishPrefab(int index){
        return instance.fishes[index];
    }
}
