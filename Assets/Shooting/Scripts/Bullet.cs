using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    public string[] tagsToHit;
    private bool hitEverything;
    
    public float travelSpeed;

    public bool ignoreIFrames = false;

    private Vector3 _direction = Vector3.forward;
    private Vector3 _normDirection;

    private Vector3 _origin;

    private Vector3 _maxTravelPoint;
    // maximum distance before bullet is destroyed
    public float maxDistance = 10;
    
    // action to take to destroy bullet
    private Action<Bullet> _destroyAction;

    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.position;
        _normDirection = Vector3.Normalize(_direction);
        _maxTravelPoint = _origin + _normDirection * maxDistance;
        hitEverything = tagsToHit.Length == 0;
        _destroyAction = Destroy;
    }

    public void Init(int damage, Vector3 direction, float travelSpeed, float maxDistance, string[] tagsToHit, Action<Bullet> destroyAction)
    {
        _origin = transform.position;

        this.damage = damage;
        
        this._direction = direction;
        
        // recalculate normalized direction
        _normDirection = Vector3.Normalize(this._direction);
        
        this.travelSpeed = travelSpeed;
        
        this.maxDistance = maxDistance;
        
        // recalculate max travel point
        _maxTravelPoint = _origin + _normDirection * this.maxDistance;

        this.tagsToHit = tagsToHit;
        hitEverything = tagsToHit.Length == 0;

        _destroyAction = destroyAction;
    }
    
    // Update is called once per frame
    void Update()
    {
        // move bullet in direction of travel
        transform.position += travelSpeed * Time.deltaTime * _normDirection;
        
        // if bullet has reached its maximum travel point, destroy it
        if (Vector3.Distance(_origin, _maxTravelPoint) <= Vector3.Distance(_origin, transform.position))
        {
            _destroyAction(this);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Debug.DrawLine(position, position + _normDirection * 10, Color.magenta);
    }

    void OnTriggerEnter(Collider other)
    {   
        FreefallAI damageable;

        if (hitEverything) 
        {
            damageable = other.gameObject.GetComponent(typeof(FreefallAI));
        }
        else
        {
            foreach (string t in tagsToHit)
            {
                if (other.CompareTag(t))
                {
                    damageable = other.gameObject.GetComponent(typeof(FreefallAI));
                }
            }
        }

        if (damageable != null)
        {
            damageable.Damage(damage);
        }

        _destroyAction(this);
    }
}
