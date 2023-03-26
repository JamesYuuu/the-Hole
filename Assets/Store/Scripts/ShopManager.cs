using System.Collections.Generic;
using Dialog.Scripts;
using Store.Scripts;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

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

    [SerializeField] private GameObject ItemInfo, CartTotal, PlayerCash, CheckoutPanel, LeftHandController, RightHandController;

    [SerializeField] private Button CashRegister,Leave;

    public static ShopManager _instance;
    private int TotalPrice, PlayerMoney;

    private Item CurrentSelectedItem;

    private void Awake()
    {
        _instance = this;
        CashRegister.onClick.AddListener(Checkout);
        ItemsForSale.ForEach(item => SetActive(item));
    }

    public void SetActive(Item item)
    {
        if (PlayerData.IsItemForSale(item.GetName()))
        {
            item.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Refer to IShop.cs for all function descriptions
    /// </summary>

    public void EnableGrab()
    {
        Invoke("EnableGrabbing", 3f);
    }   
    
    private void EnableGrabbing()
    {
        CartTotal.SetActive(true);
        PlayerCash.SetActive(true);
        CheckoutPanel.SetActive(true);
        SetPlayerMoney();
        SetTotalMoney(0);
        LeftHandController.GetComponent<XRRayInteractor>().enabled=true;
        RightHandController.GetComponent<XRRayInteractor>().enabled=true;
    }

    public void ShowPanels(Item GrabbedItem)
    {
        ItemInfo.SetActive(true);
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
            ItemsInCart.Clear();
            SetTotalMoney(-TotalPrice);
            SetPlayerMoney();
        }
        else
        {
            ShopItemDescriptionPanel.GetComponent<TMP_Text>().text = "Sorry, you don't have enough money!";
        }
    }

    public void upgrade(Item item)
    {
        item.gameObject.SetActive(false);
        if (item.GetName()!="Monster Energy Drink" || item.GetName()!="O2 Tank")
        {
            PlayerData.RemoveItemForSale(item.GetName());
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
