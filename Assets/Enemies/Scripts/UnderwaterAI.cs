using UnityEngine;
using UnityEngine.Serialization;

public class UnderwaterAI : AbstractAI
{
    /// <summary>
    /// Debug flag. Set to true to force fish to move back to test despawning.
    /// </summary>
    private const bool Debug = false;

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerHealth playerHealth;

    public static bool IsHostile = false;

    [FormerlySerializedAs("Speed")] [SerializeField]
    private float speed = 0.005f;

    [FormerlySerializedAs("MinSteps")] [SerializeField]
    private int minSteps = 500;

    [FormerlySerializedAs("MaxSteps")] [SerializeField]
    private int maxSteps = 1000;

    private int _steps = -1;

    [FormerlySerializedAs("MaxTurns")] [SerializeField]
    private int maxTurns = 1000;

    [FormerlySerializedAs("TurnX")] [SerializeField]
    private float turnX = 0.001f;

    private const float TurnY = 0f;

    [FormerlySerializedAs("TurnZ")] [SerializeField]
    private float turnZ = 0.001f;

    private int _turns;
    private Vector3 _turnAmount = new(0, 0, 0);

    private const int ReturnRadius = 24;
    private const int SpawnBase = -90;
    private const int SpawnHeight = 50;

    private bool _isColliding;

    public void Start()
    {
#pragma warning disable CS0162
        if (Debug) speed = 0.075f;
#pragma warning restore CS0162
    }

    public void FixedUpdate()
    {
        if (SpawnControl.IsFreefall)
        {
            return;
        }

        var loc = gameObject.transform.position;
        if (Debug)
#pragma warning disable CS0162
            UpdateDebug();
#pragma warning restore CS0162
        else
            switch (IsHostile)
            {
                case false when !IsLocationWithinHole(loc):
                    gameObject.transform.Rotate(new(0f, -0.15f, 0f));
                    break;
                case true:
                    UpdateHostile();
                    break;
                default:
                    UpdatePeaceful();
                    break;
            }

        var o = gameObject;
        o.transform.position += o.transform.forward * speed;
        if (_isColliding && IsHostile)
        {
            playerHealth.ChangeOxygen(-attack);
        }
    }

    private void OnTriggerEnter(Collider triggeredCollider)
    {
        if (triggeredCollider.gameObject.Equals(player))
        {
            _isColliding = true;
        }
    }

    private void OnTriggerExit(Collider triggeredCollider)
    {
        if (triggeredCollider.gameObject.Equals(player))
        {
            _isColliding = false;
        }
    }

    private void UpdateDebug()
    {
        var o = gameObject;
        var position = o.transform.position;
        var locX = position.x;
        var locY = position.y;
        var locZ = position.z;

        var position1 = player.transform.position;
        var forwardX = locX - position1.x;
        var forwardY = locY - position1.y;
        var forwardZ = locZ - position1.z;

        o.transform.forward = new Vector3(forwardX, forwardY, forwardZ);
        gameObject.transform.forward.Normalize();
    }

    private void UpdatePeaceful()
    {
        if (_steps == -1)
        {
            _steps = Random.Range(minSteps, maxSteps);
        }

        if (_steps == 0 && _turns == 0)
        {
            _turns = -1;
            UpdatePeacefulTurn();
            return;
        }

        if (_turns != 0)
        {
            UpdatePeacefulTurn();
            return;
        }

        UpdatePeacefulMove();
    }

    private void UpdatePeacefulMove()
    {
        --_steps;
    }

    private void UpdatePeacefulTurn()
    {
        if (_turns == -1)
        {
            float randTurnX = Random.Range(-turnX, turnX);
            float randTurnY = Random.Range(-TurnY, TurnY);
            float randTurnZ = Random.Range(-turnZ, turnZ);

            _turnAmount = new(randTurnX, randTurnY, randTurnZ);
            _turns = Random.Range(0, maxTurns);
        }

        if (_turns == 0)
        {
            _steps = Random.Range(minSteps, maxSteps);
        }

        var o = gameObject;
        var forward = o.transform.forward;
        forward += _turnAmount;
        o.transform.forward = forward;
        forward.Normalize();
        --_turns;
    }

    private void UpdateHostile()
    {
        var o = gameObject;
        var position = o.transform.position;
        var locX = position.x;
        var locY = position.y;
        var locZ = position.z;

        var position1 = player.transform.position;
        var forwardX = position1.x - locX;
        var forwardY = position1.y - locY;
        var forwardZ = position1.z - locZ;

        o.transform.forward = new Vector3(forwardX, forwardY, forwardZ);
        gameObject.transform.forward.Normalize();
    }

    private bool IsLocationWithinHole(Vector3 loc)
    {
        if (FindDistance(loc.x, 0, loc.z) > ReturnRadius)
        {
            return false;
        }

        if (loc.y < SpawnBase || loc.y > SpawnBase + SpawnHeight)
        {
            return false;
        }

        return true;
    }

    private float FindDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }
}