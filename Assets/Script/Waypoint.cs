using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public RectTransform prefab;
    private RectTransform _waypoint;
    private Transform _player;
    private Text _distance;

    private Vector3 _offset = new Vector3(0, 1.25f, 0);

    // Start is called before the first frame update
    void Start()
    {
        var canvas = GameObject.Find("Waypoints").transform;
        _waypoint = Instantiate(prefab, canvas);
        _distance = _waypoint.GetComponentInChildren<Text>();
        _player = GameObject.Find("Main Camera").transform;

    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.position + _offset);
        _waypoint.position = screenPos;

        _waypoint.gameObject.SetActive(screenPos.z > 0);

        _distance.text = Vector3.Distance(_player.position, transform.position).ToString("0") + " m";
    }
}
