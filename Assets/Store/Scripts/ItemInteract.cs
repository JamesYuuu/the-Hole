using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class ItemInteract : XRGrabInteractable
{
    [SerializeField] private Item item;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (item != null) ShopManager._instance.ShowPanels(item);
    }
}