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

    public override void OnInteract()
    {
        Debug.Log("Interact Fishing Spot");
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
