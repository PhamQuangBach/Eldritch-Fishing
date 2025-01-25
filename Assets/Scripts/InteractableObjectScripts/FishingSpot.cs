using UnityEngine;

public class FishingSpot : BaseInteractble
{
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
        Debug.Log("Interact Fishing Spot");
        if (weapon is not FishingRod)
        {
            return;
        }
        // Freeze movement
        (weapon as FishingRod).CastLine(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
