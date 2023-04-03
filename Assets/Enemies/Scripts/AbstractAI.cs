using UnityEngine;

public abstract class AbstractAI : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private int reward = 5;
    [SerializeField] protected float attack = 0.0025f;
    [SerializeField] private new GameObject particleSystem;

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
        PlayerData.AddMoney(reward);
        gameObject.SetActive(false);
        particleSystem.SetActive(true);
        particleSystem.transform.position = gameObject.transform.position;
    }
}