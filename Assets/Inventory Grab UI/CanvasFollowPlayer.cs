using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    public Camera playerCamera; // Reference to the player's camera

    // Update is called once per frame
    void Update()
    {
        // Set the canvas's position and rotation to match that of the player's camera
        transform.position = playerCamera.transform.position;
        transform.rotation = playerCamera.transform.rotation;
    }
}
