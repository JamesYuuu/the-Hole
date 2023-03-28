using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Alpha : MonoBehaviour
{
    [SerializeField] private Material myMaterial;
    [SerializeField] private Renderer myModel;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlphaSlider(float val)
    {
        Color color = myModel.material.color;
        color.a = 1 - val/10;
        // print(color.a);
        myModel.material.color = color;
    }
}
