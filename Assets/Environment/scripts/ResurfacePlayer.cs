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
    private float speed;
    private int triggerNum = 0;
    private bool isSurfacing = false;
    private GameObject player;
    private Rigidbody rb;
    private Vector3 targetPosition;

    void Update()
    {
        if (isSurfacing)
        {
            if (player.transform.position == target1.transform.position)
            {
                targetPosition = target2.transform.position;
                speed = speed2;
            }
            else if (player.transform.position == target2.transform.position)
            {
                targetPosition = target3.transform.position;
                speed = speed3;
            }
            else if (player.transform.position == target3.transform.position)
            {
                Debug.Log("Ian: Resurface Update target3 triggered");
                SpawnControl.LoadFreeFall();
                SceneManager.LoadSceneAsync(shootingSceneNum, LoadSceneMode.Additive);
                isSurfacing = false;
                rb.useGravity = true;
                speed = speed1;
                triggerNum = 0;
            }
            else
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);
            }
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
                else
                {
                    isSurfacing = true;
                    speed = speed1;
                    targetPosition = target1.transform.position;
                    rb = player.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                }
            }
        }
    }
}