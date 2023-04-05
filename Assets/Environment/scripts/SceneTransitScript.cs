using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneTransitScript : MonoBehaviour
{
    private Boolean _isLoaded = false;
    public int loadSceneNum = 1;

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

    void Start()
    {
        SceneManager.LoadSceneAsync(loadSceneNum, LoadSceneMode.Additive);
        _isLoaded = true;
    }
}
