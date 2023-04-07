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

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        ItemUpgrade._instance.SetSelected(true);
        ItemUpgrade._instance.SetItemName(item.GetName());
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ItemUpgrade._instance.SetSelected(false);
    }
}