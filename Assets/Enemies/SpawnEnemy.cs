using UnityEngine;
public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private float spawnDistance = 100f;
    [SerializeField] private float viewDistance = 150f;

    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        print("Player at " + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject fish = ObjectPool.SharedInstance.GetPooledObject();
        while (fish != null)
        {
            SpawnFish(fish);
            fish = ObjectPool.SharedInstance.GetPooledObject();
        }

        foreach (GameObject enemy in ObjectPool.SharedInstance.pooledObjects)
        {
            if (FindDistanceToPlayer(enemy) > viewDistance)
            {
                DespawnFish(enemy);
            }
        }
    }

    void SpawnFish(GameObject fish)
    {
        float randX = Random.Range(0f, 1f);
        float randY = Random.Range(0f, 1f);
        float randZ = Random.Range(0f, 1f);

        float randDist = FindDistance(randX, randY, randZ);

        float scale = (viewDistance - spawnDistance) / randDist;

        float randTotal = randX + randY + randZ;
        float minX = spawnDistance * randX / randTotal;
        float minY = spawnDistance * randY / randTotal;
        float minZ = spawnDistance * randZ / randTotal;

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

        fish.transform.position = loc;
        fish.transform.forward = forward;
        fish.SetActive(true);
    }

    void DespawnFish(GameObject fish)
    {
        fish.SetActive(false);
        ObjectPool.SharedInstance.SetPooledObject(fish);
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
