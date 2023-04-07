using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    public Image imageToBlink;
    public float blinkDuration = 0.1f;
    public int blinkCount = 3;
    public AudioClip warningSound;

    private bool isBlinking = false;

    public void StartBlinking()
    {
        if (isBlinking) return;

        isBlinking = true;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        // Play the warning sound
        if (warningSound != null && audioSource != null)
        {
            audioSource.clip = warningSound;
            audioSource.Play();
        }

        float alpha = 0f;
        float alphaStep = 1f / (blinkDuration * 30f); // 30 updates per second

        for (int i = 0; i < blinkCount; i++)
        {
            // Fade in
            while (alpha < 1f)
            {
                alpha += alphaStep;
                Color blinkColor = imageToBlink.color;
                blinkColor.a = alpha;
                imageToBlink.color = blinkColor;
                yield return new WaitForSeconds(1f / 30f);
            }

            // Fade out
            while (alpha > 0f)
            {
                alpha -= alphaStep;
                Color blinkColor = imageToBlink.color;
                blinkColor.a = alpha;
                imageToBlink.color = blinkColor;
                yield return new WaitForSeconds(1f / 30f);
            }

            alpha = 0f;
        }

        isBlinking = false;
    }
}
