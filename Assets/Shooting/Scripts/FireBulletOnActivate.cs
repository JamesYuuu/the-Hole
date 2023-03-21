using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class FireBulletOnActivate : MonoBehaviour
{
    private ObjectPool<Bullet> pool;
    public int defaultObjectPoolSize = 50;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 10;
    public float maxBulletDist = 10;
    public string[] tagsToHit;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        pool = new ObjectPool<Bullet>(CreatePooledBullet, GetBulletFromPool, ReleaseBulletToPool, DestroyPooledBullet, false, defaultObjectPoolSize);
        grabbable.activated.AddListener(FireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBullet(ActivateEventArgs arg) {
        pool.Get();
    }

    Bullet CreatePooledBullet() {
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        return spawnedBullet.GetComponent<Bullet>();
    }

    void GetBulletFromPool(Bullet bullet) {
        bullet.gameObject.SetActive(true);
        bullet.Init(1, spawnPoint.forward, bulletSpeed, maxBulletDist, tagsToHit, ReleaseBullet);
    }

    public void ReleaseBulletToPool(Bullet bullet) {
        bullet.gameObject.SetActive(false);
    }

    void DestroyPooledBullet(Bullet bullet) {
        Destroy(bullet.gameObject);
    }

    void ReleaseBullet(Bullet bullet) {
        pool.Release(bullet);
    }
}
