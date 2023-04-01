using UnityEngine;
using Unity.XR.CoreUtils;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerColliderResizer : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform cameraTransform;
    private CapsuleCollider _playerCollider;
    private float _playerHeight;
    private Vector3 _colliderCenter;

    void Awake()
    {
        _playerCollider = GetComponent<CapsuleCollider>();
        _playerCollider.direction = 1;
        _colliderCenter = Vector3.zero;
    }

    
    void Update()
    {
        // get the player's height relative to (0,0,0) in XR Space
        _playerHeight = xrOrigin.CameraInOriginSpaceHeight;

        // add buffer space on top of player's head, set collider height
        _playerCollider.height = _playerHeight + _playerCollider.radius;
        _colliderCenter.y = -(_playerCollider.height * 0.5f - _playerCollider.radius);
        _playerCollider.center = _colliderCenter;

        // move collider to line up with camera
        gameObject.transform.position = cameraTransform.position;
    }
}
