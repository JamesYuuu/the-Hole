using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : LocomotionProvider
{
    public float ascendVelocity = 1.0f;
    public float vignetteVelocity = 1.0f;
    private Rigidbody rb;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.GetInstance();
        rb = GetComponent<Rigidbody>();
        locomotionPhase = LocomotionPhase.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (locomotionPhase == LocomotionPhase.Idle && rb.velocity.magnitude > vignetteVelocity) {
            locomotionPhase = LocomotionPhase.Started;
        } else if (rb.velocity.magnitude > vignetteVelocity) {
            locomotionPhase = LocomotionPhase.Moving;
        } else if (locomotionPhase == LocomotionPhase.Moving && rb.velocity.magnitude < vignetteVelocity) {
            locomotionPhase = LocomotionPhase.Done;
        } else if (locomotionPhase == LocomotionPhase.Done) {
            locomotionPhase = LocomotionPhase.Idle;
        }

        if (inputManager.PlayerHoldingPrimaryR()) {
            rb.MovePosition(rb.position + Vector3.up * ascendVelocity * Time.deltaTime);
        }
    }
}
