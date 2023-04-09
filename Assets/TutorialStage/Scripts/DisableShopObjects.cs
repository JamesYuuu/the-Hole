using UnityEngine;

/// <summary>
/// Disables every item (in the shop) that has the TutDisableTag script placed on it.
/// Called by Dialog 2 in tutorial scene
/// </summary>
public class DisableShopObjects : MonoBehaviour
{
    public void DisableShop()
    {
        var items = FindObjectsOfType<TutDisableTag>();
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }
    }
}
