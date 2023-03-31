using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioClip underwaterAmbience;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "water") 
        {
            audioSource.PlayOneShot(splashSound);
            audioSource.PlayOneShot(underwaterAmbience);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "water") 
        {
            audioSource.Stop();
            audioSource.PlayOneShot(splashSound);
        }
    }
}
