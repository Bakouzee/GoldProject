using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public PlayerManager PlayerManager { private get; set; }

    public bool IsInOnionZone = false;

    public bool IsInWindowZone = false;

    public float invincibilityFlash = 0.2f;

    public bool IsInvincible = false;

    public System.Action<int> OnHealthUpdated;
    
    public SpriteRenderer sprite;

    private void Update()
    {
        if(currentHealth <= 0)
        {
            Death();
        }
    }
    public void HealPlayer(int healAmount)
    {
        if((currentHealth + healAmount) > healthMax)
        {
            currentHealth = healthMax;
        }
        else
        {
            currentHealth += healAmount;

        }
        OnHealthUpdated?.Invoke(currentHealth);
    }

    void Death()
    {
        // Temporary just for apk
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
            .buildIndex);
        // Time.timeScale = 0;
    }

    public void TakeFireDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            OnHealthUpdated?.Invoke(currentHealth);
            IsInvincible = true;
            StartCoroutine(BurningFlash());
            IsInvincible = false;
            // StartCoroutine(InvincibillityDelay());

        }
    }
    public void TakeStinkDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            OnHealthUpdated?.Invoke(currentHealth);
            IsInvincible = true;
            StartCoroutine(StinkFlash());
            // StartCoroutine(InvincibillityDelay());
            IsInvincible = false;

        }
    }


    public override void TakeDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            OnHealthUpdated?.Invoke(currentHealth);
            IsInvincible = true;
            StartCoroutine(InvincibillityFlash());
            IsInvincible = false;
            // StartCoroutine(InvincibillityDelay());

        }
    }

    public IEnumerator InvincibillityFlash()
    {
        while (IsInvincible == true)
        {
            sprite.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlash);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);

        }
    }
    public IEnumerator BurningFlash()
    {
        while (IsInvincible == true)
        {
            sprite.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);

        }
    }

    public IEnumerator StinkFlash()
    {
        while (IsInvincible == true)
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
