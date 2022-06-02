using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

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

        // Arrows
        [Header("Arrows")]
        [SerializeField] private Button topArrowButton;
        [SerializeField] private Button leftArrowButton;
        [SerializeField] private Button downArrowButton;
        [SerializeField] private Button rightArrowButton;
        
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
                playerHealthBar.SetMaxHealth(playerHealth.healthMax);
                playerHealth.OnHealthUpdated += playerHealthBar.SetHealth;
            }
            
            // Arrows
            var instancePlayer = PlayerManager.Instance.Player;
            SetButtonListener(leftArrowButton, () => instancePlayer.MoveLeft());
            SetButtonListener(topArrowButton, () => instancePlayer.MoveUp());
            SetButtonListener(rightArrowButton, () => instancePlayer.MoveRight());
            SetButtonListener(downArrowButton, () => instancePlayer.MoveDown());
        }

        private void SetButtonListener(Button button, UnityAction listener)
        {
            if (button == null)
                return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(listener);
        }
    }
}