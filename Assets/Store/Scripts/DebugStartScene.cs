using Dialog.Scripts;
using UnityEngine;

/// <summary>
/// A debug script that starts the conversation with the shop owner NPC upon
/// entering the scene
///
/// TODO: remove when the player can interact with shopkeeper by clicking on them
/// if it makes sense to interact by clicking
/// </summary>
public class DebugStartScene : MonoBehaviour
{
    public ShopDialogBehaviour sdb;

    void Start()
    {
        sdb.StartDialog(); 
        PlayerData.AddMoney(3000);
       // PlayerData.AddItem()
    }
}
