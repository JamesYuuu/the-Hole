using UnityEngine;

public class DoorTag : MonoBehaviour
{
    [SerializeField] private GameObject actualDoor;
    public void ActivateDoor()
    {
        actualDoor.SetActive(true);
    }
} 