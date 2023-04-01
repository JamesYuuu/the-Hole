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
    private bool _hitEverything;
    
    public float travelSpeed;

    private Rigidbody _rb;

    public bool ignoreIFrames = false;

    private Vector3 _direction = Vector3.forward;
    private Vector3 _normDirection;
    private Vector3 _velocity = Vector3.forward;
    private Vector3 _origin;

    private Vector3 _maxTravelPoint;
    // maximum distance before bullet is destroyed
    public float maxDistance = 10;
    
    // action to take to destroy bullet
    private ObjectPool<Bullet> _pool;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _origin = transform.position;
        _normDirection = Vector3.Normalize(_direction);
        _maxTravelPoint = _origin + _normDirection * maxDistance;
        _hitEverything = tagsToHit.Length == 0;
    }

    public void Init(int damage, Vector3 startPosition, Vector3 direction, float travelSpeed, float maxDistance, string[] tagsToHit, ObjectPool<Bullet> objectPool)
    {
        _origin = startPosition;
        transform.position = startPosition;

        this.damage = damage;
        
        _direction = direction;
        
        // recalculate normalized direction
        _normDirection = Vector3.Normalize(_direction);
        
        transform.rotation = Quaternion.LookRotation(direction);

        this.travelSpeed = travelSpeed;
        
        // set bullet velocity
        _velocity = _normDirection * this.travelSpeed;
        _rb.velocity = _velocity;

        this.maxDistance = maxDistance;
        
        // recalculate max travel point
        _maxTravelPoint = _origin + _normDirection * this.maxDistance;

        this.tagsToHit = tagsToHit;
        _hitEverything = tagsToHit.Length == 0;

        _pool = objectPool;
    }
    
    // Update is called once per frame
    void Update()
    {
        // if bullet has reached its maximum travel point, destroy it
        if (Vector3.Distance(_origin, _maxTravelPoint) <= Vector3.Distance(_origin, transform.position))
        {
            _pool.Release(this);
        }
    }

    void FixedUpdate() 
    {
        // move bullet in direction of travel
        _rb.velocity = _velocity;
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Debug.DrawLine(position, position + _normDirection * 10, Color.magenta);
    }

    void OnDisable() 
    {
        _rb.velocity = Vector3.zero;    
    }

    void OnTriggerEnter(Collider other)
    {   
        AbstractAI damageable = null;

        if (_hitEverything) 
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

        _pool.Release(this);
    }
}
