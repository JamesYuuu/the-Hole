using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public string levelToLoad;
    public float transitionTime = 1f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
