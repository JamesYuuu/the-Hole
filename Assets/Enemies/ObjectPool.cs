using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance = null;

    public int AMOUNT = 20;

    public List<GameObject> pooledObjects = new(20);

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
            for (int i = 0; i < AMOUNT; i++)
            {
                SharedInstance.pooledObjects[i].SetActive(false);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < AMOUNT; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
