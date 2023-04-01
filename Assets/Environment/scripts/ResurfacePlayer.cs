using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ResurfacePlayer : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    [FormerlySerializedAs("Speed1")] public float speed1 = 10;
    [FormerlySerializedAs("Speed2")] public float speed2 = 20;
    [FormerlySerializedAs("Speed3")] public float speed3 = 20;

    public int shootingSceneNum = 3;
    private float _speed;
    private int _triggerNum = 0;
    private bool _isSurfacing = false;
    private GameObject _player;
    private Rigidbody _rb;
    private Vector3 _targetPosition;

    void Update()
    {
        if (_isSurfacing)
        {
            if (_player.transform.position == target1.transform.position)
            {
                _targetPosition = target2.transform.position;
                _speed = speed2;
            }
            else if (_player.transform.position == target2.transform.position)
            {
                _targetPosition = target3.transform.position;
                _speed = speed3;
            }
            else if (_player.transform.position == target3.transform.position)
            {
                Debug.Log("Ian: Resurface Update target3 triggered");
                SpawnControl.LoadFreeFall();
                SceneManager.LoadSceneAsync(shootingSceneNum, LoadSceneMode.Additive);
                _isSurfacing = false;
                _rb.useGravity = true;
                _speed = speed1;
                _triggerNum = 0;
            }
            else
            {
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, _targetPosition, _speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        _player = collision.gameObject;
        if (_isSurfacing == false)
        {
            if (_player.CompareTag("Player"))
            {
                if (_triggerNum == 0)
                {
                    _triggerNum ++;
                }
                else
                {
                    _isSurfacing = true;
                    _speed = speed1;
                    _targetPosition = target1.transform.position;
                    _rb = _player.GetComponent<Rigidbody>();
                    _rb.useGravity = false;
                }
            }
        }
    }
}