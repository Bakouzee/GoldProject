using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthMax = 100;
    
    protected int currentHealth;
    public int CurrentHealth => currentHealth;

    protected bool dead;
    public bool Dead => dead;

    protected virtual void Start()
    {
        currentHealth = healthMax;
    }

    public virtual void TakeDamage(int amount)
    {
        if (dead)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public System.Action OnDeath;
    private void Die()
    {
        if (dead)
            return;
        dead = true;

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}