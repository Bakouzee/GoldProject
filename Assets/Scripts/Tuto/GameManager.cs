using System.Collections.Generic;
using Enemies;
using GoldProject;
using GoldProject.Rooms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AudioController;
using GoldProject.FrighteningEvent;
using PlayStoreScripts;

namespace Tuto
{
    public class GameManager : SingletonBase<GameManager>
    {
        public VentManager[] vm;

        public enum DayState
        {
            DAY,
            NIGHT
        };


        public int actionCountForVent = 0;


        public static DayState dayState = DayState.DAY;
        public static EventSystem eventSystem;

        [Header("Turns")] public int actionPerPhase;
        public int actionCount;
        private int currentDay;
        public System.Action<int> OnDayChanged;

        public int CurrentDay
        {
            get => currentDay;
            private set
            {
                currentDay = Mathf.Clamp(value, 1, int.MaxValue);
                OnDayChanged?.Invoke(currentDay);
            }
        }

        [SerializeField] private Vector2 dayNightTurnCooldown;
        private Cooldown turnCooldown;
        public GameObject undoButton;

        // Enemy Spawn
        [SerializeField] private Transform enemySpawnPoint;
        public Transform EnemySpawnPoint => enemySpawnPoint;
        private bool spawningEnemies;
        private bool chiefSpawned;
        private Enemies.EnemyType[] enemiesToSpawn;
        private int enemySpawned = 0;

        protected override void Awake()
        {
            base.Awake();

            ResetStaticVars();
        }

        private void Start()
        {
            // Set turn cooldown
            turnCooldown = new Cooldown(dayNightTurnCooldown.x);

            OnDayStart += () =>
            {
                AudioManager.Instance.PlayMusic(MusicAudioTracks.M_DAY);
                AudioManager.Instance.PlayEnemySound(EnemyAudioTracks.E_Entrance);
                AudioManager.Instance.PlayAmbianceSound(AmbianceAudioTracks.Cocorico);
            };

            OnNightStart += () =>
            {
                AudioManager.Instance.PlayAmbianceSound(AmbianceAudioTracks.Thunder);
                AudioManager.Instance.PlayMusic(MusicAudioTracks.M_NIGHT);
            };

            // Init days
            currentDay = 0;
            StartDay();
        }

        private void ResetStaticVars()
        {
            dayState = DayState.DAY;
            eventSystem = FindObjectOfType<EventSystem>();

            EnemyManager.Reset();
        }

        private void Update()
        {
            if (dayState == DayState.DAY)
            {
                if (undoButton != null) undoButton.SetActive(false);
            }
            else
            {
                if (undoButton != null) undoButton.SetActive(true);
            }
        }

        #region Start phases

        public System.Action OnDayStart;
        public System.Action OnNightStart;

        public void StartDay()
        {
            Debug.Log("Day");
            StartPhaseBase();
            CurrentDay++;
            dayState = DayState.DAY;

            // Set turn cooldown
            turnCooldown.cooldownDuration = dayNightTurnCooldown.x;

            OnDayStart?.Invoke();
        }

        public void StartNight()
        {
            Debug.Log("Night");
            StartPhaseBase();
            dayState = DayState.NIGHT;

            // Set turn cooldown
            turnCooldown.cooldownDuration = dayNightTurnCooldown.y;

            OnNightStart?.Invoke();
        }

        private void StartPhaseBase()
        {
            actionCount = 0;
        }

        #endregion

        #region UI Methods

        public void ActivateTrap(Button trapButton)
        {
            // Have to reset if the player reactivates the trap
            ColorBlock colorTrapActivated = new ColorBlock();
            colorTrapActivated.disabledColor = Color.green;
            colorTrapActivated.colorMultiplier = 1;
            trapButton.interactable = false;
            trapButton.colors = colorTrapActivated;
            Debug.Log("Trap Activated");
        }

        #endregion
    }
}