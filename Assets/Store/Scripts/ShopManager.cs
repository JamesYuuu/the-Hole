using System.Collections.Generic;
using Dialog.Scripts;
using Store.Scripts;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Implements the IShop interface, letting the player
/// buy items and talk with the shopkeeper
/// TODO: send james the interaction sequence
/// </summary>
public class ShopManager : MonoBehaviour, IShop
{
    public UnityEvent CheckoutEvent;
    [SerializeField] private ShopDialogBehaviour dialogBehaviour;

    // can use PlayerData.Money, .AddMoney() and .RemoveMoney() for money operations.
    [SerializeField] private List<Item> ItemsForSale = new List<Item>();
    [SerializeField] private List<Item> ItemsInCart = new List<Item>();

    void SetAllItems()
    {
        // i think you can drag and drop the items into the list in the unity inspector instead
        List<string> ItemNames = new List<string> {"SodaCan"};
    }

    /// <summary>
    /// Refer to IShop.cs for all function descriptions
    /// </summary>
    public void EnableGrab()
    {
        ItemsForSale.ForEach(item => item.gameObject.SetActive(true)); // replace this with making it grabbable
    }

    public void ShowPanels()
    {
        throw new System.NotImplementedException(); // please replace these lines
    }

    public void AddToCart(Item item)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveFromCart(Item item)
    {
        throw new System.NotImplementedException();
    }

    public bool Checkout()
    {
        CheckoutEvent.Invoke();
        // if ( /* have enough money */)
        {
            // PlayerData.RemoveMoney(/* amt in their cart */);
            return true;
        }
        // else
        {
            // prompt ui to show you have not enough money
            return false;
        }
    }
}
