using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    [SerializeField] private UnityEvent OnUseItem; 
    [SerializeField] private UnityEvent OnAscend; 
    [SerializeField] private UnityEvent OnGrapple; 

    public static InputManager GetInstance() {
        return _instance;
    }

    private PlayerControls _playerControls;
    private void Awake() {

        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        _playerControls = new PlayerControls();
    }

    private void OnEnable() {
        if (_playerControls != null) _playerControls.Enable();    
    }

    private void OnDisable() {
        if (_playerControls != null) _playerControls.Disable();
    }

    public Vector2 GetCameraDelta() {
        return _playerControls.VR.Direction.ReadValue<Vector2>();
    }

    public bool PlayerHoldingTriggerL() {
        return _playerControls.VR.ShootL.IsPressed();
    }
    public bool PlayerHoldingTriggerR() {
        OnGrapple.Invoke();
        return _playerControls.VR.ShootR.IsPressed();
    }
    public bool PlayerHoldingPrimaryR() {
        OnAscend.Invoke();
        return _playerControls.VR.Ascend.IsPressed();
    }
    public bool PlayerPressedPrimaryL() {
        OnUseItem.Invoke();
        return _playerControls.VR.UseItem.IsPressed();
    }
    public bool PlayerPressedSecondaryL() {
        return _playerControls.VR.NextDialog.IsPressed();
    }
    public bool PlayerHoldingGripR() {
        return _playerControls.VR.GripR.IsPressed();
    }
}
