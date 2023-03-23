using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreefallAI : AbstractAI
{
    private float FallSpeed = 0.05f;

    public void FixedUpdate()
    {
        if (!SpawnControl.IsFreefall)
        {
            return;
        }
        gameObject.transform.position = new(gameObject.transform.position.x, gameObject.transform.position.y - FallSpeed, gameObject.transform.position.z);
    }
}
