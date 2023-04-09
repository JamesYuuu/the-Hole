using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class LeaveShop : MonoBehaviour
{
    public UnityEvent leave;
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<XRDirectInteractor>() != null)
        {
            leave.Invoke();
        }
    }
}
