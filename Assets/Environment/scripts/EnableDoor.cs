using UnityEngine;

public class EnableDoor : MonoBehaviour
{
    public void EnableTheDoor()
    {
        FindObjectOfType<DoorTag>().ActivateDoor();
            // GetComponent<DoorTag>()
    }
} 