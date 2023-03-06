using UnityEngine;
public class SpawnEnemy : MonoBehaviour
{
    private readonly float VIEW_DISTANCE = 10;
    private readonly float[] DESPAWN_LOCATION = { 0, 200, 0 };

    private GameObject player = null;
    private GameObject[] enemies = { };

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("XRRig");
        }
        GameObject fish = ObjectPool.SharedInstance.GetPooledObject();
        while (fish != null)
        {
            SpawnFish(fish);
            fish = ObjectPool.SharedInstance.GetPooledObject();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnFish(GameObject fish)
    {
        float randX = Random.Range(0f, 1f);
        float randY = Random.Range(0f, 1f);
        float randZ = Random.Range(0f, 1f);
        float distance = FindDistance(randX, randY, randZ);

        float factor = VIEW_DISTANCE / distance;
        float dispX = randX * factor;
        float dispY = randY * factor;
        float dispZ = randZ * factor;

        float forwardX = player.transform.position.x - dispX;
        float forwardY = player.transform.position.y - dispY;
        float forwardZ = player.transform.position.z - dispZ;

        fish.transform.position.Set(dispX, dispY, dispZ);
        fish.transform.forward.Set(forwardX, forwardY, forwardZ);
        fish.SetActive(true);
    }

    void DespawnFish(GameObject fish)
    {
        fish.transform.position.Set(DESPAWN_LOCATION[0], DESPAWN_LOCATION[1], DESPAWN_LOCATION[2]);
    }

    private float FindDistance(float x, float y, float z)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }

    private float FindDistanceToPlayer(float x, float y, float z)
    {
        float dispX = player.transform.position.x - x;
        float dispY = player.transform.position.y - y;
        float dispZ = player.transform.position.z - z;
        return FindDistance(dispX, dispY, dispZ);
    }
}
