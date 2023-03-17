using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Changer : MonoBehaviour
{
    public Animator animator;
    public int levelToLoad;
    public float transitionTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Load_Shop();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Load_Hole();
        }
    }

    public void Load_Shop() 
    {
        StartCoroutine(LoadLevel(1));
    }

    public void Load_Hole()
    {
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex) 
    {
        // Play Animation
        animator.SetTrigger("Fade_Out");
        // Wait
        yield return new WaitForSeconds(transitionTime);
        // Load Scen
        SceneManager.LoadScene(levelIndex);
    }
}
