using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Transform visualTarget;
    [SerializeField] private Vector3 localAxis;
    [SerializeField] private float resetSpeed = 5;
    [SerializeField] private float followAngleThresholdDeg = 45;
    private Vector3 initialLocalPos;
    private Vector3 offset;
    private Transform pokeAttachTransform;
    private XRBaseInteractable interactable;
    private bool isFollowing;
    private bool isFrozen;
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        initialLocalPos = visualTarget.localPosition;
        
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    
    void Update()
    {
        if (isFrozen) return;
        
        if (isFollowing)
        {
            // Make the visual follow the hand, constrained to y axis
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, 
                initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }

    /// <summary>
    /// Makes the button follow the player's hand.
    /// </summary>
    /// <param name="hover"></param>
    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactableObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor) hover.interactorObject;
            isFollowing = true;
            isFrozen = false;

            // save the offset for the visual to follow the hand
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));

            // prevent player from lifting button from the bottom
            if (pokeAngle < followAngleThresholdDeg)
            {
                isFollowing = true;
                isFrozen = false;
            }
        }
        
    }
    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            isFrozen = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFrozen = true;
        }
    }
}
