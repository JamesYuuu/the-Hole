using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// pretty much a duplicate of yilun's SceneTransitScript.cs but with a public method
/// and without a start method
/// </summary>
public class SceneTransitAdditive : MonoBehaviour
{
    [Header("ChangeScene() is called by some event.")]
    [SerializeField] private string sceneName;
    private bool _wasLoaded = false;
    
    public void AddScene()
    {
        if (_wasLoaded) return;
        StartCoroutine(AddSceneRoutine());
    }

    IEnumerator AddSceneRoutine()
    {
        AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => loadSceneProcess.isDone);
        _wasLoaded = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
    
    /// <summary>
    /// Can be used by Unity Event trigger functions
    /// </summary>
    /// <param name="nextSceneName"></param>
    public void AddSceneTo(string nextSceneName)
    {
        if (!_wasLoaded) SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        _wasLoaded = true;
    }
}
