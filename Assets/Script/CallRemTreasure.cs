using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallRemTreasure : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("calling_remove_treasure");
        PlayerData.RemoveTreasure();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
