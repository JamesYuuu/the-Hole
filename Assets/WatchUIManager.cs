using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WatchUIManager : MonoBehaviour
{
    private int Oxygen;
    public TextMeshProUGUI Display;
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
        this.Display.text = this.Oxygen.ToString("0");
    }
}
