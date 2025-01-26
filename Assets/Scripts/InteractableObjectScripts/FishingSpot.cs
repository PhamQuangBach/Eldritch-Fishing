using UnityEngine;

public class FishingSpot : BaseInteractble
{
    private enum FishingState{
        Idle,
        Casting,
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

    [SerializeField]
    private AudioSource audioLineBreak;

    [SerializeField]
    private AudioSource audioCatch;

    [SerializeField]
    private AudioSource audioLure;

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
        
    }

    void Update(){
        if (GameManager.IsPaused)
            return;

        timer -= Time.deltaTime;
        if (state != FishingState.Idle){
            if (timer <= 0){
                if (state == FishingState.Casting){
                    timer = Random.Range(3f, 10f);
                    state = FishingState.Waiting;
                }
                else if (state == FishingState.Waiting){
                    StartFishBite();
                }
                else if (state == FishingState.Biten){
                    EndFishBite();
                }
                else if (state == FishingState.Destroyed){
                    Destroy(gameObject);
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

        if (state == FishingState.Biten){
            if (Input.GetMouseButtonDown(0)){
                ReelIn();
            }
        }
        if (state == FishingState.Waiting){
            if (Input.GetMouseButtonDown(0)){
                EndFishBite();
            }
        }
    }

    void StartFishing(){
        state = FishingState.Casting;
        fishingRod.CastLine(this);
        waterRing.Play();
        audioLure.Play();
        timer = 1;
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
        fishingRod.ClearLine();
        reticleSprite = rodIcon;
        objectName = "Fishing spot";
        
        bubbles.Stop();
        waterRing.Stop();
        waterRingCrazy.Stop();
        
        audioCatch.Play();

        state = FishingState.Destroyed;
        timer = 1;
        FishManager.instance.CatchFish();
    }

    void EndFishBite(){
        fishingRod.ClearLine();
        reticleSprite = rodIcon;
        objectName = "Fishing spot";
        
        bubbles.Stop();
        waterRing.Stop();
        waterRingCrazy.Stop();
        
        audioLineBreak.Play();
        state = FishingState.Idle;
    }
}
