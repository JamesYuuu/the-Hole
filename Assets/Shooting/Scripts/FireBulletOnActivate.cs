using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class FireBulletOnActivate : MonoBehaviour
{
    private ObjectPool<Bullet> pool;
    private AudioSource audioSource;

    public int defaultObjectPoolSize = 50;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public int bulletDamage = 20;
    public float bulletSpeed = 10;
    public float maxBulletDist = 10;
    public string[] tagsToHit;
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        pool = new ObjectPool<Bullet>(CreatePooledBullet, GetBulletFromPool, ReleaseBulletToPool, DestroyPooledBullet, false, defaultObjectPoolSize);
        grabbable.activated.AddListener(FireBullet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBullet(ActivateEventArgs arg) {
        audioSource.PlayOneShot(audioSource.clip);
        pool.Get();
    }

    Bullet CreatePooledBullet() {
        GameObject spawnedBullet = Instantiate(bulletPrefab);
        return spawnedBullet.GetComponent<Bullet>();
    }

    void GetBulletFromPool(Bullet bullet) {
        bullet.gameObject.SetActive(true);
        bullet.Init(bulletDamage, spawnPoint.position, spawnPoint.forward, bulletSpeed, maxBulletDist, tagsToHit, this.pool);
    }

    void ReleaseBulletToPool(Bullet bullet) {
        bullet.gameObject.SetActive(false);
    }

    void DestroyPooledBullet(Bullet bullet) {
        Destroy(bullet.gameObject);
    }
}
