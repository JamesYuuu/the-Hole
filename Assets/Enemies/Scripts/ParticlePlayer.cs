using System.Collections;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;

    public void PlayParticle()
    {
        particleSystem.Play();
        StartCoroutine(StopParticleSystemAfterTime(3f));
    }

    private void StopParticleSystem()
    {
        particleSystem.Clear();
    }

    IEnumerator StopParticleSystemAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        StopParticleSystem();
    }
}