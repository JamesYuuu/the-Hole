using System.Collections.Generic;
using UnityEngine;

public class Print_dictionary : MonoBehaviour
{
    [SerializeField] private List<PrefabMapping> prefabMappings;

    private void Start()
    {
        PrintAttachInventory();
    }

    private void PrintAttachInventory()
    {
        foreach (var entry in PlayerData.AttachInventory)
        {
            string attachPointName = entry.Key;
            string prefabName = entry.Value;

            // Find the attach point by name
            GameObject attachPoint = GameObject.Find(attachPointName);

            // Get the position and rotation of the attach point
            Vector3 position = attachPoint.transform.position;
            Quaternion rotation = attachPoint.transform.rotation;

            // Get the prefab by name
            GameObject prefab = Resources.Load<GameObject>(prefabName); // null
            if (prefab == null) return;

            // Instantiate the prefab without a parent
            GameObject newObject = Instantiate(prefab, position, rotation);

            // Set the attachPoint as the parent of the instantiated object
            newObject.transform.SetParent(attachPoint.transform, true);
        }
    }

    private GameObject GetPrefabByName(string prefabName)
    {
        foreach (PrefabMapping mapping in prefabMappings)
        {
            if (mapping.prefabName == prefabName)
            {
                return mapping.prefab;
            }
        }
        return null;
    }

    [System.Serializable]
    public class PrefabMapping
    {
        public string prefabName;
        public GameObject prefab;
    }
}
