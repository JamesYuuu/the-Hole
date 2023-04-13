using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitScript : MonoBehaviour
{
    private Boolean _isLoaded = false;
    public int loadSceneNum = 1;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!_isLoaded)
            {
                // SceneManager.LoadSceneAsync("Environment_Underwater", LoadSceneMode.Additive);
                _isLoaded = true;
                SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
            }
        }
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
        _isLoaded = true;
    }
}
