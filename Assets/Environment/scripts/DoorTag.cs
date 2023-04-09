using UnityEngine;

public class DoorTag : MonoBehaviour
{
    // shooting door
    [SerializeField] private GameObject actualDoor;
    public void ActivateDoor()
    {
        actualDoor.SetActive(true);
    }
} 