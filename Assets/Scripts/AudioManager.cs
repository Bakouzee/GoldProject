using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioController
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBase<AudioManager>
    {
       [System.Serializable]
        public struct PlayerSounds
        {
            public PlayerAudioTracks playerTracks;
            public List<AudioClip> playerClipList;
        }
        [SerializeField] private PlayerSounds[] playerSounds;
        
       [System.Serializable]
        public struct EnemySounds
        {
            public EnemyAudioTracks enemyTracks;
            public List<AudioClip> enemyClipList;
        }
        [SerializeField] private EnemySounds[] enemySounds;

       [System.Serializable]
        public struct Music
        {
            public MusicAudioTracks musicTracks;
            public List<AudioClip> soundtrackClipList;
        }
        [SerializeField] private Music[] musics;


        // Light, doors, blood, garlic
       [System.Serializable]
        public struct AmbianceSounds
        {
            public AmbianceAudioTracks ambianceTracks;
            public List<AudioClip> ambianceClipList;
        }
        [SerializeField] private AmbianceSounds[] ambianceSounds;


        // Player's trap
       [System.Serializable]
        public struct ScarySounds
        {
            public ScaryAudioTracks scaryTracks;
            public List<AudioClip> scaryClipList;
        }
        [SerializeField] private ScarySounds[] scarySounds;

        [System.Serializable]
        public struct MenuSounds
        {
            public MenuAudioTracks menuTracks;
            public List<AudioClip> menuClipList;
        }
        [SerializeField] private MenuSounds[] menuSounds;

        [System.Serializable]
        public struct VentSounds
        {
            public VentAudioTracks ventTracks;
            public List<AudioClip> ventClipList;
        }
        [SerializeField] private VentSounds[] ventSounds;

        [System.Serializable]
        public struct WindowSounds
        {
            public WindowAudioTracks windowTracks;
            public List<AudioClip> windowClipList;
        }
        [SerializeField] private WindowSounds[] windowSounds;

        private AudioSource sourceMusic;
        private AudioSource sourceFoliage;

        private void Start()
        {
            sourceMusic = GetComponent<AudioSource>();
            //Add audio mixer music

            sourceFoliage = gameObject.AddComponent<AudioSource>();
            //Add audio mixer sfx
        }

        #region Player Methods

        public void PlaySoundForPlayer(PlayerAudioTracks audioToPlay)
        {
            for(int i = 0; i < playerSounds.Length; i++)
            {
                if(playerSounds[i].playerTracks == audioToPlay)
                {
                    if(playerSounds[i].playerClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, playerSounds[i].playerClipList.Count);
                        AudioClip clipToPlay = playerSounds[i].playerClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = playerSounds[i].playerClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }



        #endregion

    }
}
