using UnityEngine;

/// <summary>
/// The data associated with the item this script it placed on.
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] private string displayName;
    [SerializeField] private int price;
    [SerializeField][TextArea] private string description;
    // place this script on an item
    // to get the gameObject it is placed on, use this.gameObject
    public bool isBought { get; private set; }

    public Item(string name, string description, int price)
    {
        this.name = name;
        this.price = price;
        isBought = PlayerData.Inventory.ContainsKey(this);
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