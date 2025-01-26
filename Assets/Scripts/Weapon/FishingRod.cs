using Unity.VisualScripting;
using UnityEngine;


public class FishingRod : BaseWeapon
{
    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private GameObject rodHead;

    [SerializeField]
    private GameObject bobber;

    [SerializeField]
    private AudioSource[] audioNibbles;
    
    [SerializeField]
    private AudioSource rodOn;

    private float bobbingCycle = 0;
    private int bobbingNibble = 0;

    public int lineSegmentCount;

    private bool fishBite = false;

    private bool isCasted;

    public override void OnPrimaryAttack() { }
    public override void OnEquip() { 
        rodOn.Play();
    }
    public override void OnUnequip() { }
    public override void OnSecondaryAttack() { }

    public void Start(){
        bobber.transform.parent = null;
        line.transform.parent = null;
    }

    public void FixedUpdate(){
        if (isCasted){
            CreateLine();
            bobbingCycle += Time.fixedDeltaTime * (fishBite? 10 : 2) * Mathf.PI / 2;
            bobber.transform.position += new Vector3(0, Mathf.Sin(bobbingCycle) * 0.1f * Time.fixedDeltaTime * (fishBite? 10 : 2), 0);
            if(bobbingNibble < Mathf.Round(bobbingCycle / Mathf.PI / 2)){
                audioNibbles[Random.Range(0, audioNibbles.Length)].Play();
                bobbingNibble += 1;
            }
        }
    }

    public void CastLine(FishingSpot fishingSpot){
        isCasted = true;
        bobbingCycle = 0;
        bobbingNibble = 0;
        bobber.gameObject.SetActive(true);
        bobber.transform.position = fishingSpot.transform.position + new Vector3(0, 0.03f, 0);
        line.gameObject.SetActive(true);
    }

    public void ClearLine(){
        isCasted = false;
        fishBite = false;
        bobber.gameObject.SetActive(false);
        line.gameObject.SetActive(false);
    }

    void CreateLine(){
        Vector3[] linePositions = new Vector3[lineSegmentCount + 1];
        for(int i = 0; i < lineSegmentCount + 1; i++){
            float t = (float)i / lineSegmentCount;
            linePositions[i] = Vector3.Lerp(rodHead.transform.position, bobber.transform.position, t);
            linePositions[i].y = t * t + rodHead.transform.position.y * (1 - t) +  t * (bobber.transform.position.y - 1);
            //linePositions[i] -= line.transform.position;
        }
        line.SetPositions(linePositions);
    }

    public void FishBite(){
        fishBite = true;
    }
}