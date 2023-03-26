using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ResurfacePlayer : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    public float Speed1 = 10;
    public float Speed2 = 20;
    public float Speed3 = 20;

    public int shootingSceneNum = 2;
    private float speed;
    private int triggerNum = 0;
    private bool isSurfacing = false;
    private GameObject player;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private GrappleController grappleController;

    // Start is called before the first frame update
    //void start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (isSurfacing && UnderwaterAI.IsHostile == true)
        {
            if (UnderwaterAI.IsHostile == true)
            {
                teleport_to_shooting();
            }
                // resupply oxygen

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        player = collision.gameObject;
        if (isSurfacing == false)
        {
            if (player.CompareTag("Player"))
            {
                if (triggerNum == 0)
                {
                    triggerNum ++;
                }
                else if(UnderwaterAI.IsHostile == true)
                {
                    isSurfacing = true;
                    speed = Speed1;
                    targetPosition = target1.transform.position;
                    rb = player.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    grappleController = player.GetComponent<GrappleController>();
                    grappleController.SetGrappleActive(Grappleable.Hand.Left, false);
                    grappleController.SetGrappleActive(Grappleable.Hand.Right, false);


                }
                else
                {
                    triggerNum = 0;
                    // resupply oxygen
                }
            }
        }
        else
        {
            if (player.CompareTag("Player"))
            {
                SpawnControl.LoadFreeFall();
            }
        }
    }

    void teleport_to_shooting()
    {
        if (player.transform.position == target1.transform.position)
        {
            targetPosition = target2.transform.position;
            speed = Speed2;
        }
        else if (player.transform.position == target2.transform.position)
        {
            targetPosition = target3.transform.position;
            speed = Speed3;
        }
        else if (player.transform.position == target3.transform.position)
        {
            SceneManager.LoadSceneAsync(shootingSceneNum, LoadSceneMode.Additive);
            isSurfacing = false;
            rb.useGravity = true;
            speed = Speed1;
            triggerNum = 0;
        }
        else
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
