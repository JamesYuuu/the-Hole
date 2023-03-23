using System.Collections.Generic;
using Dialog.Scripts;
using Store.Scripts;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Implements the IShop interface, letting the player
/// buy items and talk with the shopkeeper
/// TODO: decouple ui and shop manager operations
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

    [SerializeField] private Button AddCart,RemoveCart,CashRegister,Leave;

    public static ShopManager _instance;
    private int TotalPrice, PlayerMoney;

    private Item CurrentSelectedItem;

    private void Awake()
    {
        _instance = this;
        PlayerData.AddMoney(2340);
        SetPlayerMoney();
        SetTotalMoney(0);
        //AddCart.onClick.AddListener(AddToCart);
        //RemoveCart.onClick.AddListener(RemoveFromCart);
        CashRegister.onClick.AddListener(Checkout);
    }

    /// <summary>
    /// Refer to IShop.cs for all function descriptions
    /// </summary>

    public void ShowPanels(Item GrabbedItem)
    {
        ShopItemDescriptionPanel.GetComponent<TMP_Text>().text = GrabbedItem.GetDescription();
        ShopItemPricePanel.GetComponent<TMP_Text>().text = GrabbedItem.GetPrice().ToString() + 'G';
        ShopItemNamePanel.GetComponent<TMP_Text>().text = GrabbedItem.GetName();
        CurrentSelectedItem = GrabbedItem;
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

    public void Checkout()
    {
        
        if (TotalPrice <= PlayerMoney)
        {
            PlayerData.RemoveMoney(TotalPrice);
            CheckoutEvent.Invoke();
            ItemsInCart.ForEach(item => upgrade(item));
            SetTotalMoney(0);
            SetPlayerMoney();
            ItemsInCart.Clear();
        }
        else
        {
            // prompt ui to show you have not enough money
        }
    }

    public void upgrade(Item item)
    {
        
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
