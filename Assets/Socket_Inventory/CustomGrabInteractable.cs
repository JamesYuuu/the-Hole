using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrabInteractable : XRGrabInteractable
{
    private CustomXRSocketInteractor socketInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (args.interactor is XRDirectInteractor)
        {
            print("Grabbing");
        }
        socketInteractor = selectingInteractor.GetComponent<CustomXRSocketInteractor>();
        if (socketInteractor != null)
        {
            print("Set to True");
            //socketInteractor.SetIsObjectGrabbed(true);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        if (args.interactor is XRDirectInteractor)
        {
            print("Releasing");
        }
        if (socketInteractor != null)
        {
            print("Set to False");
            //socketInteractor.SetIsObjectGrabbed(false);
        }
    }
}
