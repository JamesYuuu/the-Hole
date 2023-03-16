using System.Collections.Generic;
using Dialog.Scripts;
using Store.Scripts;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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

    // UI elements
    [SerializeField] private GameObject ShopItemDescriptionPanel, ShopItemPricePanel, ShopItemNamePanel;
    [SerializeField] private GameObject ShopCartTotalPanel, ShopPlayerMoneyPanel;

    public static ShopManager _instance;
    private int TotalPrice, PlayerMoney;

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// Refer to IShop.cs for all function descriptions
    /// </summary>
    public void EnableGrab()
    {
        ItemsForSale.ForEach(item => item.gameObject.SetActive(true)); // replace this with making it grabbable
    }

    public void ShowPanels(Item GrabbedItem)
    {
        ShopItemDescriptionPanel.GetComponent<TMP_Text>().text = GrabbedItem.GetDescription();
        ShopItemPricePanel.GetComponent<TMP_Text>().text = GrabbedItem.GetPrice().ToString() + 'G';
        ShopItemNamePanel.GetComponent<TMP_Text>().text = GrabbedItem.name;
    }

    public void AddToCart(Item item)
    {
        ItemsInCart.Add(item);
        SetTotalMoney(item.GetPrice());
    }

    public void RemoveFromCart(Item item)
    {
        ItemsInCart.Remove(item);
        SetTotalMoney(-item.GetPrice());
    }

    public bool Checkout()
    {
        CheckoutEvent.Invoke();
        
        if (TotalPrice <= PlayerMoney)
        {
            PlayerData.RemoveMoney(TotalPrice);
            return true;
        }
        else
        {
            // prompt ui to show you have not enough money
            return false;
        }
    }

    public void SetPlayerMoney()
    {
        PlayerMoney = PlayerData.Money;
        ShopPlayerMoneyPanel.GetComponent<TMP_Text>().text = PlayerMoney.ToString() + 'G';
    }
    
    public void SetTotalMoney(int AddedMoney)
    {
        TotalPrice += AddedMoney;
        ShopCartTotalPanel.GetComponent<TMP_Text>().text = TotalPrice.ToString() + 'G';
    }
}
