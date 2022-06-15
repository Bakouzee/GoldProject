using AudioController;
using Enemies;
using GoldProject;
using UnityEngine;

namespace Tuto
{
    public class PlayerManager : SingletonBase<PlayerManager>
    {
        public Tuto.Player Player { get; private set; } 
        public PlayerHealth PlayerHealth { get; private set; } 
        public PlayerBonuses Bonuses { get; private set; }

        public GameObject arrowToMovePlayer;
        public GameObject miniMap;
        public GameObject mainCam;
        public static bool mapSeen = false;

        protected override void Awake()
        {
            base.Awake();
            Player = GetComponent<Tuto.Player>();
            Player.PlayerManager = this;
        
            PlayerHealth = GetComponent<PlayerHealth>();
            // PlayerHealth.PlayerManager = this;

            // Bonuses = GetComponent<PlayerBonuses>();
            // Bonuses.PlayerManager = this;
        }

        private void Start()
        {
            if(miniMap) miniMap.SetActive(false);
        }


        public System.Action onShowMap;
        public void ShowMap()
        {
            if (miniMap != null)
            {
                if (!mapSeen)
                {
                    AudioManager.Instance.PlayMapSound(MapAudioTracks.Map_Open);
                    foreach(EnemyBase enemy in Player.CurrentRoom.enemies)
                    {
                        enemy.gameObject.layer = 3;
                    }
                    miniMap.SetActive(true);
                    arrowToMovePlayer.SetActive(false);
                    //mainCam.SetActive(false);
                    mainCam.GetComponent<AudioListener>().enabled = false;
                    onShowMap?.Invoke();
                } else
                {
                    AudioManager.Instance.PlayMapSound(MapAudioTracks.Map_Close);
                    foreach (EnemyBase enemy in Player.CurrentRoom.enemies)
                    {
                        enemy.gameObject.layer = 0;
                    }
                    mainCam.GetComponent<AudioListener>().enabled = true;
                    //mainCam.SetActive(true);
                    arrowToMovePlayer.SetActive(true);
                    miniMap.SetActive(false);
                    onShowMap?.Invoke();
                }
            }
            mapSeen = !mapSeen;
        }
    }
}
