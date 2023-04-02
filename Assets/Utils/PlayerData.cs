using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class containing the player's current state.
/// TODO: how do I place this on the XRRig? And how do I make it DoNotDestroyOnLoad scenes?
/// </summary>
public static class PlayerData
{
    /// <summary>
    /// Amount of money that player has.
    /// Can be get from anywhere, but only set through methods.
    /// </summary>
    public static int Money { get; private set; } = 0;
    public static Dictionary<Item, int> Inventory { get; private set; } = new();
    /// <summary>
    /// Treasure item. Given to player upon pickup.
    /// </summary>
    private static readonly Item Treasure = new("Treasure", "An ancient relic. Who knows what finding this might do?", -1);
    private static readonly Dictionary<Item, int> InventoryMax = new()
    {
        {Treasure, 1},
    };

    private static Dictionary<string, bool> _itemsForsale = new()
    {
        {"Shotgun", true},
        {"O2 Tank", true},
        {"Diving Mask", true},
        {"Monster Energy Drink", true},
        {"Diving Equipment", true},
        {"Diving Helmet", true},
        {"Fins", true},
    };

    public static bool LeftHandGrapple = false;
    public static float GrapplingShootSpeed = 25.0f;
    public static float GrapplingReelSpeed = 40.0f;
    public static float GrapplingRange = 40;
    public static float Oxygen = 100;

    public static bool IsEnergyDrinkBought = false;
    public static bool IsO2TankBought = false;

    // C# Dictionary == Java's HashMap
    // an element can be (Item = SodaCan, quantity = 3)
    // we need a way to limit some items to be only bought once

    /// <summary>
    /// For debug. This works when called from CreateShopItems.cs::Start()
    /// </summary>
    public static void TestExistence()
    {
        Debug.Log("PlayerData static class exists");
    }

    #region Money
    public static void AddMoney(int amount)
    {
        Money += amount;
    }

    public static bool RemoveMoney(int amount)
    {
        if (Money < amount)
        {
            return false;
        }
        Money -= amount;
        return true;
    }
    #endregion

    #region Inventory

    // C# dictionary methods:
    // .Add(Item i, int qty),
    // .ContainsKey(Item i),
    // .TryGetValue(Item i, out variable), // returns bool and stores value in variable
    // .Count,
    // .Remove(Item i),
    // .Inventory[key] // to get the value

    public static void AddTreasure()
    {
        AddItem(Treasure, 1);
        UnderwaterAI.IsHostile = true;
    }

    /// <summary>
    /// Removes the treasure from the player's inventory.
    /// MUST BE CALLED AT START OF EACH DIVE.
    /// </summary>
    public static void RemoveTreasure()
    {
        RemoveItem(Treasure, 1);
    }

    public static bool HasTreasure()
    {
        if (!Inventory.ContainsKey(Treasure)) return false;
        return Inventory[Treasure] == 1;
    }

    /// <summary>
    /// Adds item to the player's inventory safely.
    /// Will not insert item past limit set in InventoryMax.
    /// </summary>
    /// <param name="item">Item to be added to the player's inventory.</param>
    /// <param name="amount">Amount of the item to be added to the player's inventory.</param>
    /// <returns>Whether the item has been added successfully.</returns>
    private static bool AddItem(Item item, int amount)
    {
        int currAmount = 0;
        if (Inventory.ContainsKey(item))
        {
            currAmount = Inventory[item];
        }
        int newAmount = currAmount + amount;
        if (newAmount > InventoryMax[item])
        {
            return false;
        }
        Inventory[item] = newAmount;
        return true;
    }

    /// <summary>
    /// Removes item from the player's inventory safely.
    /// Will not remove items if resulting amount is less than 0.
    /// </summary>
    /// <param name="item">Item to be removed from the player's inventory.</param>
    /// <param name="amount">Amount of the item to be removed from the player's inventory.</param>
    /// <returns>Whether the item has been removed successfully.</returns>
    private static bool RemoveItem(Item item, int amount)
    {
        if (!Inventory.ContainsKey(item))
        {
            return false;
        }
        int currAmount = Inventory[item];
        int newAmount = currAmount - amount;
        if (newAmount < 0)
        {
            return false;
        }
        if (newAmount == 0)
        {
            Inventory.Remove(item);
            return true;
        }
        Inventory[item] = newAmount;
        return true;
    }
    #endregion

    #region ShopItems
    public static bool IsItemForSale(string itemName)
    {
        return _itemsForsale[itemName];
    }
    public static void RemoveItemForSale(string itemName)
    {
        _itemsForsale[itemName] = false;
    }

    public static void AddOxygen(int amount)
    {
        Oxygen += amount;
    }

    public static void AddGrapplingShootSpeed(float amount)
    {
        GrapplingShootSpeed += amount;
    }
    public static void AddGrapplingReelSpeed(float amount)
    {
        GrapplingReelSpeed += amount;
    }
    public static void AddGrapplingRange(float amount)
    {
        GrapplingRange += amount;
    }

    #endregion
}
