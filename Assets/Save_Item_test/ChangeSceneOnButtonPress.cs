using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnButtonPress : MonoBehaviour
{
    public string sceneName; // The name of the scene to change to

    private bool isPressed = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isPressed)
            {
                isPressed = true;
                SceneManager.LoadScene(sceneName);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isPressed = false;
        }
    }
}
