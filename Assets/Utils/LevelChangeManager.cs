using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A messy class that pastes all the scene changing functions together first
/// not in use yet
///
/// Not a singleton. Should be placed on obj in scene and preset which scene to go to next
///
/// Current load scenes:
/// Assets/LevelChanger.cs
// 36:        SceneManager.LoadScene(levelToLoad);
// 
// Assets/PlayerHealth.cs
// 79:                SceneManager.LoadScene(1);
// 
// Assets/Environment/scripts/ResurfacePlayer.cs
// 41:            SceneManager.LoadSceneAsync(shootingSceneNum, LoadSceneMode.Additive);
// 
// Assets/Environment/scripts/SceneTransitScript.cs
// 18:                // SceneManager.LoadSceneAsync("Environment_Underwater", LoadSceneMode.Additive);
// 20:                SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
// 27:        SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
/// </summary>
public class LevelChangeManager : MonoBehaviour
{
    public string levelToLoad;
    [SerializeField] private string sceneName;
    public int loadSceneNum = 1;
    public float transitionTime = 1f;
    public Animator animator;
    
    private bool _isLoaded = false;

    private void Start()
    {
        // AddScene(); // from SceneTransitScript
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        // if (collision.gameObject.GetComponent<XRDirectInteractor>() != null) // TODO: suggested trigger instead
        {
            if (!_isLoaded)
            {
                // SceneManager.LoadSceneAsync("Environment_Underwater", LoadSceneMode.Additive);
                _isLoaded = true;
                SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
            }
        }
    }

    public void LoadNextLevel() 
    {
        StartCoroutine(LoadLevel());
    }

    /// <summary>
    /// from level changer
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Used by tutorial scene to load the shop scene
    /// </summary>
    public void AddScene()
    {
        if (!_isLoaded) SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _isLoaded = true;
    }
    
    /// <summary>
    /// Can be used by Unity Event trigger functions
    /// Can be used by ResurfacePlayer.cs line 41 to load shooting scene
    /// Can be used by tutorial scene
    /// </summary>
    /// <param name="nextSceneName"></param>
    public void AddSceneTo(string nextSceneName)
    {
        if (!_isLoaded) SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        _isLoaded = true;
    }

    /// <summary>
    /// can be called by PlayerHealth when player dies
    /// </summary>
    /// <param name="nextSceneName"></param>
    public void LoadSceneNo(int i)
    {
        if (!_isLoaded) SceneManager.LoadSceneAsync(i); // was no 1 but we should really use strings instead
        _isLoaded = true;
    }
}
