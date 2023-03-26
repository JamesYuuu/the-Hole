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
    private Rigidbody rb;
    private Vector3 targetPosition;
    private GrappleController grappleController;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (isDiving)
        {
            teleport_to_underwater();
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
                rb = player.GetComponent<Rigidbody>();
                rb.useGravity = false;
                grappleController = player.GetComponent<GrappleController>();
                grappleController.SetGrappleActive(Grappleable.Hand.Left, false);
                grappleController.SetGrappleActive(Grappleable.Hand.Right, false);

            }
        }
    }

    void teleport_to_underwater()
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
            rb.useGravity = true;
            speed = runningSpeed;
            grappleController.SetGrappleActive(Grappleable.Hand.Left, true);
            grappleController.SetGrappleActive(Grappleable.Hand.Right, true);
        }
        else
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
