using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorWithInventory : XRSocketInteractor
{
    public string prefabName;

    private PlayerInventory playerInventory;

    protected override void Awake()
    {
        base.Awake();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Check if the selected object is a Grabbable Interactable.
        if (args.interactable is XRGrabInteractable grabInteractable)
        {
            // Check if the attach point is empty.
            if (selectTarget == null)
            {
                // Attach the selected object to the attach point.
                grabInteractable.transform.position = attachTransform.position;
                grabInteractable.transform.rotation = attachTransform.rotation;
                grabInteractable.transform.SetParent(attachTransform);

                // Add the attach point and prefab to the inventory.
                int attachPointIndex = transform.GetSiblingIndex();
                playerInventory.AddItem(attachPointIndex, prefabName);
            }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Check if the unselected object is a Grabbable Interactable.
        if (args.interactable is XRGrabInteractable grabInteractable)
        {
            // Check if the unselected object is currently attached to the attach point.
            if (grabInteractable.transform.parent == attachTransform)
            {
                // Detach the object from the attach point.
                grabInteractable.transform.SetParent(null);

                // Remove the attach point and prefab from the inventory.
                int attachPointIndex = transform.GetSiblingIndex();
                playerInventory.RemoveItem(attachPointIndex);
            }
        }
    }
}
