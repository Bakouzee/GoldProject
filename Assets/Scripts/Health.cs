using System;
using UnityEngine;
using AudioController;
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

    /// <summary>Function to apply damage</summary>
    /// <param name="amount">the amount of damage to apply</param>
    /// <returns>is dead</returns>
    public virtual bool TakeDamage(int amount)
    {
        if (dead)
            return false;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public System.Action OnDeath;
    private void Die()
    {
        if (dead)
            return;
        dead = true;
        AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Kill);

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}