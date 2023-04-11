using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : LocomotionProvider
{
    public float ascendVelocity = 1.0f;
    public float vignetteVelocity = 1.0f;
    private Rigidbody _rb;
    private InputManager _inputManager;

    private bool _canAscend = false;

    // Start is called before the first frame update
    void Start()
    {
        _inputManager = InputManager.GetInstance();
        _rb = GetComponent<Rigidbody>();
        locomotionPhase = LocomotionPhase.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (locomotionPhase == LocomotionPhase.Idle && _rb.velocity.magnitude > vignetteVelocity) {
            locomotionPhase = LocomotionPhase.Started;
        } else if (locomotionPhase == LocomotionPhase.Started) {
            locomotionPhase = LocomotionPhase.Moving;
        } else if (locomotionPhase == LocomotionPhase.Moving && _rb.velocity.magnitude < vignetteVelocity) {
            locomotionPhase = LocomotionPhase.Done;
        } else if (locomotionPhase == LocomotionPhase.Done) {
            locomotionPhase = LocomotionPhase.Idle;
        }

        if (_inputManager.PlayerHoldingPrimaryR() && _canAscend) {
            _rb.MovePosition(_rb.position + Vector3.up * (ascendVelocity * Time.deltaTime));
        }
    }

    public void AllowAscend(bool allow) 
    {
        _canAscend = allow;
    }
}
