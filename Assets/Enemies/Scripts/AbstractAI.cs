using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAI : MonoBehaviour
{
    [SerializeField] private int Health = 20;
    [SerializeField] private int Reward = 5;

    public abstract void Start();

    public abstract void Update();

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
