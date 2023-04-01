using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerToHole : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    public float runningSpeed = 10;
    public float jumpSpeed = 20;
    public float fallingSpeed = 20;
    private float _speed;
    private bool _isDiving = false;
    private GameObject _player;
    private Rigidbody _rb;
    private Vector3 _targetPosition;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (_isDiving)
        {
            if (_player.transform.position == target1.transform.position)
            {
                _targetPosition = target2.transform.position;
                _speed = jumpSpeed;
            }
            else if (_player.transform.position == target2.transform.position)
            {
                _targetPosition = target3.transform.position;
                _speed = fallingSpeed;
            }
            else if (_player.transform.position == target3.transform.position)
            {
                _isDiving = false;
                _rb.useGravity = true;
                _speed = runningSpeed;
            }
            else
            {
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, _targetPosition, _speed * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (_isDiving == false)
        {
            _player = collision.gameObject;
            if (_player.CompareTag("Player"))
            {
                _isDiving = true;
                _speed = runningSpeed;
                _targetPosition = target1.transform.position;
                _rb = _player.GetComponent<Rigidbody>();
                _rb.useGravity = false;

            }
        }
    }
}
