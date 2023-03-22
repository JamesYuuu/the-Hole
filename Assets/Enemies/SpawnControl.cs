using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    private static SpawnControl Instance = null;
    [SerializeField] private static bool Debug = false;

    [SerializeField] private GameObject player;
    [SerializeField] private int PoolSize = 20;
    [SerializeField] private int ActiveSize = 10;
    [SerializeField] private float SpawnDistance = 150f;
    [SerializeField] private float viewDistance = 175f;
    [SerializeField] private int DespawnTime = 100;

    /** Spawn volume settings */
    private readonly int SpawnRadius = 33;
    private readonly int SpawnBase = -90;
    private readonly int SpawnHeight = 50;
    private static readonly int FreefallBase = 90;
    private static readonly int FreefallHeight = 120;

    [SerializeField] private List<GameObject> PooledEnemies = new();
    [SerializeField] private List<GameObject> ActiveEnemies = new();
    [SerializeField] private Dictionary<GameObject, int> DespawnTimes = new();

    private static readonly List<Vector3> TransferActivePosition = new();
    public static bool IsFreefall = false;

    public static void ChangeScene()
    {
        if (Debug)
        {
            print("[LOG][SC] Changing scenes, saving active fish positions...");
        }
        IsFreefall = true;
        foreach (GameObject fish in Instance.ActiveEnemies)
        {
            TransferActivePosition.Add(fish.transform.position);
        }
    }
    
    private static void ChangeSceneTransform()
    {
        if (Debug)
        {
            print("[LOG][SC] Scene changed. Respawning fish for shooting...");
        }
        for (int i = 0; i < Instance.ActiveSize; i++)
        {
            GameObject fish = Instance.PooledEnemies[i];
            fish.SetActive(true);
            fish.transform.position = TransferActivePosition[i];
            fish.transform.forward = new(0, 1, 0);
            int newY = Random.Range(FreefallBase, FreefallHeight);
            fish.transform.position = new(fish.transform.position.x, newY, fish.transform.position.z);
        }
    }

    void Awake()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            PooledEnemies[i].SetActive(false);
        }
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (IsFreefall)
        {
            Instance = this;
            ChangeSceneTransform();
        }
        if (Debug)
        {
            print("[LOG][SC] Player at " + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z);
        }
    }

    void Update()
    {
        if (IsFreefall)
        {
            return;
        }
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
        if (Debug)
        {
            print("[LOG][SC] Activating pooled enemy...");
        }
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
            if (Debug)
            {
                print("[LOG][SC] Transformed enemy location out of spawn volume. Cancelling...");
            }
            return null;
        }

        TransformPooledEnemyRot(pickedEnemy, loc);

        PooledEnemies.Remove(pickedEnemy);
        ActiveEnemies.Add(pickedEnemy);
        DespawnTimes.Add(pickedEnemy, -1);
        pickedEnemy.SetActive(true);

        if (Debug)
        {
            print("[LOG][SC] Activated pooled enemy. Current active enemies: " + ActiveEnemies.Count);
        }
        return pickedEnemy;
    }

    private void DeactivatePoolEnemy(GameObject activeEnemy)
    {
        if (Debug)
        {
            print("[LOG][SC] Deactivating pooled enemy...");
        }
        activeEnemy.SetActive(false);
        ActiveEnemies.Remove(activeEnemy);
        DespawnTimes.Remove(activeEnemy);
        PooledEnemies.Add(activeEnemy);
        if (Debug)
        {
            print("[LOG][SC] Deactivated pooled enemy. Current active enemies: " + ActiveEnemies.Count);
        }
    }

    private void StartDespawnTimer(GameObject activeEnemy)
    {
        if (Debug)
        {
            print("[LOG][SC] Pooled enemy out of range. Starting despawn timer...");
        }
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

        if (Debug)
        {
            print("[LOG][SC] Transformed pooled enemy location: " + locX + "," + locY + "," + locZ);
        }
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

        if (Debug)
        {
            print("[LOG][SC] Transformed pooled enemy rotation: " + forwardX + "," + forwardZ);
        }
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
