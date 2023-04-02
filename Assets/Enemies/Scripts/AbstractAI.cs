using System.Collections;
using UnityEngine;

public abstract class AbstractAI : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private int reward = 5;
    [SerializeField] protected float attack = 0.0025f;
    [SerializeField] private new ParticleSystem particleSystem;

    public void Damage(int damage)
    {
        print("fish is hit");
        if (damage >= health)
        {
            Die();
            return;
        }

        health -= damage;
    }

    private void Die()
    {
        PlayerData.AddMoney(reward);
        StartParticleSystem();
        gameObject.SetActive(false);
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