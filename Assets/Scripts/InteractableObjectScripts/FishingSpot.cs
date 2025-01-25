using UnityEngine;

public class FishingSpot : BaseInteractble
{
    private enum FishingState{
        Idle,
        Waiting,
        Biten,
        Destroyed,
    }

    [SerializeField]
    private ParticleSystem bubbles;

    [SerializeField]
    private ParticleSystem waterRing;
    
    [SerializeField]
    private ParticleSystem waterRingCrazy;

    private float timer;

    private FishingState state = FishingState.Idle;

    private FishingRod fishingRod;

    [SerializeField]
    private Sprite rodIcon;

    [SerializeField]
    private float breakDistance;

    private Transform playerCamera;

    public override void OnDeHighlight()
    {

    }

    public override void OnHighlight()
    {

    }

    public override void OnInteract(BaseWeapon weapon)
    {
        if (weapon is not FishingRod)
        {
            objectName = "You can't fish with this";
            timer = 1;
            return;
        }
        fishingRod = weapon as FishingRod;
        // Freeze movement
        if (state == FishingState.Idle){
            StartFishing();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = Camera.main.transform;
        bubbles.Stop();
        waterRing.Stop();
        waterRingCrazy.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.IsPaused)
            return;

        timer -= Time.fixedDeltaTime;
        if (state != FishingState.Idle){
            if (timer <= 0){
                if (state == FishingState.Waiting){
                    StartFishBite();
                }
                else if (state == FishingState.Biten){
                    EndFishBite();
                }
            }
            
            if ((playerCamera.position - transform.position).sqrMagnitude > breakDistance * breakDistance){
                EndFishBite();
            }
        }
        else{
            if (timer <= 0){
                objectName = "Fishing spot";
            }
        }
    }

    void Update(){
        if (state == FishingState.Biten){
            if (Input.GetMouseButtonDown(0)){
                ReelIn();
            }
        }
    }

    void StartFishing(){
        state = FishingState.Waiting;
        fishingRod.CastLine(this);
        waterRing.Play();
        timer = Random.Range(3f, 10f);
        reticleSprite = null;
        objectName = "";
    }

    void StartFishBite(){
        state = FishingState.Biten;
        timer = Random.Range(2f, 3f);
        fishingRod.FishBite();
        bubbles.Play();
        waterRingCrazy.Play();
        reticleSprite = rodIcon;
        objectName = "Reel in";
    }

    void ReelIn(){
        EndFishBite();
        state = FishingState.Destroyed;
        FishManager.instance.CatchFish();
        Destroy(gameObject);
    }

    void EndFishBite(){
        fishingRod.ClearLine();
        reticleSprite = rodIcon;
        objectName = "Fishing spot";
        
        bubbles.Stop();
        waterRing.Stop();
        waterRingCrazy.Stop();
        state = FishingState.Idle;
        // Kick player out of fishing
        // return movement
    }
}
