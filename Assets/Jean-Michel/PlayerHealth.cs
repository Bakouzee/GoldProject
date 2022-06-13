using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioController;

public class PlayerHealth : Health
{
    public PlayerManager PlayerManager { private get; set; }

    [HideInInspector]
    public bool IsInOnionZone = false;
    [HideInInspector]
    public bool IsInWindowZone = false;
    [HideInInspector]
    public float invincibilityFlash = 0.2f;
    [HideInInspector]
    public bool IsInvincible = false;

    public GameObject deathUI;

    /// <summary>Event called when the health is updated. Gives the new health amount and health max</summary>
    /// <params>newHealth, healthMax </params>
    public System.Action<int, int> OnHealthUpdated;
    
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
        OnHealthUpdated?.Invoke(currentHealth, healthMax);
    }

    public void Death()
    {
        // Temporary just for apk

        //ParticuleManager.Instance.OnPlayerDeathParticule();

        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        //    .buildIndex);
        
        StartCoroutine(SoundDeath());
        

        
        // Time.timeScale = 0;
    }

    public void TakeFireDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            OnHealthUpdated?.Invoke(currentHealth, healthMax);
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
            OnHealthUpdated?.Invoke(currentHealth, healthMax);
            IsInvincible = true;
            StartCoroutine(StinkFlash());
            // StartCoroutine(InvincibillityDelay());
            IsInvincible = false;

        }
    }


    public override bool TakeDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            OnHealthUpdated?.Invoke(currentHealth, healthMax);
            IsInvincible = true;
            StartCoroutine(InvincibillityFlash());
            IsInvincible = false;
            // StartCoroutine(InvincibillityDelay());
        }

        return false;
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

    public IEnumerator SoundDeath()
    {

        AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Death);
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 0;        
        deathUI.SetActive(true);
    }
}
