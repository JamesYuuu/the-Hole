using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

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
        return _playerControls.VR.ShootR.IsPressed();
    }
    public bool PlayerHoldingPrimaryR() {
        return _playerControls.VR.Ascend.IsPressed();
    }

    public bool PlayerPressedPrimaryL() {
        return _playerControls.VR.NextDialog.IsPressed();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
