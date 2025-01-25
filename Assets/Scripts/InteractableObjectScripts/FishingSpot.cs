using UnityEngine;

public class FishingSpot : BaseInteractble
{
    [SerializeField]
    private GameObject fish;

    [SerializeField]
    private ParticleSystem bubbles;

    private float timer;

    private int state = 0;

    private FishingRod fishingRod;

    [SerializeField]
    private Sprite rodIcon;

    public override void OnDeHighlight()
    {
        Debug.Log("DeHighligh Fishing Spot");
    }

    public override void OnHighlight()
    {
        Debug.Log("Highligh Fishing Spot");
    }

    public override void OnInteract(BaseWeapon weapon)
    {
        if (weapon is not FishingRod)
        {
            return;
        }
        fishingRod = weapon as FishingRod;
        // Freeze movement
        if (state == 0){
            StartFishing();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state != 0){
            timer -= Time.fixedDeltaTime;
            if (timer <= 0){
                if (state == 1){
                    StartFishBite();
                }
                else if (state == 2){
                    EndFishBite();
                }
            }

            
        }
    }

    void Update(){
        if (state == 2){
            if (Input.GetMouseButtonDown(0)){
                ReelIn();
            }
        }
    }

    void StartFishing(){
        state = 1;
        fishingRod.CastLine(this);
        bubbles.gameObject.SetActive(true);
        timer = Random.Range(3f, 10f);
        reticleSprite = null;
        objectName = "";
    }

    void StartFishBite(){
        state = 2;
        timer = Random.Range(2f, 3f);
        fishingRod.FishBite();
        reticleSprite = rodIcon;
        objectName = "Reel in";
    }

    void ReelIn(){
        EndFishBite();
        FishManager.instance.CatchFish();
        Destroy(gameObject);
    }

    void EndFishBite(){
        state = 0;
        fishingRod.ClearLine();
        reticleSprite = rodIcon;
        objectName = "Fishing spot";
        // Kick player out of fishing
        // return movement
    }
}
