using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Waypoint_System : MonoBehaviour
{
    public GameObject vrCamera;
    public GameObject waypointPrefab;
    public Vector3 waypointPosition;

    private GameObject waypointInstance;
    private Canvas waypointCanvas;
    private RectTransform waypointRectTransform;

    void Start()
    {
        InitializeWaypoint();
    }

    void Update()
    {
        UpdateWaypointPosition();
        FaceWaypointToCamera();
    }

    private void InitializeWaypoint()
    {
        // Instantiate the waypoint prefab and parent it to the current object
        waypointInstance = Instantiate(waypointPrefab, waypointPosition, Quaternion.identity, transform);

        // Set the position of the waypoint
        waypointInstance.transform.position = waypointPosition;

        // Get the RectTransform and Canvas components
        waypointRectTransform = waypointInstance.GetComponent<RectTransform>();
        waypointCanvas = waypointInstance.GetComponent<Canvas>();

        // Set the canvas render mode to World Space
        waypointCanvas.renderMode = RenderMode.WorldSpace;

        // Set the canvas' world camera
        waypointCanvas.worldCamera = vrCamera.GetComponent<Camera>();
    }

    private void UpdateWaypointPosition()
    {
        // Update the waypoint's position if it has changed
        if (waypointInstance.transform.position != waypointPosition)
        {
            waypointInstance.transform.position = waypointPosition;
        }
    }

    private void FaceWaypointToCamera()
    {
        // Calculate the direction vector from the waypoint to the camera
        Vector3 directionToCamera = (vrCamera.transform.position - waypointInstance.transform.position).normalized;

        // Calculate the rotation required to face the camera
        Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);

        // Smoothly rotate the waypoint to face the camera
        waypointRectTransform.rotation = Quaternion.Slerp(waypointRectTransform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
