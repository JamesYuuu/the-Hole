using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int PoolSize = 20;
    [SerializeField] private int ActiveSize = 10;
    [SerializeField] private float SpawnDistance = 150f;
    [SerializeField] private float viewDistance = 175f;
    [SerializeField] private int DespawnTime = 1000;

    /** Spawn volume settings */
    private int SpawnRadius = 33;
    private int SpawnBase = 0;
    private int SpawnHeight = 100;

    [SerializeField] private List<GameObject> PooledEnemies = new();
    [SerializeField] private List<GameObject> ActiveEnemies = new();
    [SerializeField] private Dictionary<GameObject, int> DespawnTimes = new();

    void Awake()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            PooledEnemies[i].SetActive(false);
        }
    }

    void Start()
    {
        print("[LOG][SC] Player at " + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z);
    }

    void Update()
    {
        // make it so that each update cycle only spawns 1 fish
        if (CanSpawn())
        {
            ActivatePooledEnemy();
        }
        foreach (GameObject enemy in ActiveEnemies)
        {
            if (FindDistanceToPlayer(enemy) <= viewDistance)
            {
                DespawnTimes[enemy] = -1;
                continue;
            }
            if (DespawnTimes[enemy] == 0)
            {
                DeactivatePoolEnemy(enemy);
                continue;
            }
            if (DespawnTimes[enemy] == -1)
            {
                StartDespawnTimer(enemy);
                continue;
            }
            TickDespawnTimer(enemy);
        }
    }

    private GameObject ActivatePooledEnemy()
    {
        print("[LOG][SC] Activating pooled enemy...");
        int remaining = PooledEnemies.Count;
        if (remaining == 0)
        {
            return null;
        }
        int picked = Random.Range(0, remaining);
        GameObject pickedEnemy = PooledEnemies[picked];

        Vector3 loc = TransformPooledEnemyLoc(pickedEnemy);

        if (!IsLocationWithinHole(loc))
        {
            print("[LOG][SC] Transformed enemy location out of spawn volume. Cancelling...");
            return null;
        }

        TransformPooledEnemyRot(pickedEnemy, loc);

        PooledEnemies.Remove(pickedEnemy);
        ActiveEnemies.Add(pickedEnemy);
        DespawnTimes.Add(pickedEnemy, -1);
        pickedEnemy.SetActive(true);

        print("[LOG][SC] Activated pooled enemy. Current active enemies: " + ActiveEnemies.Count);
        return pickedEnemy;
    }

    private void DeactivatePoolEnemy(GameObject activeEnemy)
    {
        print("[LOG][SC] Deactivating pooled enemy...");
        activeEnemy.SetActive(false);
        ActiveEnemies.Remove(activeEnemy);
        DespawnTimes.Remove(activeEnemy);
        PooledEnemies.Add(activeEnemy);
        print("[LOG][SC] Deactivated pooled enemy. Current active enemies: " + ActiveEnemies.Count);
    }

    private void StartDespawnTimer(GameObject activeEnemy)
    {
        print("[LOG][SC] Pooled enemy out of range. Starting despawn timer...");
        DespawnTimes[activeEnemy] = DespawnTime;
    }

    private void TickDespawnTimer(GameObject activeEnemy)
    {
        --DespawnTimes[activeEnemy];
    }

    private bool CanSpawn()
    {
        int remaining = PooledEnemies.Count;
        if (remaining == 0)
        {
            return false;
        }
        int active = ActiveEnemies.Count;
        return active < ActiveSize;
    }

    private Vector3 TransformPooledEnemyLoc(GameObject activeEnemy)
    {
        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        float randDist = FindDistance(randX, randY, randZ);

        float scale = (viewDistance - SpawnDistance) / randDist;

        float dispX = randX * scale;
        float dispY = randY * scale;
        float dispZ = randZ * scale;

        float locX = player.transform.position.x + dispX;
        float locY = player.transform.position.y + dispY;
        float locZ = player.transform.position.z + dispZ;

        Vector3 loc = new(locX, locY, locZ);

        activeEnemy.transform.position = loc;

        print("[LOG][SC] Transformed pooled enemy location: " + locX + "," + locY + "," + locZ);
        return loc;
    }

    private Vector3 TransformPooledEnemyRot(GameObject activeEnemy, Vector3 loc)
    {
        float forwardX = player.transform.position.x - loc.x;
        float forwardY = 0;
        float forwardZ = player.transform.position.z - loc.z;

        Vector3 forward = new(forwardX, forwardY, forwardZ);
        forward.Normalize();

        activeEnemy.transform.forward = forward;

        print("[LOG][SC] Transformed pooled enemy rotation: " + forwardX + "," + forwardZ);
        return forward;
    }

    private bool IsLocationWithinHole(Vector3 loc)
    {
        if (FindDistance(loc.x, 0, loc.z) > SpawnRadius)
        {
            return false;
        }
        if (loc.y < SpawnBase || loc.y > SpawnBase + SpawnHeight)
        {
            return false;
        }
        return true;
    }

    private float FindDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    private float FindDistanceToPlayer(GameObject fish)
    {
        float dispX = fish.transform.position.x - player.transform.position.x;
        float dispY = fish.transform.position.y - player.transform.position.y;
        float dispZ = fish.transform.position.z - player.transform.position.z;
        return FindDistance(dispX, dispY, dispZ);
    }
}
