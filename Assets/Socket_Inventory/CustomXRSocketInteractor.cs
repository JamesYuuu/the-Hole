// CustomXRSocketInteractor.cs
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomXRSocketInteractor : XRSocketInteractor
{
    BlinkImage blink;
    public string SocketName;
    private bool isObjectGrabbed;

    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        base.OnSelectEntered(interactable);

        if (interactable.CompareTag("treasure"))
        {
            blink = GameObject.Find("Watch").GetComponent<BlinkImage>();
            PlayerData.AddTreasure();
            blink.StartBlinking();
        }

        if (interactable.CompareTag("Item"))
        {
            ItemUpgrade._instance.SetSelected(false);
            ItemUpgrade._instance.SetItem(null);
        }

        if (interactable is XRGrabInteractable grabInteractable)
        {
            string prefabName = grabInteractable.gameObject.name;
            PlayerData.AttachInventory[SocketName] = prefabName;
        }
        
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        if (!gameObject.scene.isLoaded)
        {
            return; // skip removing socket name and object name from dictionary
        }

        base.OnSelectExited(interactable);

        if (interactable.CompareTag("treasure"))
        {
            PlayerData.RemoveTreasure();
        }

        if (interactable.CompareTag("Item"))
        {
            ItemUpgrade._instance.SetSelected(true);
            ItemUpgrade._instance.SetItem(interactable.gameObject);
        }

        if (PlayerData.AttachInventory.ContainsKey(SocketName))
        {
            PlayerData.AttachInventory.Remove(SocketName);
        }
    }


}
