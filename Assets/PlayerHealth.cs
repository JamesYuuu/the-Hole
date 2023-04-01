using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class PlayerHealth : MonoBehaviour
{
    public float maxOxygen;
    public float currentOxygen;
    public Animator animator;
    [FormerlySerializedAs("OxygenDecreasePerSecond")] public float oxygenDecreasePerSecond;

    // For updating watch display
    public GameObject watch;
    public WatchUIManager watchUIManager;

    // For smoother drowning transistion
    [FormerlySerializedAs("drowning_Screen")] public GameObject drowningScreen;
    [FormerlySerializedAs("change_Alpha")] public ChangeAlpha changeAlpha;

    private bool _inWater;
    private bool _drowning;

    // Start is called before the first frame update
    void Start()
    {
        currentOxygen = maxOxygen;
        oxygenDecreasePerSecond = 1f;
        _drowning = false;
        watch = GameObject.Find("Watch");
        drowningScreen = GameObject.Find("Drowning Screen");
    }

    private void OnTriggerStay(Collider other)
    {
        // Decrease Oxygen when colliding with water
        if (other.gameObject.tag == "water")
        {
            // print("Under Water");

            currentOxygen -= oxygenDecreasePerSecond * Time.deltaTime;
            // print(currentOxygen);
        }
        // Increase Oxygen 5 times when colliding with air
        if (other.gameObject.tag == "air")
        {
            print("Not Under Water");
            if (currentOxygen < 100)
                currentOxygen += oxygenDecreasePerSecond * 5 * Time.deltaTime;
        }
        
    }

    // Call this function to increase/decrease Oxygen
    public void ChangeOxygen(float val)
    {
        currentOxygen += val;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Wirst Display
        watch.GetComponent<WatchUIManager>().ChangeScore((int)currentOxygen);
        
        drowningScreen.GetComponent<ChangeAlpha>().AlphaSlider(10f);
        
        if (currentOxygen <= 10f)
        {
            print("lower");
            // Update Drowning Screen
            drowningScreen.GetComponent<ChangeAlpha>().AlphaSlider(currentOxygen);
            if (currentOxygen < 0f)
            {
                print("Dead");
                SceneManager.LoadScene(1);
                // to-do Add sound if die to shop
            }
        }
    }

}
