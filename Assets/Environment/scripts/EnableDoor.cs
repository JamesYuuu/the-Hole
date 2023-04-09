using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableDoor : MonoBehaviour
{
    public string shootSceneName = "Environment_shooting_tim";
    public void EnableTheDoor()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(shootSceneName));
        FindObjectOfType<DoorTag>().ActivateDoor();
    }
} 