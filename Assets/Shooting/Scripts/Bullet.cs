using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public int damage = 1;

    public string[] tagsToHit;
    private bool hitEverything;
    
    public float travelSpeed;

    private Rigidbody rb;

    public bool ignoreIFrames = false;

    private Vector3 direction = Vector3.forward;
    private Vector3 normDirection;
    private Vector3 velocity = Vector3.forward;
    private Vector3 origin;

    private Vector3 maxTravelPoint;
    // maximum distance before bullet is destroyed
    public float maxDistance = 10;
    
    // action to take to destroy bullet
    private ObjectPool<Bullet> pool;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        normDirection = Vector3.Normalize(direction);
        maxTravelPoint = origin + normDirection * maxDistance;
        hitEverything = tagsToHit.Length == 0;
    }

    public void Init(int damage, Vector3 startPosition, Vector3 direction, float travelSpeed, float maxDistance, string[] tagsToHit, ObjectPool<Bullet> objectPool)
    {
        origin = startPosition;
        transform.position = startPosition;

        this.damage = damage;
        
        this.direction = direction;
        
        // recalculate normalized direction
        normDirection = Vector3.Normalize(this.direction);
        
        transform.rotation = Quaternion.LookRotation(direction);

        this.travelSpeed = travelSpeed;
        
        // set bullet velocity
        velocity = normDirection * this.travelSpeed;
        rb.velocity = velocity;

        this.maxDistance = maxDistance;
        
        // recalculate max travel point
        maxTravelPoint = origin + normDirection * this.maxDistance;

        this.tagsToHit = tagsToHit;
        hitEverything = tagsToHit.Length == 0;

        pool = objectPool;
    }
    
    // Update is called once per frame
    void Update()
    {
        // if bullet has reached its maximum travel point, destroy it
        if (Vector3.Distance(origin, maxTravelPoint) <= Vector3.Distance(origin, transform.position))
        {
            pool.Release(this);
        }
    }

    void FixedUpdate() 
    {
        // move bullet in direction of travel
        rb.velocity = velocity;
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Debug.DrawLine(position, position + normDirection * 10, Color.magenta);
    }

    void OnDisable() 
    {
        rb.velocity = Vector3.zero;    
    }

    void OnTriggerEnter(Collider other)
    {   
        AbstractAI damageable = null;

        if (hitEverything) 
        {
            damageable = other.gameObject.GetComponent<AbstractAI>();
        }
        else
        {
            foreach (string t in tagsToHit)
            {
                if (other.CompareTag(t))
                {
                    damageable = other.gameObject.GetComponent<AbstractAI>();
                }
            }
        }

        if (damageable != null)
        {
            damageable.Damage(damage);
        }

        pool.Release(this);
    }
}
