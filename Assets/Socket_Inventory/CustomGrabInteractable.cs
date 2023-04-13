using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrabInteractable : XRGrabInteractable
{
    private CustomXRSocketInteractor socketInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        socketInteractor = selectingInteractor.GetComponent<CustomXRSocketInteractor>();
    }
}
