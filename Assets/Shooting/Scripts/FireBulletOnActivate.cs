using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 10;
    public float maxBulletDist = 10;
    public string[] tagsToHit;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(Fire);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(ActivateEventArgs arg) {
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        Bullet bullet = spawnedBullet.GetComponent<Bullet>();
        bullet.Init(1, spawnPoint.forward, bulletSpeed, maxBulletDist, tagsToHit);
    }
}
