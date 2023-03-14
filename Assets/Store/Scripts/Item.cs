using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public int price;
    public GameObject item;
    public bool isBought;

    public Item(string name, string description, int price, GameObject item)
    {
        this.name = name;
        this.price = price;
        this.item = item;
        isBought = PlayerData.Inventory.ContainsKey(this);
        // ian to james: did i understand your if-else block correctly?
        // it was checking the inventory if there was the item and returned true if there was.
    }

    /// <summary>
    /// Increments the quantity of this item in the inventory
    /// if it is bought. Else, adds this item to the inventory.
    /// </summary>
    public void BuyItem()
    {
        if (isBought)
        {
            PlayerData.Inventory[this]++;
        }
        else
        {
            PlayerData.Inventory.Add(this, 1);
        }
    }

    /// <summary>
    /// Removes this item from the inventory.
    /// This function is used to make items expire.
    /// </summary>
    public void RemoveItem()
    {
        if (isBought)
        {
            PlayerData.Inventory.Remove(this);
            isBought = false;
        }
    }
}