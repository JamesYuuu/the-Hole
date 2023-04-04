using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class EventTriggerVolume : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnLeave;
    [SerializeField] private bool _isReactive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<XRDirectInteractor>()) return;
        if (_isReactive) OnEnter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<XRDirectInteractor>()) return;
        if (_isReactive) OnLeave.Invoke();
    }

    /// <summary>
    /// Triggered by reaching the correct dialogue step
    /// </summary>
    public void SetReactive(bool isReactive) { _isReactive = isReactive; }
}
