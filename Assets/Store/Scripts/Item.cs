using UnityEngine;

/// <summary>
/// The data associated with the item this script it placed on.
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] private string displayName;
    [SerializeField] private int price;
    [SerializeField] [TextArea] private string description;

    public string GetName()
    {
        return displayName;
    }

    public int GetPrice()
    {
        return price;
    }

    public string GetDescription()
    {
        return description;
    }

    public bool IsBought { get; private set; }

    public Item(string name, string description, int price)
    {
        displayName = name;
        this.price = price;
        IsBought = PlayerData.Inventory.ContainsKey(this);
    }

    /// <summary>
    /// Increments the quantity of this item in the inventory
    /// if it is bought. Else, adds this item to the inventory.
    /// </summary>
    public void BuyItem()
    {
        if (IsBought)
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
        if (IsBought)
        {
            PlayerData.Inventory.Remove(this);
            IsBought = false;
        }
    }
}
