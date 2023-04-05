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
    private bool _isLoaded = false;
    
    public void AddScene()
    {
        if (!_isLoaded) SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        _isLoaded = true;
    }
    /// <summary>
    /// Can be used by Unity Event trigger functions
    /// </summary>
    /// <param name="nextSceneName"></param>
    public void AddSceneTo(string nextSceneName)
    {
        if (!_isLoaded) SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        _isLoaded = true;
    }
}
