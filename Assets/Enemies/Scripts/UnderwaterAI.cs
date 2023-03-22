using System.Collections.Generic;
using UnityEngine;

public class UnderwaterAI : AbstractAI
{
    /// <summary>
    /// Debug flag. Set to true to force fish to move back to test despawning.
    /// </summary>
    [SerializeField] private static bool Debug = false;
    [SerializeField] private GameObject Player;

    public static bool IsHostile = true;
    [SerializeField] private float Speed = 0.005f;
    [SerializeField] private int MinSteps = 500;
    [SerializeField] private int MaxSteps = 1000;
    private int Steps = -1;

    [SerializeField] private int MaxTurns = 1000;
    [SerializeField] private float TurnX = 0.001f;
    private float TurnY = 0f;
    [SerializeField] private float TurnZ = 0.001f;
    private int Turns = 0;
    private Vector3 TurnAmount = new(0, 0, 0);

    private int ReturnRadius = 35;
    private int SpawnBase = 0;
    private int SpawnHeight = 100;

    private bool IsColliding = false;

    public override void Start()
    {
        if (Debug)
        {
            Speed = 0.075f;
        }
    }

    public override void Update()
    {
        if (SpawnControl.IsFreefall)
        {
            return;
        }
        Vector3 loc = gameObject.transform.position;
        if (Debug)
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
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Speed;
        if (IsColliding)
        {
            Player_Health healthManager = Player.GetComponent<Player_Health>();
            healthManager.changeOxygen(-0.05f);
            print("FUCK YOU");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsColliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsColliding = false;
        }
    }

    private void UpdateDebug()
    {
        float locX = gameObject.transform.position.x;
        float locY = gameObject.transform.position.y;
        float locZ = gameObject.transform.position.z;

        float forwardX = locX - Player.transform.position.x;
        float forwardY = locY - Player.transform.position.y;
        float forwardZ = locZ - Player.transform.position.z;

        gameObject.transform.forward = new(forwardX, forwardY, forwardZ);
        gameObject.transform.forward.Normalize();
    }

    private void UpdatePeaceful()
    {
        if (Steps == -1)
        {
            Steps = Random.Range(MinSteps, MaxSteps);
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
            float turnX = Random.Range(-TurnX, TurnX);
            float turnY = Random.Range(-TurnY, TurnY);
            float turnZ = Random.Range(-TurnZ, TurnZ);

            TurnAmount = new(turnX, turnY, turnZ);
            Turns = Random.Range(0, MaxTurns);
        }

        if (Turns == 0)
        {
            Steps = Random.Range(MinSteps, MaxSteps);
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

        float forwardX = Player.transform.position.x - locX;
        float forwardY = Player.transform.position.y - locY;
        float forwardZ = Player.transform.position.z - locZ;

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
