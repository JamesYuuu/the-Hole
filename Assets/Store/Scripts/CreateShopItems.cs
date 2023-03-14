using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShopItems : MonoBehaviour
{

    // can use PlayerData.Money, .AddMoney() and .RemoveMoney() for money operations.
    public List<Item> Items = new List<Item>();

    void SetAllItems()
    {
        List<string> ItemNames = new List<string> {"SodaCan"};
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerData.TestExistence();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
