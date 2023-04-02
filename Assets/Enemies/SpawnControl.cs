using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public class SpawnControl : MonoBehaviour
{
    private static SpawnControl _instance;
    private const bool Debug = false;

    [SerializeField] private GameObject player;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private int activeSize = 10;
    [SerializeField] private float spawnDistance = 150f;
    [SerializeField] private float viewDistance = 175f;
    [SerializeField] private int despawnTime = 100;

    /**
     * Spawn volume settings
     */
    private readonly int _spawnRadius = 33;

    private readonly int _spawnBase = -70;
    private readonly int _spawnHeight = 100;
    private static readonly int FreefallBase = 90;
    private static readonly int FreefallHeight = 120;

    [FormerlySerializedAs("PooledEnemies")]
    [SerializeField]
    private List<GameObject> pooledEnemies = new();

    [FormerlySerializedAs("ActiveEnemies")]
    [SerializeField]
    private List<GameObject> activeEnemies = new();

    private readonly Dictionary<GameObject, int> _despawnTimes = new();

    private static readonly List<Vector3> TransferActivePosition = new();
    public static bool IsFreefall;
    private static bool _isFreefalled;

    public static void LoadFreeFall()
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Changing scenes, saving active fish positions...");
#pragma warning restore CS0162
        IsFreefall = true;
        if (!_instance) return;

        // print("Transfer Count:" + _instance.activeEnemies.Count);

        foreach (var fish in _instance.activeEnemies) TransferActivePosition.Add(fish.transform.position);
        List<GameObject> toDeactive = new List<GameObject>();
        foreach (var fish in _instance.activeEnemies) toDeactive.Add(fish);
        foreach (var fish in toDeactive) _instance.DeactivatePoolEnemy(fish);
    }

    public static void ResetScene()
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Resetting scene...");
#pragma warning restore CS0162
        IsFreefall = false;
    }

    private static void ChangeSceneTransform()
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Scene changed. Respawning fish for shooting...");
#pragma warning restore CS0162
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
        // ReSharper disable once InvertIf
        if (Debug)
#pragma warning disable CS0162
        {
            var position = player.transform.position;
            print("[LOG][SC] Player at " + position.x + "," + position.y + "," + position.z);
        }
#pragma warning restore CS0162
    }

    private void Update()
    {
        if (IsFreefall)
        {
            if (!_isFreefalled)
            {
                print("BRUH");
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
                _despawnTimes[enemy] = -1;
                continue;
            }

            switch (_despawnTimes[enemy])
            {
                case 0:
                    toDeactivate.Add(enemy);
                    continue;
                case -1:
                    StartDespawnTimer(enemy);
                    continue;
                default:
                    TickDespawnTimer(enemy);
                    break;
            }
        }

        toDeactivate.ForEach(DeactivatePoolEnemy);
    }

    private void ActivatePooledEnemy()
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Activating pooled enemy...");
#pragma warning restore CS0162
        var remaining = pooledEnemies.Count;
        if (remaining == 0) return;
        var picked = Random.Range(0, remaining);
        var pickedEnemy = pooledEnemies[picked];

        var loc = TransformPooledEnemyLoc(pickedEnemy);

        if (!IsLocationWithinHole(loc))
        {
#pragma warning disable CS0162
            if (Debug) print("[LOG][SC] Transformed enemy location out of spawn volume. Cancelling...");
#pragma warning restore CS0162
            return;
        }

        TransformPooledEnemyRot(pickedEnemy, loc);

        pooledEnemies.Remove(pickedEnemy);
        activeEnemies.Add(pickedEnemy);
        _despawnTimes.Add(pickedEnemy, -1);
        pickedEnemy.SetActive(true);

#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Activated pooled enemy. Current active enemies: " + activeEnemies.Count);
#pragma warning restore CS0162
    }

    private void DeactivatePoolEnemy(GameObject activeEnemy)
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Deactivating pooled enemy...");
#pragma warning restore CS0162
        activeEnemy.SetActive(false);
        activeEnemies.Remove(activeEnemy);
        _despawnTimes.Remove(activeEnemy);
        pooledEnemies.Add(activeEnemy);
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Deactivated pooled enemy. Current active enemies: " + activeEnemies.Count);
#pragma warning restore CS0162
    }

    private void StartDespawnTimer(GameObject activeEnemy)
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Pooled enemy out of range. Starting despawn timer...");
#pragma warning restore CS0162
        _despawnTimes[activeEnemy] = despawnTime;
    }

    private void TickDespawnTimer(GameObject activeEnemy)
    {
        --_despawnTimes[activeEnemy];
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

        var position = player.transform.position;
        var locX = position.x + dispX;
        var locY = position.y + dispY;
        var locZ = position.z + dispZ;

        Vector3 loc = new(locX, locY, locZ);

        activeEnemy.transform.position = loc;

#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Transformed pooled enemy location: " + locX + "," + locY + "," + locZ);
#pragma warning restore CS0162
        return loc;
    }

    private void TransformPooledEnemyRot(GameObject activeEnemy, Vector3 loc)
    {
        var position = player.transform.position;
        var forwardX = position.x - loc.x;
        const float forwardY = 0;
        var forwardZ = position.z - loc.z;

        Vector3 forward = new(forwardX, forwardY, forwardZ);
        forward.Normalize();

        activeEnemy.transform.forward = forward;

#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Transformed pooled enemy rotation: " + forwardX + "," + forwardZ);
#pragma warning restore CS0162
    }

    private bool IsLocationWithinHole(Vector3 loc)
    {
        if (FindDistance(loc.x, 0, loc.z) > _spawnRadius) return false;
        return !(loc.y < _spawnBase) && !(loc.y > _spawnBase + _spawnHeight);
    }

    private static float FindDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    private float FindDistanceToPlayer(GameObject fish)
    {
        var position = fish.transform.position;
        var position1 = player.transform.position;
        var dispX = position.x - position1.x;
        var dispY = position.y - position1.y;
        var dispZ = position.z - position1.z;
        return FindDistance(dispX, dispY, dispZ);
    }
}