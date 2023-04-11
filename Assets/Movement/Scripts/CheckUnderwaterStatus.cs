using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CheckUnderwaterStatus : MonoBehaviour
{
    public UnityEvent enteredWater;
    public UnityEvent exitedWater;
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "water") 
        {
            enteredWater?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "water") 
        {
            exitedWater?.Invoke();
        }
    }

}
