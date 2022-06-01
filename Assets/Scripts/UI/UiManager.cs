using UnityEngine;
using TMPro;

namespace GoldProject.UI
{
    public class UiManager : SingletonBase<UiManager>
    {
        // Day counter
        [SerializeField] private TMP_Text dayCounter;
        public void SetDay(int day) => dayCounter.text = day.ToString();

        // Action counter
        [SerializeField] private TMP_Text actionCounter;
        public void SetActionCount(int actionCount) => actionCounter.text = actionCount.ToString();

        // Health bar
        [SerializeField] private HealthBar playerHealthBar;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            // Day counter
            if (dayCounter != null)
            {
                SetDay(gameManager.CurrentDay);
                gameManager.OnDayChanged += SetDay;
            }

            // Action counter
            if (actionCounter != null)
                gameManager.OnLaunchedTurn += SetActionCount;

            // Health bar
            if (playerHealthBar != null)
            {
                var playerHealth = PlayerManager.Instance.PlayerHealth;
                playerHealthBar.SetMaxHealth(playerHealth.maxHealth);
                playerHealth.OnHealthUpdated += playerHealthBar.SetHealth;
            }
        }
    }
}