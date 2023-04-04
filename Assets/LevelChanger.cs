using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public string levelToLoad;
    public float transitionTime = 1f;
    public Animator animator;

    // TODO: REMOVE THIS FUNCTION, it is very expensive
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel() 
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() 
    {
        // TODO: Call this when loading shooting scene
        // SpawnControl.ChangeScene();
        // TODO: Call this when loading underwater scene
        SpawnControl.ResetScene();
        // Play Animation
        animator.SetTrigger("Fade_Out");
        // Wait
        yield return new WaitForSeconds(transitionTime);
        // Load Scene
        SceneManager.LoadScene(levelToLoad);
    }
}
