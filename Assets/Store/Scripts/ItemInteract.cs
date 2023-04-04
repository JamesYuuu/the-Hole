using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class ItemInteract : XRGrabInteractable
{
    [SerializeField] private Item item;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (ShopManager.Instance == null) return;
        if (item != null) ShopManager.Instance.ShowPanels(item);
    }
}