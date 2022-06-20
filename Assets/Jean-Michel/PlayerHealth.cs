using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioController;
using GoldProject.UI;
using SplashArt;

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
        Debug.Log("HealPlayer");
        ParticuleManager.Instance.OnPlayerHeal();
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
        if(dead)
            return;
        dead = true;
        
        // Temporary just for apk
        currentHealth = 0;
        OnHealthUpdated?.Invoke(currentHealth, healthMax);

        //ParticuleManager.Instance.OnPlayerDeathParticule();

        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        //    .buildIndex);

        // Reset Cam
        PlayerManager.Instance.miniMap.SetActive(false);
        if(PlayerManager.mapSeen == true)
            PlayerManager.mapSeen = !PlayerManager.mapSeen;

        Camera.main.GetComponent<AudioListener>().enabled = true;

        GameManager.SetPause(true);
        StartCoroutine(SoundDeath());
        
        // Time.timeScale = 0;
    }

    public void TakeFireDamage(int damage)
    {
        if (!IsInvincible)
        {
            currentHealth -= damage;
            AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Hurt);
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
            AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Hurt);
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
            AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Hurt);
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
        AudioManager.Instance.StopEverySound();
        AudioManager.Instance.PlayPlayerSound(PlayerAudioTracks.P_Death);

        yield return new WaitForSecondsRealtime(0.3f);

        UiManager.Instance.LauchGameOverMenu();
    }
}
