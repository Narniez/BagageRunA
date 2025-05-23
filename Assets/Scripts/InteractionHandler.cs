using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public static InteractionHandler Instance { get; private set; }
    private Interactable currentInteractable;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        // Check for the interact key (e.g., F key)
        if (Input.GetKeyDown(KeyCode.F))
        {
            HandleInteractionInput();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Interacting");
        if (collision.TryGetComponent(out Interactable interactable))
        {
            if (currentInteractable != interactable)
            {
                if (currentInteractable != null)
                {
                    currentInteractable.OnLoseFocus();
                }

                currentInteractable = interactable;
                currentInteractable.OnFocus();

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentInteractable != null && collision.GetComponent<Interactable>() == currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnInteract();
        }
    }
}
