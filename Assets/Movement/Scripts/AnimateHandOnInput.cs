using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public Animator handAnimator;
    public Grappleable grappleHook;

    public InputActionProperty triggerAction;
    public InputActionProperty gripAction;

    [Header("For Testing Animations")]
    [SerializeField] private bool _debugOverrideValues = false;
    [SerializeField] private bool _triggerPressed = false;
    [SerializeField] private bool _gripPressed = false;
    [SerializeField] private bool _grappleReady = false;
    [SerializeField] private bool _grappleFixed = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (grappleHook != null) 
        {
            grappleHook.OnChangeToAim += HandUnstickFromWall;
            grappleHook.OnChangeToReel += HandStickToWall;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_debugOverrideValues) 
        {
            _triggerPressed = triggerAction.action.ReadValue<bool>();
            _gripPressed = gripAction.action.ReadValue<bool>();

            if (grappleHook != null)
            {
                _grappleReady = grappleHook.IsReadyToShoot();
            } 
            else
            {
                _grappleReady = false;
            }
        }

        handAnimator.SetBool("TriggerPressed", _triggerPressed);
        handAnimator.SetBool("GripPressed", _gripPressed);
        handAnimator.SetBool("GrappleReady", _grappleReady);
        handAnimator.SetBool("GrappleFixed", _grappleFixed);
    }

    void HandUnstickFromWall()
    {
        _grappleFixed = false;
    }

    void HandStickToWall()
    {
        _grappleFixed = true;
    }

    void OnEnable() 
    {
        if (grappleHook != null) 
        {
            grappleHook.OnChangeToAim += HandUnstickFromWall;
            grappleHook.OnChangeToReel += HandStickToWall;
        }
    }

    void OnDisable()
    {
        if (grappleHook != null) 
        {
            grappleHook.OnChangeToAim -= HandUnstickFromWall;
            grappleHook.OnChangeToReel -= HandStickToWall;
        }
    }
}
