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
        bool isTreasure = false;
        base.OnSelectEntered(interactable);

        if (interactable.CompareTag("treasure"))
        {
            blink = GameObject.Find("Watch").GetComponent<BlinkImage>();
            blink.StartBlinking();
            isTreasure = true;
            Debug.Log("Treasure enter");
        }

        if (interactable is XRGrabInteractable grabInteractable)
        {
            if (isTreasure)
            {
                PlayerData.AddTreasure();
            }
            string prefabName = grabInteractable.gameObject.name;
            PlayerData.AttachInventory[SocketName] = prefabName;
            print(prefabName + " has been ATTACHED from: " + SocketName);
        }
        
    }

    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        bool isTreasure = false;
        if (!gameObject.scene.isLoaded)
        {
            return; // skip removing socket name and object name from dictionary
        }

        base.OnSelectExited(interactable);

        if (interactable.CompareTag("treasure"))
        {
            isTreasure=true;
            Debug.Log("Treasure exit");
        }

        if (PlayerData.AttachInventory.ContainsKey(SocketName))
        {
            if (isTreasure)
            {
                PlayerData.RemoveTreasure();
            }
            PlayerData.AttachInventory.Remove(SocketName);
            print(interactable + " has been REMOVED from: " + SocketName);
        }
    }


}
