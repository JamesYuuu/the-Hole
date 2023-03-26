using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // List of items currently stored in the inventory
    private Dictionary<int, string> inventory = new Dictionary<int, string>();

    // Add an item to the inventory with the specified attach point index and prefab name
    public void AddItem(int attachPointIndex, string prefabName)
    {
        inventory.Add(attachPointIndex, prefabName);
        print("Added " + prefabName + " to inventory at attach point index " + attachPointIndex);
    }

    // Remove an item from the inventory with the specified attach point index
    public void RemoveItem(int attachPointIndex)
    {
        if (inventory.ContainsKey(attachPointIndex))
        {
            string prefabName = inventory[attachPointIndex];
            inventory.Remove(attachPointIndex);
            print("Removed " + prefabName + " from inventory at attach point index " + attachPointIndex);
        }
        else
        {
            print("No item found in inventory at attach point index " + attachPointIndex);
        }
    }

    // Check if the inventory contains an item with the specified prefab name
    public bool HasItem(string prefabName)
    {
        return inventory.ContainsValue(prefabName);
    }

    // Get the attach point index of an item with the specified prefab name
    public int GetAttachPointIndex(string prefabName)
    {
        foreach (KeyValuePair<int, string> entry in inventory)
        {
            if (entry.Value == prefabName)
            {
                return entry.Key;
            }
        }
        return -1; // Not found
    }
}
