using UnityEngine;
using UnityEngine.Events;

public class LeaveShop : MonoBehaviour
{
    public UnityEvent leave;
    public void OnTriggerEnter(Collider other)
    {
        // TODO: change the hand tag on the player prefab to player too.
        if (other.gameObject.CompareTag("Player"))
        {
            leave.Invoke();
        }
    }
}
