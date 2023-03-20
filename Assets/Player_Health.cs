using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player_Health : MonoBehaviour
{
    public float maxOxygen = 100;
    public float currenOxygen;
    public Animator animator;
    public float OxygenDecreasePerSecond;

    // For updating watch display
    public GameObject watch;
    public WatchUIManager watchUIManager;

    // For smoother drowning transistion
    public GameObject drowning_Screen;
    public Change_Alpha change_Alpha;

    private bool inWater;
    private bool drowning;

    // Start is called before the first frame update
    void Start()
    {
        currenOxygen = maxOxygen;
        OxygenDecreasePerSecond = 1f;
        drowning = false;
        watch = GameObject.Find("Watch");
        drowning_Screen = GameObject.Find("Drowning Screen");
    }

    private void OnTriggerStay(Collider other)
    {
        // Decrease Oxygen when colliding with water
        if (other.gameObject.tag == "water")
        {
            print("Under Water");

            currenOxygen -= OxygenDecreasePerSecond * Time.deltaTime;
        }
        // Increase Oxygen 2 times when colliding with air
        if (other.gameObject.tag == "air")
        {
            print("Not Under Water");
            if (currenOxygen < 100)
                currenOxygen += OxygenDecreasePerSecond * 5 * Time.deltaTime;
        }
        
    }

    // Call this function to increase/decrease Oxygen
    public void changeOxygen(float val)
    {
        currenOxygen += val;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Wirst Display
        watch.GetComponent<WatchUIManager>().ChangeScore((int)currenOxygen);
        // print(currenOxygen);
        drowning_Screen.GetComponent<Change_Alpha>().AlphaSlider(10f);
        if (currenOxygen <= 10f)
        {
            // Update Drowning Screen
            drowning_Screen.GetComponent<Change_Alpha>().AlphaSlider(currenOxygen);
            if (currenOxygen < 0f)
            {
                print("Dead");
                SceneManager.LoadScene(1);
                // to-do Add sound if die to shop
            }
        }
    }

}
