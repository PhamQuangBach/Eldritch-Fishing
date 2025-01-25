using UnityEngine;

public abstract class BaseInteractble : MonoBehaviour, IInteractable
{
    public Sprite reticleSprite;
    public string objectName;

    public abstract void OnDeHighlight();

    public abstract void OnHighlight();

    public abstract void OnInteract();
}
