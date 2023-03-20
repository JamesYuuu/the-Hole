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
    private float speed;
    private bool isDiving = false;
    private GameObject player;
    private Rigidbody rigidbody;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (isDiving)
        {
            if (player.transform.position == target1.transform.position)
            {
                targetPosition = target2.transform.position;
                speed = jumpSpeed;
            }
            else if (player.transform.position == target2.transform.position)
            {
                targetPosition = target3.transform.position;
                speed = fallingSpeed;
            }
            else if (player.transform.position == target3.transform.position)
            {
                isDiving = false;
                rigidbody.useGravity = true;
                speed = runningSpeed;
            }
            else
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (isDiving == false)
        {
            player = collision.gameObject;
            if (player.CompareTag("Player"))
            {
                isDiving = true;
                speed = runningSpeed;
                targetPosition = target1.transform.position;
                rigidbody = player.GetComponent<Rigidbody>();
                rigidbody.useGravity = false;

            }
        }
    }
}
