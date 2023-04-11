using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public float currentOxygen;
    private float maxOxygen;
    public Animator animator;
    [FormerlySerializedAs("OxygenDecreasePerSecond")] public float oxygenChangePerSecond = 1;

    // For updating watch display
    public GameObject watch;
    public AudioSource dieSound;
    public WatchUIManager watchUIManager;
    public LevelChangeManager levelChangeManager;

    // For smoother drowning transistion
    [FormerlySerializedAs("drowning_Screen")] public GameObject drowningScreen;
    [FormerlySerializedAs("change_Alpha")] public ChangeAlpha changeAlpha;

    private bool _isInWater;
    private bool _drowning;

    private Image levelImage;

    public static PlayerHealth _Instance;

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // currentOxygen = maxOxygen;
        currentOxygen = PlayerData.MaxOxygen;
        maxOxygen = PlayerData.MaxOxygen;
        oxygenChangePerSecond = 1f;
        _drowning = false;
        watch = GameObject.Find("Watch");
        drowningScreen = GameObject.Find("Drowning Screen");
        levelChangeManager = FindObjectOfType<LevelChangeManager>();

        // Find the Level_Image UI image
        GameObject levelImageGO = GameObject.Find("Level_Image");
        if (levelImageGO != null)
        {
            levelImage = levelImageGO.GetComponent<Image>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Decrease Oxygen when colliding with water
        if (other.gameObject.tag == "water")
        {
            print("Entered Water");
            _isInWater = true;

            return; // so that won't collide with air at the same time
        }

    }
    private void OnTriggerExit(Collider other)
    {
        // Decrease Oxygen when colliding with water
        if (other.gameObject.tag == "water")
        {
            print("Exited Water");
            _isInWater = false;

            return; // so that won't collide with air at the same time
        }

    }

    // Call this function to increase/decrease Oxygen
    public void ChangeOxygen(float val)
    {
        if(currentOxygen + val > PlayerData.MaxOxygen)
        {
            currentOxygen = PlayerData.MaxOxygen;
            return;
        }
        currentOxygen += val;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isInWater)
        {
            ChangeOxygen(-oxygenChangePerSecond * Time.deltaTime);
        }
        else
        {
            ChangeOxygen(oxygenChangePerSecond * 5 * Time.deltaTime);
        }

        // Calculate the percentage of oxygen remaining
        float oxygenPercentage = currentOxygen / maxOxygen;

        // Set the Y-axis scale of the Level_Image UI image based on the oxygen percentage
        if (levelImage != null)
        {
            Vector3 levelImageScale = levelImage.rectTransform.localScale;
            levelImageScale.y = oxygenPercentage;
            levelImage.rectTransform.localScale = levelImageScale;
        }

        // Update Wirst Display
        watch.GetComponent<WatchUIManager>().ChangeScore((int)currentOxygen);
        
        drowningScreen.GetComponent<ChangeAlpha>().AlphaSlider(10f);
        if (currentOxygen <= 10f)
        {
            // Update Drowning Screen
            drowningScreen.GetComponent<ChangeAlpha>().AlphaSlider(currentOxygen);
            if (currentOxygen < 0f)
            {
                levelChangeManager.LoadWaterToShop();
                dieSound.Play();
            }
        }
    }

}
