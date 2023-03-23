using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAI : MonoBehaviour
{
    [SerializeField] private int Health = 20;
    [SerializeField] private int Reward = 5;
    [SerializeField] protected float Attack = 0.0025f;

    public void Damage(int damage)
    {
        if (damage >= Health)
        {
            Die();
            return;
        }
        Health -= damage;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        PlayerData.AddMoney(Reward);
    }
}
