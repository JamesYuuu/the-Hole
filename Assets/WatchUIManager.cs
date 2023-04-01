using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class WatchUIManager : MonoBehaviour
{
    private int _oxygen;
    [FormerlySerializedAs("Display")] public TextMeshProUGUI display;
    // Start is called before the first frame update
    void Start()
    {
        _oxygen = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScore(int num)
    {
        _oxygen = num;
        display.text = _oxygen.ToString("0");
    }
}
