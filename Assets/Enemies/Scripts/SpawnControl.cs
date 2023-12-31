using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnControl : MonoBehaviour
{
    public UnityEvent doneShooting;
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
    private readonly Vector3 _spawnTransform = new(247, -28f, 45);
    private readonly int _spawnRadius = 200;

    private readonly int _spawnBase = -150;
    private readonly int _spawnHeight = 130;
    private static readonly int FreefallBase = 90;
    private static readonly int FreefallHeight = 120;

    [FormerlySerializedAs("PooledEnemies")] [SerializeField]
    private List<GameObject> pooledEnemies = new();

    [FormerlySerializedAs("ActiveEnemies")] [SerializeField]
    private List<GameObject> activeEnemies = new();

    private readonly Dictionary<GameObject, int> _despawnTimes = new();

    private static readonly List<Vector3> TransferActivePosition = new();
    public static bool IsFreefall;
    private static bool _isFreefalled;
    [SerializeField] private float time = 15.0f;

    public static void LoadFreeFall()
    {
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Changing scenes, saving active fish positions...");
#pragma warning restore CS0162
        IsFreefall = true;
        if (!_instance) return;

        foreach (var fish in _instance.activeEnemies) TransferActivePosition.Add(fish.transform.position);
        List<GameObject> toDeactivate = new List<GameObject>();
        foreach (var fish in _instance.activeEnemies) toDeactivate.Add(fish);
        foreach (var fish in toDeactivate) _instance.DeactivatePoolEnemy(fish);
    }

    public static void ResetScene()
    {
        IsFreefall = false;
        List<GameObject> toDeactivate = new List<GameObject>();
        foreach (var fish in _instance.activeEnemies) toDeactivate.Add(fish);
        foreach (var fish in toDeactivate) _instance.DeactivatePoolEnemy(fish);
#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Resetting scene...");
#pragma warning restore CS0162
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
            TransformFreefallEnemyLoc(fish);
            _instance.activeEnemies.Add(fish);
        }

        _instance.StartFreeFallTimer();
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

    /// <summary>
    /// Used to transform enemy to shooting location upwards
    /// </summary>
    /// <param name="activeEnemy"></param>
    /// <returns></returns>
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

        Vector3 loc = new Vector3(locX, locY, locZ) + _spawnTransform;

        activeEnemy.transform.position = loc;

#pragma warning disable CS0162
        if (Debug) print("[LOG][SC] Transformed pooled enemy location: " + locX + "," + locY + "," + locZ);
#pragma warning restore CS0162
        return loc;
    }

    private static void TransformFreefallEnemyLoc(GameObject freefallEnemy)
    {
        float distFromBoat = 20f;
        float otherSpawnDistance = 10f;
        var randX = Random.Range(-1f, 1f);
        var randY = Random.Range(-1f, 1f);
        var randZ = Random.Range(-1f, 1f);

        var randDist = FindDistance(randX, randY, randZ);

        var scale = (distFromBoat - otherSpawnDistance) / randDist;

        var dispX = randX * scale;
        var dispY = Random.Range(FreefallBase, FreefallHeight);
        var dispZ = randZ * scale;
        

        var position = _instance.player.transform.position;
        var locX = position.x + dispX;
        var locY = position.y + dispY;
        var locZ = position.z + dispZ;

        Vector3 loc = new Vector3(locX, locY, locZ);

        freefallEnemy.transform.position = loc;
        freefallEnemy.transform.forward = new Vector3(0, 1, 0);
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
        loc -= _spawnTransform;
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

    private void StartFreeFallTimer()
    {
        StartCoroutine(StopFreeFallTimerAfterTime());
    }

    private void StopFreeFallTimer()
    {
        for (var i = 0; i < TransferActivePosition.Count; i++)
        {
            _instance.pooledEnemies[i].SetActive(false);
        }

        doneShooting.Invoke();
    }

    IEnumerator StopFreeFallTimerAfterTime()
    {
        yield return new WaitForSeconds(time);
        StopFreeFallTimer();
    }

    public void OnFishDie(GameObject fish)
    {
        _instance.activeEnemies.Remove(fish);
        if (_instance.activeEnemies.Count == 0) doneShooting.Invoke();
    }
}