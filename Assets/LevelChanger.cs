using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public string levelToLoad;
    public float transitionTime = 1f;
    public Animator animator;

    public void LoadNextLevel() 
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() 
    {
        SpawnControl.ResetScene();
        // Play Animation
        // animator.SetTrigger("Fade_Out");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelToLoad);
    }
}
