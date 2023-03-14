using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance = null;

    [SerializeField] private static int amount = 20;

    public List<GameObject> pooledObjects = new(amount);
    private List<GameObject> activeObjects = new(amount);

    void Awake()
    {
        if (SharedInstance != null)
        {
            return;
        }
        SharedInstance = this;
        for (int i = 0; i < amount; i++)
        {
            SharedInstance.pooledObjects[i].SetActive(false);
        }
    }

    public GameObject GetPooledObject()
    {
        int remaining = pooledObjects.Count;
        if (remaining == 0)
        {
            return null;
        }
        int picked = Random.Range(0, remaining);
        GameObject pickedObject = pooledObjects[picked];
        pooledObjects.Remove(pickedObject);
        activeObjects.Add(pickedObject);
        return pickedObject;
    }

    public void SetPooledObject(GameObject activeObject)
    {
        activeObjects.Remove(activeObject);
        pooledObjects.Add(activeObject);
    }
}
