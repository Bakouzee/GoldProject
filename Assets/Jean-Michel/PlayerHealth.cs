using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerManager PlayerManager { private get; set; }
    public int maxHealth = 100;

    public int currentHealth;

    public bool IsInOnionZone = false;

    public bool IsInWindowZone = false;

    public float invincibilityFlash = 0.2f;

    public bool IsInvincible = false;

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
        if(currentHealth <= 0)
        {
            Death();
        }
    }
    public void HealPlayer(int healAmount)
    {
        if((currentHealth + healAmount) > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healAmount;

        }
        healthBar.SetHealth(currentHealth);
    }

    void Death()
    {
        // Temporary just for apk
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
            .buildIndex);
        // Time.timeScale = 0;
    }

    

    //public void TakeWindowDamage(int damage)
    //{
    //    if (IsInWindowZone)
    //    {
    //        currentHealth -= damage;
    //        healthBar.SetHealth(currentHealth);            
    //        StartCoroutine(BurningFlash());
            

    //    }
    //}
    //public void TakeOnionsDamage(int damage)
    //{
    //    if (IsInOnionZone)
    //    {
    //        currentHealth -= damage;
    //        healthBar.SetHealth(currentHealth);
    //        StartCoroutine(BurningFlash());

    //    }
    //}
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
        while (IsInWindowZone)
        {
            sprite.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);
            sprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlash);

        }
    }

    public IEnumerator StinkFlash()
    {
        while (IsInOnionZone)
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