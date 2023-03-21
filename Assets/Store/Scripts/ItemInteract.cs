using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class ItemInteract : XRSimpleInteractable
{
    [SerializeField] private Item item;
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        ShopManager._instance.ShowPanels(item);
    }

}