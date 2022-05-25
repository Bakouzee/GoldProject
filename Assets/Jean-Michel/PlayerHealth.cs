using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;

    public float invincibilityFlash = 0.2f;

    private bool IsInvincible = false;

    public HealthBar healthBar;

    public SpriteRenderer sprite;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);  
            IsInvincible = true;
            StartCoroutine(InvincibillityFlash());
            StartCoroutine(InvincibillityDelay());

        }
    }

    public IEnumerator InvincibillityFlash()
    {
        while (IsInvincible)
        {
            sprite.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlash);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);

        }
    }
    public IEnumerator BurningFlash()
    {
        while (IsInvincible)
        {
            sprite.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);

        }
    }

    public IEnumerator StinkFlash()
    {
        while (IsInvincible)
        {
            sprite.color = new Color(0.3332994f, 0.76f, 0f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);

        }
    }
    public IEnumerator InvincibillityDelay()
    {
        yield return new WaitForSeconds(2f);
        IsInvincible = false;
    }
}
