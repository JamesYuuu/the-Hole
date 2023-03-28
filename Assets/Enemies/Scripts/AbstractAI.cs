using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractAI : MonoBehaviour
{
    [FormerlySerializedAs("Health")] [SerializeField] private int health = 20;
    [FormerlySerializedAs("Reward")] [SerializeField] private int reward = 5;
    [FormerlySerializedAs("Attack")] [SerializeField] protected float attack = 0.0025f;

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
    }
}
