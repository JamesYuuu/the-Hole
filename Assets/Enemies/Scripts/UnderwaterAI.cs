using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnderwaterAI : AbstractAI
{
    /// <summary>
    /// Debug flag. Set to true to force fish to move back to test despawning.
    /// </summary>
    [SerializeField] private static bool _debug = false;
    [FormerlySerializedAs("Player")] [SerializeField] private GameObject player;

    public static bool IsHostile = false;
    [FormerlySerializedAs("Speed")] [SerializeField] private float speed = 0.005f;
    [FormerlySerializedAs("MinSteps")] [SerializeField] private int minSteps = 500;
    [FormerlySerializedAs("MaxSteps")] [SerializeField] private int maxSteps = 1000;
    private int Steps = -1;

    [FormerlySerializedAs("MaxTurns")] [SerializeField] private int maxTurns = 1000;
    [FormerlySerializedAs("TurnX")] [SerializeField] private float turnX = 0.001f;
    private float TurnY = 0f;
    [FormerlySerializedAs("TurnZ")] [SerializeField] private float turnZ = 0.001f;
    private int Turns = 0;
    private Vector3 TurnAmount = new(0, 0, 0);

    private readonly int ReturnRadius = 33;
    private readonly int SpawnBase = -90;
    private readonly int SpawnHeight = 50;

    private bool IsColliding = false;

    public void Start()
    {
        if (_debug)
        {
            speed = 0.075f;
        }
    }

    public void FixedUpdate()
    {
        if (SpawnControl.IsFreefall)
        {
            return;
        }
        Vector3 loc = gameObject.transform.position;
        if (_debug)
        {
            UpdateDebug();
        }
        else if (!IsHostile && !IsLocationWithinHole(loc))
        {
            gameObject.transform.Rotate(new(0f, -0.15f, 0f));
        }
        else if (IsHostile)
        {
            UpdateHostile();
        }
        else
        {
            UpdatePeaceful();
        }
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * speed;
        if (IsColliding && IsHostile)
        {
            PlayerHealth healthManager = player.GetComponent<PlayerHealth>();
            healthManager.ChangeOxygen(-attack);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.Equals(player))
        {
            IsColliding = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.Equals(player))
        {
            IsColliding = false;
        }
    }

    private void UpdateDebug()
    {
        float locX = gameObject.transform.position.x;
        float locY = gameObject.transform.position.y;
        float locZ = gameObject.transform.position.z;

        float forwardX = locX - player.transform.position.x;
        float forwardY = locY - player.transform.position.y;
        float forwardZ = locZ - player.transform.position.z;

        gameObject.transform.forward = new(forwardX, forwardY, forwardZ);
        gameObject.transform.forward.Normalize();
    }

    private void UpdatePeaceful()
    {
        if (Steps == -1)
        {
            Steps = Random.Range(minSteps, maxSteps);
        }

        if (Steps == 0 && Turns == 0)
        {
            Turns = -1;
            UpdatePeacefulTurn();
            return;
        }

        if (Turns != 0)
        {
            UpdatePeacefulTurn();
            return;
        }

        UpdatePeacefulMove();
    }

    private void UpdatePeacefulMove()
    {
        --Steps;
    }

    private void UpdatePeacefulTurn()
    {
        if (Turns == -1)
        {
            float turnX = Random.Range(-this.turnX, this.turnX);
            float turnY = Random.Range(-TurnY, TurnY);
            float turnZ = Random.Range(-this.turnZ, this.turnZ);

            TurnAmount = new(turnX, turnY, turnZ);
            Turns = Random.Range(0, maxTurns);
        }

        if (Turns == 0)
        {
            Steps = Random.Range(minSteps, maxSteps);
        }

        gameObject.transform.forward = gameObject.transform.forward + TurnAmount;
        gameObject.transform.forward.Normalize();
        --Turns;
    }

    private void UpdateHostile()
    {
        float locX = gameObject.transform.position.x;
        float locY = gameObject.transform.position.y;
        float locZ = gameObject.transform.position.z;

        float forwardX = player.transform.position.x - locX;
        float forwardY = player.transform.position.y - locY;
        float forwardZ = player.transform.position.z - locZ;

        gameObject.transform.forward = new(forwardX, forwardY, forwardZ);
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
