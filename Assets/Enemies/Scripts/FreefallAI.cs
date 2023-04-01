using UnityEngine;

public class FreefallAI : AbstractAI
{
    private const float FallSpeed = 0.05f;

    public void FixedUpdate()
    {
        if (!SpawnControl.IsFreefall)
        {
            return;
        }

        var o = gameObject;
        var position = o.transform.position;
        position = new Vector3(position.x, position.y - FallSpeed, position.z);
        o.transform.position = position;
    }
}
