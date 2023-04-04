using UnityEngine;
using UnityEngine.Events;

public class OffLightOnLeave : MonoBehaviour
{
    public UnityEvent OnLeave;
    private bool _isReactive = false;

    private void OnTriggerExit(Collider other)
    {
        if (_isReactive) OnLeave.Invoke();
    }

    /// <summary>
    /// Triggered by reaching the correct dialogue step
    /// </summary>
    public void SetReactive() { _isReactive = true; }
}
