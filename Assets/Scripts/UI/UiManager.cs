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
        [SerializeField] private TMP_Text dayCounterForMenu;

        [SerializeField] private TMP_Text enemyKilled;

        [SerializeField] private TMP_Text enemyScared;
        

        public void SetDay(int day) => dayCounter.text = day.ToString();
        public void SetDayForMenu(int day) => dayCounterForMenu.text = day.ToString();

        public void SetEnemyKilled(int day) => enemyKilled.text = day.ToString();

        public void SetEnemyScared(int day) => enemyScared.text = day.ToString();



        // Action counter
        [SerializeField] private TMP_Text actionCounter;
        public void SetActionCount(int actionCount, int actionPerPhase) => actionCounter.text = actionCount.ToString();

        // Health bar
        [SerializeField] private HealthBar playerHealthBar;

        // Arrows
        [Header("Arrows")]
        [SerializeField] private Button topArrowButton;
        [SerializeField] private Button leftArrowButton;
        [SerializeField] private Button downArrowButton;
        [SerializeField] private Button rightArrowButton;

        [Header("Clock"), Tooltip("Image of the clock that is going to be rotated"), SerializeField] 
        private Transform clock;
        private float clockStartOffset;

        [Header("Transformation Indicator"), SerializeField]
        private TransformationIndicator transformationIndicator;
        public ParticleSystem transformationParticles;

        public GameObject DeathUI;
        public void LauchGameOverMenu()
        {
            DeathUI.SetActive(true);
            SetEnemyKilled(Enemies.EnemyManager.enemyKilled);
            SetEnemyScared(Enemies.EnemyManager.enemyAfraid);
        }
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            // Day counter
            if (dayCounter != null)
            {
                SetDay(gameManager.CurrentDay);
                gameManager.OnDayChanged += SetDay;
            }

            // DayForMenu counter
            if (dayCounterForMenu != null)
            {
                SetDayForMenu(gameManager.CurrentDay);
                gameManager.OnDayChanged += SetDayForMenu;
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
            
            // Rotate the clock showing day/night
            if (clock)
            {
                clockStartOffset = clock.eulerAngles.z;
                gameManager.OnLaunchedTurn += (actionCount, actionPerPhase) =>
                {
                    float z = (actionCount / (float)actionPerPhase) * 180f;
                    if (GameManager.dayState == GameManager.DayState.NIGHT)
                        z += 180f;
                    clock.eulerAngles = Vector3.forward * (z + clockStartOffset);
                };
            }
            
            // Transformation indicator
            if (transformationIndicator)
            {
                transformationIndicator.SetIndicator(false);
                PlayerManager.Instance.Player.OnCanTransformChanged +=
                    canTransform => transformationIndicator.SetIndicator(canTransform);
            }

            // Transformation Particles
            if (transformationParticles)
            {
                transformationParticles.gameObject.SetActive(false);
            }
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