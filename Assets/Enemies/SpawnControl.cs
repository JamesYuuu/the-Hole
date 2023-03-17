using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private int activeSize = 10;
    [SerializeField] private float spawnDistance = 100f;
    public float viewDistance = 150f;

    [SerializeField] private List<GameObject> pooledEnemies = new();
    [SerializeField] private List<GameObject> activeEnemies = new();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            pooledEnemies[i].SetActive(false);
        }
    }

    void Start()
    {
        print("[LOG][SC] Player at " + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z);
    }

    void Update()
    {
        while (CanSpawn())
        {
            GameObject fish = ActivatePooledEnemy();
            TransformPooledEnemy(fish);
        }
        foreach (GameObject enemy in activeEnemies)
        {
            if (FindDistanceToPlayer(enemy) > viewDistance)
            {
                DeactivatePoolEnemy(enemy);
            }
        }
    }

    public GameObject ActivatePooledEnemy()
    {
        print("[LOG][SC] Activating pooled enemy...");
        int remaining = pooledEnemies.Count;
        if (remaining == 0)
        {
            return null;
        }
        int picked = Random.Range(0, remaining);
        GameObject pickedEnemy = pooledEnemies[picked];

        pooledEnemies.Remove(pickedEnemy);
        activeEnemies.Add(pickedEnemy);
        pickedEnemy.SetActive(true);

        print("[LOG][SC] Activated pooled enemy. Current active enemies: " + activeEnemies.Count);
        return pickedEnemy;
    }

    public void DeactivatePoolEnemy(GameObject activeEnemy)
    {
        print("[LOG][SC] Deactivating pooled enemy...");
        activeEnemy.SetActive(false);
        activeEnemies.Remove(activeEnemy);
        pooledEnemies.Add(activeEnemy);
        print("[LOG][SC] Deactivated pooled enemy. Current active enemies: " + activeEnemies.Count);
    }

    public bool CanSpawn()
    {
        int remaining = pooledEnemies.Count;
        if (remaining == 0)
        {
            return false;
        }
        int active = activeEnemies.Count;
        return active < activeSize;
    }

    private void TransformPooledEnemy(GameObject activeEnemy)
    {
        float randX = Random.Range(0f, 1f);
        float randY = Random.Range(0f, 1f);
        float randZ = Random.Range(0f, 1f);

        float randDist = FindDistance(randX, randY, randZ);

        float scale = (viewDistance - spawnDistance) / randDist;

        bool flipX = Random.Range(0, 2) == 0;
        bool flipY = Random.Range(0, 2) == 0;
        bool flipZ = Random.Range(0, 2) == 0;

        float dispX = randX * scale;
        float dispY = randY * scale;
        float dispZ = randZ * scale;

        if (flipX) dispX = -dispX;
        if (flipY) dispY = -dispY;
        if (flipZ) dispZ = -dispZ;

        float locX = player.transform.position.x + dispX;
        float locY = player.transform.position.y + dispY;
        float locZ = player.transform.position.z + dispZ;

        Vector3 loc = new(locX, locY, locZ);

        float forwardX = player.transform.position.x - locX;
        float forwardY = player.transform.position.y - locY;
        float forwardZ = player.transform.position.z - locZ;

        Vector3 forward = new(forwardX, forwardY, forwardZ);
        forward.Normalize();

        print("Spawning fish at " + locX + "," + locY + "," + locZ);

        activeEnemy.transform.position = loc;
        activeEnemy.transform.forward = forward;
    }
    private float FindDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    public float FindDistanceToPlayer(GameObject fish)
    {
        float dispX = fish.transform.position.x - player.transform.position.x;
        float dispY = fish.transform.position.y - player.transform.position.y;
        float dispZ = fish.transform.position.z - player.transform.position.z;
        return FindDistance(dispX, dispY, dispZ);
    }
}
