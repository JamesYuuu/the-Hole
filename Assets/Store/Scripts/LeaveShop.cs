using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class LeaveShop : MonoBehaviour
{
    public UnityEvent leave;
    public void OnTriggerEnter(Collider other)
    {
        // TODO: change the hand tag on the player prefab to player too.
        // if (other.gameObject.CompareTag("Player"))
        if (other.GetComponent<XRDirectInteractor>() != null)
        {
            leave.Invoke();
        }
    }
}
