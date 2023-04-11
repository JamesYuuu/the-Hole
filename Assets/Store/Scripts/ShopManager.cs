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
    public UnityEvent checkoutEvent;
    // can use PlayerData.Money, .AddMoney() and .RemoveMoney() for money operations.
    [SerializeField] private List<Item> itemsForSale = new List<Item>();

    [SerializeField] private List<Item> itemsInCart = new List<Item>();

    // UI elements
    [SerializeField] private GameObject shopItemDescriptionPanel;
    [SerializeField] private GameObject shopItemPricePanel;
    [SerializeField] private GameObject shopItemNamePanel;
    [SerializeField] private GameObject shopCartTotalPanel;
    [SerializeField] private GameObject shopPlayerMoneyPanel;

    public static ShopManager Instance;
    private int _totalPrice, _playerMoney;

    private void Awake()
    {
        PlayerData.AddMoney(3000);

        Instance = this;
        itemsForSale.ForEach(item => SetActive(item));
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
        SetPlayerMoney();
        SetCartTotalMoney(0);
    }

    public void ShowPanels(Item grabbedItem)
    {
        shopItemDescriptionPanel.GetComponent<TMP_Text>().text = grabbedItem.GetDescription();
        shopItemPricePanel.GetComponent<TMP_Text>().text = grabbedItem.GetPrice().ToString() + 'G';
        shopItemNamePanel.GetComponent<TMP_Text>().text = grabbedItem.GetName();
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

    /// <summary>
    /// Triggered by checkout button in shop scene.
    /// Singleton method.
    /// </summary>
    public static void CheckoutStatic()
    {
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
        if (item.GetName() != "Energy Drink" && item.GetName() != "O2 Tank")
        {
            PlayerData.RemoveItemForSale(item.GetName());
            item.gameObject.SetActive(false);
        }
        switch (item.GetName())
        {
            case "Grappling Hook":
                PlayerData.LeftHandGrapple = true;
                break;
            case "Diving Helmet":
                PlayerData.MultMaxOxygen(2);
                break;
            case "Diving Mask":
                PlayerData.MultMaxOxygen(2);
                break;
            case "Diving Equipment":
                PlayerData.MultGrapplingShootSpeed(1.5f);
                PlayerData.MultGrapplingReelSpeed(1.5f);
                PlayerData.MultGrapplingRange(1.5f);
                break;
            case "Fins":
                PlayerData.AddGrapplingReelSpeed(10.0f);
                break;
            case "O2 Tank":
                item.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("ItemPurchased");
                break;
            case "Energy Drink":
                item.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("ItemPurchased");
                break;
                /*
                case "O2 Tank":
                    PlayerData.AddOxygen(100);
                    PlayerData.IsO2TankBought = true;
                    break;
                    */
                /*
                case "Monster Energy Drink":
                    PlayerData.AddGrapplingReelSpeed(10.0f);
                    PlayerData.IsEnergyDrinkBought = true;
                    break;
                    */
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
