using UnityEngine;

public interface IInteractable
{
    // When the player look at the object
    void OnHighlight();

    // When the player look away
    void OnDeHighlight();

    // When the interact button is pressed
    void OnInteract();

    
}
