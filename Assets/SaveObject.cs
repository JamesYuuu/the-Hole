using UnityEngine.Serialization;

[System.Serializable]

public class SaveObject
{
    public float maxHealth;
    public int playerGold;
    [FormerlySerializedAs("SavedObject")] public string savedObject;
}
