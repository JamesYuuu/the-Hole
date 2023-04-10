using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WarningVignetteController : LocomotionProvider
{
    [Header("Debug")][SerializeField] private bool debugActivate = false;
    // Start is called before the first frame update
    void Start()
    {
        locomotionPhase = LocomotionPhase.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // Tunneling Vignette Controller from Unity XR Package turns on the vignette 
        // when a locomotion provider-type script has its LocomotionPhase moving.
        if ((PlayerData.HasTreasure() || debugActivate) && locomotionPhase == LocomotionPhase.Idle) 
        {
            // if Player has treasure, set locomotion to "moving" 
            // (get the tunneling vignette controller to trigger the vignette)
            locomotionPhase = LocomotionPhase.Started;
        } 
        else if (locomotionPhase == LocomotionPhase.Started)
        {
            // change from "Started" to "Moving" after one frame
            locomotionPhase = LocomotionPhase.Moving;
        }
        else if (locomotionPhase == LocomotionPhase.Moving && !(PlayerData.HasTreasure() || debugActivate))
        {
            // If LocomotionPhase if "Moving" and player does NOT have treasure,
            // Set LocomotionPhase to Done
            locomotionPhase = LocomotionPhase.Done;
        }
        else if (locomotionPhase == LocomotionPhase.Done)
        {
            // change from "Done" to "Idle" after one frame
            locomotionPhase = LocomotionPhase.Idle;
        }
    }
}
