using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadarPulse : MonoBehaviour {

    [SerializeField] private GameObject rPing;
    [SerializeField] private GameObject rPing_Up;
    [SerializeField] private GameObject rPing_Low;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform radarTransform;
    [SerializeField] private float rangeNow;
    [SerializeField] private float maxRange = 100f;

    private List<Collider> alreadyHit;

    void Awake()
    {
        
        alreadyHit = new List<Collider>();
    }

    void Update()
    {
        float rangeSpeed = maxRange;
        rangeNow += rangeSpeed * Time.deltaTime;

        if (rangeNow > maxRange)
        {
            rangeNow = 0f;
            alreadyHit.Clear();
        }
        radarTransform.localScale = new Vector3(rangeNow, rangeNow);

        Collider[] collidersHitArray = Physics.OverlapSphere(radarTransform.position, rangeNow, LayerMask.GetMask("Treasure"));
        foreach (Collider colliderHit in collidersHitArray)
        {
            if (colliderHit != null)
            {
                if (!alreadyHit.Contains(colliderHit))
                {
                    alreadyHit.Add(colliderHit);
                    
                        //print("find");
                        GameObject pingObj = null;
                        float yDiff = Mathf.Abs(player.transform.position.y - colliderHit.transform.position.y);
                        if (yDiff < 3)
                        {
                            // print("Equal");
                            pingObj = Instantiate(rPing, colliderHit.transform.position, player.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
                            pingObj.transform.SetParent(player.transform);

                        }
                        else if (colliderHit.transform.position.y < player.transform.position.y)
                        {
                            // print("Lower");
                            pingObj = Instantiate(rPing_Low, colliderHit.transform.position, player.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
                            pingObj.transform.SetParent(player.transform);

                        }
                        else if (colliderHit.transform.position.y > player.transform.position.y)
                        {
                            // print("Higher");
                            pingObj = Instantiate(rPing_Up, colliderHit.transform.position, player.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
                            pingObj.transform.SetParent(player.transform);

                        }

                        pingObj.GetComponent<SpriteRenderer>().material.color = Color.white;

                

                    /*
                    if (colliderHit.CompareTag("treasure"))
                    {
                        float yDiff = Mathf.Abs(player.transform.position.y - colliderHit.transform.position.y);
                        if (yDiff < 3)
                        {
                            print("Equal");
                        }
                        else if (colliderHit.transform.position.y < player.transform.position.y)
                        {
                            print("Lower");
                        }
                        else if (colliderHit.transform.position.y > player.transform.position.y)
                        {
                            print("Higher");
                        }
                        Instantiate(rPing, colliderHit.transform.position, Quaternion.Euler(90f, 0f, 0f));
                    }
                    */
                }
            }
        }
    }


}
