using System.Collections.Generic;
using Dialog.Scripts;
using Store.Scripts;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Implements the IShop interface, letting the player
/// buy items and talk with the shopkeeper
/// TODO: decouple ui and shop manager operations
/// </summary>
public class ShopManager : MonoBehaviour, IShop
{
    [FormerlySerializedAs("CheckoutEvent")] public UnityEvent checkoutEvent;
    [SerializeField] private ShopDialogBehaviour dialogBehaviour;

    // can use PlayerData.Money, .AddMoney() and .RemoveMoney() for money operations.
    [FormerlySerializedAs("ItemsForSale")] [SerializeField] private List<Item> itemsForSale = new List<Item>();

    [FormerlySerializedAs("ItemsInCart")] [SerializeField] private List<Item> itemsInCart = new List<Item>();

    // UI elements
    [FormerlySerializedAs("ShopItemDescriptionPanel")] [SerializeField] private GameObject shopItemDescriptionPanel;
    [FormerlySerializedAs("ShopItemPricePanel")] [SerializeField] private GameObject shopItemPricePanel;
    [FormerlySerializedAs("ShopItemNamePanel")] [SerializeField] private GameObject shopItemNamePanel;
    [FormerlySerializedAs("ShopCartTotalPanel")] [SerializeField] private GameObject shopCartTotalPanel;
    [FormerlySerializedAs("ShopPlayerMoneyPanel")] [SerializeField] private GameObject shopPlayerMoneyPanel;

    [FormerlySerializedAs("ItemInfo")] [SerializeField] private GameObject itemInfo;
    [FormerlySerializedAs("CartTotal")] [SerializeField] private GameObject cartTotal;
    [FormerlySerializedAs("PlayerCash")] [SerializeField] private GameObject playerCash;
    [FormerlySerializedAs("CheckoutPanel")] [SerializeField] private GameObject checkoutPanel;
    [FormerlySerializedAs("LeftHandController")] [SerializeField] private GameObject leftHandController;
    [FormerlySerializedAs("RightHandController")] [SerializeField] private GameObject rightHandController;

    [FormerlySerializedAs("CashRegister")] [SerializeField] private Button cashRegister;
    [FormerlySerializedAs("Leave")] [SerializeField] private Button leave;

    public static ShopManager Instance;
    private int _totalPrice, _playerMoney;

    private Item _currentSelectedItem;

    private void Awake()
    {
        Instance = this;
        cashRegister.onClick.AddListener(Checkout);
        itemsForSale.ForEach(item => SetActive(item));
        if (PlayerData.IsEnergyDrinkBought)
        {
            PlayerData.AddGrapplingReelSpeed(-10.0f);
            PlayerData.IsEnergyDrinkBought = false;
        }
        if (PlayerData.IsO2TankBought)
        {
            PlayerData.AddOxygen(-100);
            PlayerData.IsO2TankBought = false;
        }
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
        cartTotal.SetActive(true);
        playerCash.SetActive(true);
        SetPlayerMoney();
        SetCartTotalMoney(0);
        leftHandController.GetComponent<XRRayInteractor>().enabled=true;
        rightHandController.GetComponent<XRRayInteractor>().enabled=true;
    }

    public void ShowPanels(Item grabbedItem)
    {
        itemInfo.SetActive(true);
        shopItemDescriptionPanel.GetComponent<TMP_Text>().text = grabbedItem.GetDescription();
        shopItemPricePanel.GetComponent<TMP_Text>().text = grabbedItem.GetPrice().ToString() + 'G';
        shopItemNamePanel.GetComponent<TMP_Text>().text = grabbedItem.GetName();
        _currentSelectedItem = grabbedItem;
    }

    public void AddToCart(Item item)
    {
        itemsInCart.Add(item);
        SetCartTotalMoney(item.GetPrice());
    }

    public void RemoveFromCart(Item item)
    {
        itemsInCart.Remove(item);
        SetCartTotalMoney(-item.GetPrice());
    }

    public static void DebugCheckout()
    {
        Debug.Log("Static Checkout triggered!");
        Instance.Checkout();
    }

    public void Checkout()
    {
        
        if (_totalPrice <= _playerMoney)
        {
            PlayerData.RemoveMoney(_totalPrice);
            checkoutEvent.Invoke();
            itemsInCart.ForEach(item => Upgrade(item));
            itemsInCart.Clear();
            SetCartTotalMoney(-_totalPrice);
            SetPlayerMoney();
        }
        else
        {
            shopItemDescriptionPanel.GetComponent<TMP_Text>().text = "Sorry, you don't have enough money!";
        }
    }

    public void Upgrade(Item item)
    {
        item.gameObject.SetActive(false);
        if (item.GetName()!="Monster Energy Drink" || item.GetName()!="O2 Tank")
        {
            PlayerData.RemoveItemForSale(item.GetName());
        }
        switch (item.GetName()){
            case "Shotgun":
                PlayerData.LeftHandGrapple = true;
                break;
            case "O2 Tank":
                PlayerData.AddOxygen(100);
                PlayerData.IsO2TankBought = true;
                break;
            case "Diving Mask":
                PlayerData.AddOxygen(100);
                break;
            case "Monster Energy Drink":
                PlayerData.AddGrapplingReelSpeed(10.0f);
                PlayerData.IsEnergyDrinkBought = true;
                break;
            case "Diving Equipment":
                PlayerData.AddOxygen(100);
                break;
            case "Diving Helmet":
                PlayerData.AddOxygen(100);
                break;
            case "Fins":
                PlayerData.AddGrapplingReelSpeed(10.0f);
                break;
        }
    }
    public void SetPlayerMoney()
    {
        _playerMoney = PlayerData.Money;
        shopPlayerMoneyPanel.GetComponent<TMP_Text>().text = _playerMoney.ToString() + 'G';
    }
    
    public void SetCartTotalMoney(int addedMoney)
    {
        _totalPrice += addedMoney;
        shopCartTotalPanel.GetComponent<TMP_Text>().text = _totalPrice.ToString() + 'G';
    }
}
