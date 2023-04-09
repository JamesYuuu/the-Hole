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
            blink.StartBlinking();
            Debug.Log("Treasure enter");
        }

        if (interactable is XRGrabInteractable grabInteractable)
        {
            string prefabName = grabInteractable.gameObject.name;
            PlayerData.AttachInventory[SocketName] = prefabName;
            print(prefabName + " has been ATTACHED from: " + SocketName);
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
            Debug.Log("Treasure exit");
        }

        if (PlayerData.AttachInventory.ContainsKey(SocketName))
        {
            PlayerData.AttachInventory.Remove(SocketName);
            print(interactable + " has been REMOVED from: " + SocketName);
        }
    }


}
