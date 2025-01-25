using UnityEngine;

public class FishingSpot : BaseInteractble
{

    [SerializeField]
    private ParticleSystem bubbles;

    [SerializeField]
    private ParticleSystem waterRing;
    
    [SerializeField]
    private ParticleSystem waterRingCrazy;

    private float timer;

    private int state = 0;

    private FishingRod fishingRod;

    [SerializeField]
    private Sprite rodIcon;

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
        if (state == 0){
            StartFishing();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbles.Stop();
        waterRing.Stop();
        waterRingCrazy.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (state != 0){
            if (timer <= 0){
                if (state == 1){
                    StartFishBite();
                }
                else if (state == 2){
                    EndFishBite();
                }
            }
        }
        else{
            if (timer <= 0){
                objectName = "Fishing spot";
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
        bubbles.Play();
        waterRing.Play();
        timer = Random.Range(3f, 10f);
        reticleSprite = null;
        objectName = "";
    }

    void StartFishBite(){
        state = 2;
        timer = Random.Range(2f, 3f);
        fishingRod.FishBite();
        waterRingCrazy.Play();
        reticleSprite = rodIcon;
        objectName = "Reel in";
    }

    void ReelIn(){
        EndFishBite();
        state = -1;
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
        state = 0;
        // Kick player out of fishing
        // return movement
    }
}
