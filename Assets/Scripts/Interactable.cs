using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string interactionText;

    //Method that each interactable will overwrite and call when the player interacts 
    public abstract void OnInteract();

    //Method called when the raycast hits the interactable 
    public abstract void OnFocus();

    //Gets called when we are no longer looking at the interactable
    public abstract void OnLoseFocus();
}
