using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterEffect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 0;
            RenderSettings.fogEndDistance = 100;
            RenderSettings.fogColor = new Vector4(31/255,44/255,79/255,1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) RenderSettings.fog = false;
    }
}
