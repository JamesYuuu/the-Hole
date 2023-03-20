using System.Collections.Generic;
using UnityEngine;

public class UnderwaterAI : MonoBehaviour
{
    /// <summary>
    /// Debug flag. Set to true to force fish to move back to test despawning.
    /// </summary>
    [SerializeField] private static bool Debug = false;
    [SerializeField] private GameObject Player;

    public static bool IsHostile = true;
    [SerializeField] private float Speed = 0.0025f;
    [SerializeField] private int MinSteps = 1000;
    [SerializeField] private int MaxSteps = 2000;
    private int Steps = -1;

    [SerializeField] private int MaxTurns = 2000;
    [SerializeField] private float TurnX = 0.001f;
    [SerializeField] private float TurnY = 0.0001f;
    [SerializeField] private float TurnZ = 0.001f;
    private int Turns = 0;
    private Vector3 TurnAmount = new(0, 0, 0);

    void Start()
    {
        if (Debug)
        {
            Speed = 0.075f;
        }
    }

    private void Update()
    {
        if (IsHostile)
        {
            UpdateHostile();
        } else
        {
            UpdatePeaceful();
        }
        if (Debug)
        {
            UpdateDebug();
        }
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Speed;
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

        if (Steps == 0)
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
}
