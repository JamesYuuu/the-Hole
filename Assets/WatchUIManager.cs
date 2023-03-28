using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class WatchUIManager : MonoBehaviour
{
    private int Oxygen;
    [FormerlySerializedAs("Display")] public TextMeshProUGUI display;
    // Start is called before the first frame update
    void Start()
    {
        this.Oxygen = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScore(int num)
    {
        this.Oxygen = num;
        this.display.text = this.Oxygen.ToString("0");
    }
}
