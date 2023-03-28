using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnControl : MonoBehaviour
{
    private static SpawnControl _instance;
    [SerializeField] private static readonly bool Debug = false;

    [SerializeField] private GameObject player;
    [FormerlySerializedAs("PoolSize")] [SerializeField] private int poolSize = 20;
    [FormerlySerializedAs("ActiveSize")] [SerializeField] private int activeSize = 10;
    [FormerlySerializedAs("SpawnDistance")] [SerializeField] private float spawnDistance = 150f;
    [SerializeField] private float viewDistance = 175f;
    [FormerlySerializedAs("DespawnTime")] [SerializeField] private int despawnTime = 100;

    /**
     * Spawn volume settings
     */
    private readonly int SpawnRadius = 33;

    private readonly int SpawnBase = -90;
    private readonly int SpawnHeight = 50;
    private static readonly int FreefallBase = 90;
    private static readonly int FreefallHeight = 120;

    [FormerlySerializedAs("PooledEnemies")] [SerializeField] private List<GameObject> pooledEnemies = new();
    [FormerlySerializedAs("ActiveEnemies")] [SerializeField] private List<GameObject> activeEnemies = new();
    [SerializeField] private readonly Dictionary<GameObject, int> DespawnTimes = new();

    private static readonly List<Vector3> TransferActivePosition = new();
    public static bool IsFreefall;
    private static bool _isFreefalled;

    public static void LoadFreeFall()
    {
        if (Debug) print("[LOG][SC] Changing scenes, saving active fish positions...");
        IsFreefall = true;
        if (!_instance) return;

        print("Transfer Count:" + _instance.activeEnemies.Count);

        foreach (var fish in _instance.activeEnemies) TransferActivePosition.Add(fish.transform.position);
    }

    public static void ResetScene()
    {
        if (Debug) print("[LOG][SC] Resetting scene...");
        IsFreefall = false;
    }

    private static void ChangeSceneTransform()
    {
        if (Debug) print("[LOG][SC] Scene changed. Respawning fish for shooting...");
        print("Transferred Count:" + TransferActivePosition.Count);
        for (var i = 0; i < TransferActivePosition.Count; i++)
        {
            var fish = _instance.pooledEnemies[i];
            fish.SetActive(true);
            fish.transform.forward = new Vector3(0, 1, 0);
            var newY = Random.Range(FreefallBase, FreefallHeight);
            fish.transform.position = new Vector3(TransferActivePosition[i].x, newY, TransferActivePosition[i].z);
        }
    }

    private void Awake()
    {
        for (var i = 0; i < poolSize; i++) pooledEnemies[i].SetActive(false);
    }

    private void Start()
    {
        if (_instance == null) _instance = this;
        if (Debug)
            print("[LOG][SC] Player at " + player.transform.position.x + "," + player.transform.position.y + "," +
                  player.transform.position.z);
    }

    private void Update()
    {
        if (IsFreefall)
        {
            if (!_isFreefalled)
            {
                print("BRUH");
                LoadFreeFall();
                ChangeSceneTransform();
                _isFreefalled = true;
            }

            return;
        }

        // make it so that each update cycle only spawns 1 fish
        if (CanSpawn()) ActivatePooledEnemy();
        List<GameObject> toDeactivate = new();
        foreach (var enemy in activeEnemies)
        {
            if (FindDistanceToPlayer(enemy) <= viewDistance)
            {
                DespawnTimes[enemy] = -1;
                continue;
            }

            if (DespawnTimes[enemy] == 0)
            {
                toDeactivate.Add(enemy);
                continue;
            }

            if (DespawnTimes[enemy] == -1)
            {
                StartDespawnTimer(enemy);
                continue;
            }

            TickDespawnTimer(enemy);
        }

        toDeactivate.ForEach(DeactivatePoolEnemy);
    }

    private GameObject ActivatePooledEnemy()
    {
        if (Debug) print("[LOG][SC] Activating pooled enemy...");
        var remaining = pooledEnemies.Count;
        if (remaining == 0) return null;
        var picked = Random.Range(0, remaining);
        var pickedEnemy = pooledEnemies[picked];

        var loc = TransformPooledEnemyLoc(pickedEnemy);

        if (!IsLocationWithinHole(loc))
        {
            if (Debug) print("[LOG][SC] Transformed enemy location out of spawn volume. Cancelling...");
            return null;
        }

        TransformPooledEnemyRot(pickedEnemy, loc);

        pooledEnemies.Remove(pickedEnemy);
        activeEnemies.Add(pickedEnemy);
        DespawnTimes.Add(pickedEnemy, -1);
        pickedEnemy.SetActive(true);

        if (Debug) print("[LOG][SC] Activated pooled enemy. Current active enemies: " + activeEnemies.Count);
        return pickedEnemy;
    }

    private void DeactivatePoolEnemy(GameObject activeEnemy)
    {
        if (Debug) print("[LOG][SC] Deactivating pooled enemy...");
        activeEnemy.SetActive(false);
        activeEnemies.Remove(activeEnemy);
        DespawnTimes.Remove(activeEnemy);
        pooledEnemies.Add(activeEnemy);
        if (Debug) print("[LOG][SC] Deactivated pooled enemy. Current active enemies: " + activeEnemies.Count);
    }

    private void StartDespawnTimer(GameObject activeEnemy)
    {
        if (Debug) print("[LOG][SC] Pooled enemy out of range. Starting despawn timer...");
        DespawnTimes[activeEnemy] = despawnTime;
    }

    private void TickDespawnTimer(GameObject activeEnemy)
    {
        --DespawnTimes[activeEnemy];
    }

    private bool CanSpawn()
    {
        var remaining = pooledEnemies.Count;
        if (remaining == 0) return false;
        var active = activeEnemies.Count;
        return active < activeSize;
    }

    private Vector3 TransformPooledEnemyLoc(GameObject activeEnemy)
    {
        var randX = Random.Range(-1f, 1f);
        var randY = Random.Range(-1f, 1f);
        var randZ = Random.Range(-1f, 1f);

        var randDist = FindDistance(randX, randY, randZ);

        var scale = (viewDistance - spawnDistance) / randDist;

        var dispX = randX * scale;
        var dispY = randY * scale;
        var dispZ = randZ * scale;

        var locX = player.transform.position.x + dispX;
        var locY = player.transform.position.y + dispY;
        var locZ = player.transform.position.z + dispZ;

        Vector3 loc = new(locX, locY, locZ);

        activeEnemy.transform.position = loc;

        if (Debug) print("[LOG][SC] Transformed pooled enemy location: " + locX + "," + locY + "," + locZ);
        return loc;
    }

    private Vector3 TransformPooledEnemyRot(GameObject activeEnemy, Vector3 loc)
    {
        var forwardX = player.transform.position.x - loc.x;
        float forwardY = 0;
        var forwardZ = player.transform.position.z - loc.z;

        Vector3 forward = new(forwardX, forwardY, forwardZ);
        forward.Normalize();

        activeEnemy.transform.forward = forward;

        if (Debug) print("[LOG][SC] Transformed pooled enemy rotation: " + forwardX + "," + forwardZ);
        return forward;
    }

    private bool IsLocationWithinHole(Vector3 loc)
    {
        if (FindDistance(loc.x, 0, loc.z) > SpawnRadius) return false;
        if (loc.y < SpawnBase || loc.y > SpawnBase + SpawnHeight) return false;
        return true;
    }

    private float FindDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    private float FindDistanceToPlayer(GameObject fish)
    {
        var dispX = fish.transform.position.x - player.transform.position.x;
        var dispY = fish.transform.position.y - player.transform.position.y;
        var dispZ = fish.transform.position.z - player.transform.position.z;
        return FindDistance(dispX, dispY, dispZ);
    }
}