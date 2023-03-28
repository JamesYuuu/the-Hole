using UnityEngine;

/// <summary>
/// A trigger class that detects items added and removed, and sends the
/// info to shopmanager. To be placed on the trigger collider volume
/// of the cash register.
/// </summary>
public class CashRegister : MonoBehaviour
{
    /// <summary>
    /// Set up an event that sends this item component to shopmanager when an item enters
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Item i = other.gameObject.GetComponent<Item>();
        if (i != null) ShopManager.Instance.AddToCart(i);
    }
    
    /// <summary>
    /// Remove the item that exited from the shop
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        Item i = other.gameObject.GetComponent<Item>();
        if (i != null) ShopManager.Instance.RemoveFromCart(i);
    }
}
