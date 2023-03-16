using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 0.0025f;
    /** Debug flag. Set to true to force fish to move back to test despawning. */
    [SerializeField] private static bool debug = true;

    void Start()
    {
        if (debug)
        {
            speed = 0.075f;
        }
    }

    private void Update()
    {
        float locX = gameObject.transform.position.x;
        float locY = gameObject.transform.position.y;
        float locZ = gameObject.transform.position.z;

        Vector3 loc = new(locX, locY, locZ);

        float forwardX = player.transform.position.x - locX;
        float forwardY = player.transform.position.y - locY;
        float forwardZ = player.transform.position.z - locZ;

        Vector3 forward = new(forwardX, forwardY, forwardZ);
        forward.Normalize();

        Vector3 next = loc + forward * speed;
        if (debug)
        {
            next = loc - forward * speed;
        } 

        gameObject.transform.forward = forward;
        gameObject.transform.position = next;
    }
}
