using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class BodySocketManager : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the XR Rig in the new scene
        GameObject xrRig = GameObject.Find("Player (XR Origin)");
        if (xrRig == null)
        {
            Debug.LogError("Could not find XR Rig in the scene");
            return;
        }

        XRInteractionManager interactionManager = xrRig.GetComponent<XRInteractionManager>();
        if (interactionManager == null)
        {
            Debug.LogError("Could not find XRInteractionManager in the XR Rig");
            return;
        }

        // Attach the snapped objects to the XR Rig
        foreach (CustomXRSocketInteractor socketInteractor in FindObjectsOfType<CustomXRSocketInteractor>())
        {
            Dictionary<string, GameObject> snappedItems = socketInteractor.GetSnappedItems();
            Debug.Log($"Snapped items for '{socketInteractor.gameObject.name}': {string.Join(", ", snappedItems.Keys)}");

            foreach (KeyValuePair<string, GameObject> kvp in snappedItems)
            {
                string socketName = kvp.Key;
                GameObject item = kvp.Value;

                Transform socketTransform = xrRig.transform.Find(socketName);
                if (socketTransform == null)
                {
                    Debug.LogError($"Could not find socket with name '{socketName}'");
                    continue;
                }

                XRBaseInteractable interactable = item.GetComponent<XRBaseInteractable>();
                if (interactable == null)
                {
                    Debug.LogError($"Could not find XRBaseInteractable on item '{item.name}'");
                    continue;
                }

                // Snap the object to the Attach point
                interactionManager.ForceSelect(socketInteractor, interactable);

                // Set the local position and rotation of the snapped object to match the Attach point
                item.transform.SetParent(socketTransform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void AddSnappedItem(string socketName, GameObject item)
    {
        if (!SnappedItemsStore.Instance.SnappedItems.ContainsKey(socketName))
        {
            SnappedItemsStore.Instance.SnappedItems.Add(socketName, item);
            Debug.Log($"Adding new item '{item.name}' to socket '{socketName}'");
        }
        else
        {
            SnappedItemsStore.Instance.SnappedItems[socketName] = item;
            Debug.Log($"Updating item in socket '{socketName}' to '{item.name}'");
        }

        DontDestroyOnLoad(item);
    }

    public void RemoveSnappedItem(string socketName)
    {
        if (!SnappedItemsStore.Instance.SnappedItems.ContainsKey(socketName))
        {
            Debug.Log($"No item found in socket '{socketName}' to remove");
            return;
        }

        Debug.Log($"Removing item from socket '{socketName}'");
        GameObject item = SnappedItemsStore.Instance.SnappedItems[socketName];
        SnappedItemsStore.Instance.SnappedItems.Remove(socketName);
    }
}
