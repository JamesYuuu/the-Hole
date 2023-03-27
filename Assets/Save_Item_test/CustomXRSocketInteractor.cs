using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class CustomXRSocketInteractor : XRSocketInteractor
{
    private Dictionary<string, GameObject> snappedItems = new Dictionary<string, GameObject>(); // dictionary of snapped items by attach point name
    private bool isSceneChanging = false; // flag to indicate whether the scene is changing

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);

        // Store the snapped item in the dictionary
        string attachPointName = gameObject.name; // use the name of the GameObject this script is attached to as the attach point name
        if (!snappedItems.ContainsKey(attachPointName))
        {
            snappedItems.Add(attachPointName, interactable.gameObject);
            Debug.Log($"Snapped item '{interactable.gameObject.name}' to attach point '{attachPointName}'");
        }
        else
        {
            snappedItems[attachPointName] = interactable.gameObject;
            Debug.Log($"Updated snapped item '{interactable.gameObject.name}' in attach point '{attachPointName}'");
        }
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        base.OnSelectExited(interactable);

        // Remove the snapped item from the dictionary only if the scene is not changing
        if (!isSceneChanging)
        {
            string attachPointName = gameObject.name; // use the name of the GameObject this script is attached to as the attach point name
            GameObject snappedItem = interactable.gameObject;
            if (snappedItems.ContainsKey(attachPointName) && snappedItem.scene == gameObject.scene)
            {
                snappedItems.Remove(attachPointName);
                Debug.Log($"Removed snapped item from attach point '{attachPointName}'");
            }
        }
    }

    public Dictionary<string, GameObject> GetSnappedItems()
    {
        return snappedItems;
    }

    private void OnDisable()
    {
        isSceneChanging = true;
    }

    private void OnEnable()
    {
        isSceneChanging = false;
    }
}
