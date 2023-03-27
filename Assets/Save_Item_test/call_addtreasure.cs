using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class call_addtreasure : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        print("calling_add_treasure");
        PlayerData.AddTreasure();

    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
