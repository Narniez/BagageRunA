using System.Collections;
using UnityEngine;

public class LosePackageInteractable : Interactable
{
    [SerializeField] PackageHandler packageHandler;
    [SerializeField] PlayerController playerController;

    public GameObject interactionUI;

    public Sprite newSprite;

    public Transform UIPosition;
    private Vector3 initialUIPosition;
    private bool isHovering = false;

    private bool hasInteracted = false;

    public bool shouldChangeSprite = false;
    public override void OnFocus()
    {

        if (interactionUI != null && !hasInteracted)
        {
            interactionUI.SetActive(true);
            StartCoroutine(HoverUI());
        }
    }

    public override void OnLoseFocus()
    {

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
            StopAllCoroutines();
            interactionUI.transform.position = initialUIPosition;
        }
    }

    public override void OnInteract()
    {
        if (interactionUI != null && !hasInteracted)
        {
            interactionUI.SetActive(false);
        }
        packageHandler.PopLastPackage();
        playerController.StopMovementForOneSecond();

        if (shouldChangeSprite)
        {
            ChangeSprite();
        }
        hasInteracted = true;
    }


    private IEnumerator HoverUI()
    {
        isHovering = true;
        float hoverSpeed = 1f;
        // float hoverHeight = 0.1f;

        while (isHovering)
        {

            float newY = initialUIPosition.y + Mathf.Sin(Time.time * hoverSpeed);
            interactionUI.transform.position = new Vector3(initialUIPosition.x, newY, initialUIPosition.z);

            yield return null;
        }
    }

    private void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
