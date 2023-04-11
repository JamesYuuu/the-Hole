using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioClip underwaterAmbience;

    public void EnterWater() 
    {
        audioSource.PlayOneShot(splashSound);
        audioSource.PlayOneShot(underwaterAmbience);
    }

    public void ExitWater()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(splashSound);
    }
}
