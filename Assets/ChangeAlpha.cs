using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlpha : MonoBehaviour
{
    [SerializeField] private Material myMaterial;
    [SerializeField] private Renderer myModel;
    public float fadeDuration = 5;
    // Start is called before the first frame update
    void Start()
    {
        Fade(1, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AlphaSlider(float val)
    {
        Color color = myModel.material.color;
        color.a = 1 - val / 10;
        myModel.material.color = color;
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;

        while (timer <= fadeDuration)
        {
            Color color = myModel.material.color;
            color.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            myModel.material.color = color;

            timer += Time.deltaTime;
            yield return null;
        }
        //color.a = alphaOut;
    }
}

