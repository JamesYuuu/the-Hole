using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public interface IGrappleable
{

}

public enum GrappleState
{
    Aiming,
    Shooting,
    Reeling
}


[RequireComponent(typeof(LineRenderer))]
public class Grappleable : MonoBehaviour, IGrappleable
{
    [Header("Object References")]
    public Transform pointer;
    public GameObject targetPointPrefab;
    public Rigidbody affectedRigidbody;

    [Header("Haptics")]
    public XRBaseController xrController;
    [Range(0, 1)]
    public float shootVibrationAmplitude = 0.3f;
    public float shootVibrationDuration = 0.05f;
    public float reelVibrationAmplitude = 0.1f;
    public float reelVibrationSpeed = 2;

    [Header("Variables")]
    public LayerMask ignoreLayers;
    public float range = 30f;
    public float shootSpeed = 25f;
    public float reelSpeed = 50f;

    [Header("Materials")]
    public Material cannotShootMaterial;
    public Material canShootMaterial;
    public Material reelMaterial;

    private GrappleState _state;
    private InputManager _inputManager;

    private GameObject _targetPoint;
    private Vector3 _targetPos;
    private Vector3 _reelDir;
    private Vector3 _hookPos;
    private float _playerMass;

    public enum Hand { Left, Right }
    [Header("Left/Right Hand")]
    [SerializeField]
    private Hand hand;

    private delegate bool CheckForShoot();
    private CheckForShoot _checkForShoot;

    private LineRenderer _lineRenderer;
    private Vector3[] _lineVertices = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        _state = GrappleState.Aiming;
        _inputManager = InputManager.GetInstance();
        _lineRenderer = GetComponent<LineRenderer>();
        _targetPoint = Instantiate(targetPointPrefab);
        _targetPoint.SetActive(false);
        _playerMass = affectedRigidbody.mass;
        if (hand == Hand.Left)
        {
            _checkForShoot = _inputManager.PlayerHoldingTriggerL;
        }
        else
        {
            _checkForShoot = _inputManager.PlayerHoldingTriggerR;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case GrappleState.Aiming:
                AimHook();
                break;
            case GrappleState.Shooting:
                ShootHook();
                break;
            case GrappleState.Reeling:
                break;
            default:
                break;
        }

        RenderReelLine();

        Debug.DrawLine(pointer.position, pointer.position + pointer.forward * range, Color.cyan); // Debug line showing pointer direction
    }

    void FixedUpdate()
    {
        // ReelHook logic is in fixed update because it involves physics
        if (_state == GrappleState.Reeling) ReelHook();
    }

    void ChangeState(GrappleState newState)
    {

        _targetPoint.SetActive(false);

        switch (newState)
        {
            case GrappleState.Aiming:
                _hookPos = pointer.position;
                break;
            case GrappleState.Shooting:
                xrController.SendHapticImpulse(shootVibrationAmplitude, shootVibrationAmplitude);
                _hookPos = pointer.position;
                break;
            case GrappleState.Reeling:
                break;
            default:
                break;
        }

        _state = newState;
    }

    void AimHook()
    {
        RaycastHit hit;
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, range, ~ignoreLayers)){
            _targetPoint.SetActive(true);
            _targetPoint.transform.position = hit.point;
            _targetPoint.transform.rotation = Quaternion.LookRotation(hit.normal);
            _targetPos = _targetPoint.transform.position;
            if (_checkForShoot()) ChangeState(GrappleState.Shooting);

        }
        else
        {
            _targetPoint.SetActive(false);
        }
    }

    void ShootHook()
    {
        if (!_checkForShoot()) ChangeState(GrappleState.Aiming);
        _hookPos = Vector3.MoveTowards(_hookPos, _targetPos, shootSpeed * Time.deltaTime);
        if (Vector3.Distance(_hookPos, _targetPos) < float.Epsilon) _state = GrappleState.Reeling;
    }

    void ReelHook()
    {
        if (!_checkForShoot()) ChangeState(GrappleState.Aiming);
        if (affectedRigidbody.velocity.magnitude > reelVibrationSpeed) xrController.SendHapticImpulse(reelVibrationAmplitude, 0.1f);
        _reelDir = Vector3.Normalize(_targetPos - pointer.position);
        affectedRigidbody.AddForce(_reelDir * reelSpeed * _playerMass);
    }

    void RenderReelLine()
    {
        // lineVertices[0] is the far end of the line
        switch (_state)
        {
            case GrappleState.Aiming:
                _lineRenderer.enabled = true;
                if (_targetPoint.activeInHierarchy)
                {
                    // if target point is active, it means player can shoot
                    _lineRenderer.material = canShootMaterial;
                    // set far end of line to target position
                    _lineVertices[0] = _targetPos;
                }
                else
                {
                    // if target point is not active, it means player cannot shoot
                    _lineRenderer.material = cannotShootMaterial;
                    // set far end of line to <range> distance away from pointer
                    _lineVertices[0] = pointer.position + pointer.forward * range;
                }
                break;
            case GrappleState.Shooting:
                // in this state, far end of line moves with the hook
                _lineRenderer.material = reelMaterial;
                _lineVertices[0] = _hookPos;
                break;
            case GrappleState.Reeling:
                // in this state, hook should not be moving, so no updates to lineVertices[0]
                _lineVertices[0] = _hookPos;
                break;
            default:
                break;
        }

        // lineVertices[1] is the origin position of the grappling gun
        _lineVertices[1] = pointer.position;

        // Set the positions in the Line Renderer Component
        _lineRenderer.SetPositions(_lineVertices);
    }

    public void SetValues(float range, float shootSpeed, float reelSpeed)
    {
        this.range = range;
        this.shootSpeed = shootSpeed;
        this.reelSpeed = reelSpeed;
    }

    void OnDisable()
    {
        if (_targetPoint) _targetPoint.SetActive(false);
    }

    void OnEnable()
    {
    }

}
