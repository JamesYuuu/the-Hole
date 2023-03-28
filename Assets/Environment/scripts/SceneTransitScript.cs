using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitScript : MonoBehaviour
{
    private Boolean isLoaded = false;
    public int loadSceneNum = 1;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isLoaded)
            {
                // SceneManager.LoadSceneAsync("Environment_Underwater", LoadSceneMode.Additive);
                isLoaded = true;
                SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
            }
        }
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
        isLoaded = true;
    }
}
