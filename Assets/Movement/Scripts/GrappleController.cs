using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    public Grappleable leftGrapple;
    public Grappleable rightGrapple;

    public bool forceActiveLeftGrapple = false;
    public bool overrideStaticVariables = false;
    void Awake()
    {
        if (!overrideStaticVariables)
        {
            leftGrapple.SetValues(PlayerData.GrapplingRange, PlayerData.GrapplingShootSpeed, PlayerData.GrapplingReelSpeed);
            rightGrapple.SetValues(PlayerData.GrapplingRange, PlayerData.GrapplingShootSpeed, PlayerData.GrapplingReelSpeed);
        }
        SetGrappleActive(Grappleable.Hand.Left, PlayerData.LeftHandGrapple || forceActiveLeftGrapple);
    }


    /// <summary>
    /// Sets whether a left or right Grapple Hook GameObject is active.
    /// </summary>
    /// <param name="hand">Which hand, either Grappleable.Hand.Left or Grappleable.Hand.Right.</param>
    /// <param name="isActive">Whether the Grapple Hook should be active or not.</param>
    public void SetGrappleActive(Grappleable.Hand hand, bool isActive) 
    {
        switch (hand) {
            case Grappleable.Hand.Left:
                leftGrapple.gameObject.SetActive(isActive);
                break;
            case Grappleable.Hand.Right:
                rightGrapple.gameObject.SetActive(isActive);
                break;
        }
    }

    /// <summary>
    /// Returns whether a left or right Grapple Hook GameObject is active.
    /// </summary>
    /// <param name="hand">Which hand, either Grappleable.Hand.Left or Grappleable.Hand.Right.</param>
    /// <returns>Whether the Grapple Hook GameObject is active.</returns>
    public bool CheckGrappleActive(Grappleable.Hand hand)
    {
        switch (hand) {
            case Grappleable.Hand.Left:
                return leftGrapple.isActiveAndEnabled;
            case Grappleable.Hand.Right:
                return rightGrapple.isActiveAndEnabled;
            default:
                return false;
        }
    }
}
