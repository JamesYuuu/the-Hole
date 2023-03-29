using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerColliderResizer : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform cameraTransform;
    private CapsuleCollider playerCollider;
    private float playerHeight;
    private Vector3 colliderCenter;

    void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerCollider.direction = 1;
        colliderCenter = Vector3.zero;
    }

    
    void Update()
    {
        // get the player's height relative to (0,0,0) in XR Space
        playerHeight = xrOrigin.CameraInOriginSpaceHeight;

        // add buffer space on top of player's head, set collider height
        playerCollider.height = playerHeight + playerCollider.radius;
        colliderCenter.y = -(playerCollider.height * 0.5f - playerCollider.radius);
        playerCollider.center = colliderCenter;

        // move collider to line up with camera
        gameObject.transform.position = cameraTransform.position;
    }
}
