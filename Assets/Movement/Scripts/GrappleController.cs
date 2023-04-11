using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    public Grappleable leftGrapple;
    public Grappleable rightGrapple;

    public static GrappleController _instance;

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
        _instance = this;
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

    /// <summary>
    /// Sets the range, shooting speed, reeling speed of a left or right hand grapple
    /// </summary>
    /// <param name="hand">Which hand, either Grappleable.Hand.Left or Grappleable.Hand.Right.</param>
    /// <param name="range">New range for grappling hook.</param>
    /// <param name="shootSpeed">New shooting speed for grappling hook.</param>
    /// <param name="reelSpeed">New reeling speed for grappling hook.</param>
    public void SetGrappleValues(Grappleable.Hand hand, float range, float shootSpeed, float reelSpeed)
    {
        switch (hand) {
            case Grappleable.Hand.Left:
                leftGrapple.SetValues(range, shootSpeed, reelSpeed);
                break;
            case Grappleable.Hand.Right:
                rightGrapple.SetValues(range, shootSpeed, reelSpeed);
                break;
        }
    }
}
