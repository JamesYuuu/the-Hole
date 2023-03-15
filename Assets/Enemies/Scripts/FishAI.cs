using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject self;
    [SerializeField] private float speed = 0.0025f;

    private void Update()
    {
        float locX = self.transform.position.x;
        float locY = self.transform.position.y;
        float locZ = self.transform.position.z;

        Vector3 loc = new(locX, locY, locZ);

        float forwardX = player.transform.position.x - locX;
        float forwardY = player.transform.position.y - locY;
        float forwardZ = player.transform.position.z - locZ;

        Vector3 forward = new(forwardX, forwardY, forwardZ);
        forward.Normalize();

        Vector3 next = loc + forward * speed;

        self.transform.forward = forward;
        self.transform.position = next;
    }
}
