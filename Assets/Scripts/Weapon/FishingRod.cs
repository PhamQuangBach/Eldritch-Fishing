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

    private float bobbingCycle = 0;

    public int lineSegmentCount;


    private bool isCasted;

    public override void OnPrimaryAttack() { }
    public override void OnEquip() { }
    public override void OnUnequip() { }
    public override void OnSecondaryAttack() { }

    public void Start(){
        bobber.transform.parent = null;
        line.transform.parent = null;
    }

    public void FixedUpdate(){
        if (isCasted){
            CreateLine();
            bobbingCycle += Time.fixedDeltaTime;
            bobber.transform.position += new Vector3(0, Mathf.Sin(bobbingCycle) * 0.1f * Time.fixedDeltaTime, 0);
        }
    }

    public void CastLine(FishingSpot fishingSpot){
        isCasted = true;
        bobber.gameObject.SetActive(true);
        bobber.transform.position = fishingSpot.transform.position + new Vector3(0, 0.3f, 0);
        line.gameObject.SetActive(true);
    }

    void ClearLine(){
        line.SetPositions(new Vector3[0]);
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
}