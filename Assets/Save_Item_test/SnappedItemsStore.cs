using System.Collections.Generic;
using UnityEngine;

public class SnappedItemsStore : MonoBehaviour
{
    public static SnappedItemsStore Instance { get; private set; }

    // Dictionary of snapped items by attach point name
    public Dictionary<string, GameObject> SnappedItems { get; private set; } = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // make the singleton persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
