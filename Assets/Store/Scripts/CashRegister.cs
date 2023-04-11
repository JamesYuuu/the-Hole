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

    public bool TankIsBrought = false;
    public bool DrinkIsBrought = false;
    public static CashRegister _instance;
    private void Awake()
    {
        _instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        Item i = other.gameObject.GetComponent<Item>();
        if (i == null || i.GetName() == "Energy Drink" && DrinkIsBrought == true || i.GetName() == "O2 Tank" && TankIsBrought == true)
        {
            return;
        }
        ShopManager.Instance.AddToCart(i);
    }
    
    /// <summary>
    /// Remove the item that exited from the shop
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        Item i = other.gameObject.GetComponent<Item>();
        if (i == null || i.GetName() == "Energy Drink" && DrinkIsBrought == true || i.GetName() == "O2 Tank" && TankIsBrought == true)
        {
            return;
        }
        ShopManager.Instance.RemoveFromCart(i);
    }
}
