using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class containing the player's current state.
/// TODO: how do I place this on the XRRig? And how do I make it DoNotDestroyOnLoad scenes?
/// </summary>
public static class PlayerData
{
    public static int Money { get; private set; } = 0;
    // this lets other scripts get PlayerData.Money; but they cannot set it.
    public static Dictionary<Item, int> Inventory { get; private set; } = new();
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

    public static void RemoveMoney(int amount)
    {
        Money -= amount;
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
    /*
     * example on how to use values if they are in the dictionary:
        int quantity = 0;
        if (Inventory.TryGetValue("Oxygen", out quantity))
        {
            Debug.Log(quantity);
        }
        
     * eg. on how to loop through dictionary:
        foreach( KeyValuePair<Item, int> kvp in Inventory )
        {
            Debug.Log("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }
     * see more at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=netframework-4.8
     */
    #endregion
}
