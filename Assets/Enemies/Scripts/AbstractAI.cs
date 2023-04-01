using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractAI : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private int reward = 5;
    [SerializeField] protected float attack = 0.0025f;
    [SerializeField] private ParticleSystem particleSystem;

    public void Damage(int damage)
    {
        if (damage >= health)
        {
            Die();
            return;
        }
        health -= damage;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        PlayerData.AddMoney(reward);
        StartParticleSystem();
    }

    private void StartParticleSystem()
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
