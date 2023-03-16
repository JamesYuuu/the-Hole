using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public RectTransform prefab;
    private RectTransform waypoint;
    private Transform player;
    private Text distance;

    private Vector3 offset = new Vector3(0, 1.25f, 0);

    // Start is called before the first frame update
    void Start()
    {
        var canvas = GameObject.Find("Waypoint_UI").transform;
        waypoint = Instantiate(prefab, canvas);
        distance = waypoint.GetComponentInChildren<Text>();
        print(distance);
        player = GameObject.Find("Main Camera").transform;

    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
        waypoint.position = screenPos;

        waypoint.gameObject.SetActive(screenPos.z > 0);
        
        distance.text = Vector3.Distance(player.position, transform.position).ToString("0") + " m";
    }
}
