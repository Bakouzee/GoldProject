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

        public void PlayPlayerSound(PlayerAudioTracks audioToPlay)
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

        #region Enemy Methods

        public void PlayEnemySound(EnemyAudioTracks audioToPlay)
        {
            for (int i = 0; i < enemySounds.Length; i++)
            {
                if (enemySounds[i].enemyTracks == audioToPlay)
                {
                    if (enemySounds[i].enemyClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, enemySounds[i].enemyClipList.Count);
                        AudioClip clipToPlay = enemySounds[i].enemyClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = enemySounds[i].enemyClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Menu Methods

        public void PlayMenuSound(MenuAudioTracks audioToPlay)
        {
            for (int i = 0; i < menuSounds.Length; i++)
            {
                if (menuSounds[i].menuTracks == audioToPlay)
                {
                    if (menuSounds[i].menuClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, menuSounds[i].menuClipList.Count);
                        AudioClip clipToPlay = menuSounds[i].menuClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = menuSounds[i].menuClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #region Menu Methods

        public void PlaySoundForMenu(MenuAudioTracks audioToPlay)
        {
            for (int i = 0; i < menuSounds.Length; i++)
            {
                if (menuSounds[i].menuTracks == audioToPlay)
                {
                    if (menuSounds[i].menuClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, menuSounds[i].menuClipList.Count);
                        AudioClip clipToPlay = menuSounds[i].menuClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = menuSounds[i].menuClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion
        #endregion

        #region Window Methods

        public void PlayWindowSound(WindowAudioTracks audioToPlay)
        {
            for (int i = 0; i < windowSounds.Length; i++)
            {
                if (windowSounds[i].windowTracks == audioToPlay)
                {
                    if (windowSounds[i].windowClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, windowSounds[i].windowClipList.Count);
                        AudioClip clipToPlay = windowSounds[i].windowClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = windowSounds[i].windowClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Vent Methods

        public void PlayVentSound(VentAudioTracks audioToPlay)
        {
            for (int i = 0; i < ventSounds.Length; i++)
            {
                if (ventSounds[i].ventTracks == audioToPlay)
                {
                    if (ventSounds[i].ventClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, ventSounds[i].ventClipList.Count);
                        AudioClip clipToPlay = ventSounds[i].ventClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = ventSounds[i].ventClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Ambiance Methods

        public void PlayAmbianceSound(AmbianceAudioTracks audioToPlay)
        {
            for (int i = 0; i < ambianceSounds.Length; i++)
            {
                if (ambianceSounds[i].ambianceTracks == audioToPlay)
                {
                    if (ambianceSounds[i].ambianceClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, ambianceSounds[i].ambianceClipList.Count);
                        AudioClip clipToPlay = ambianceSounds[i].ambianceClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = ambianceSounds[i].ambianceClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Music Methods

        public void PlayMusic(MusicAudioTracks audioToPlay)
        {
            for (int i = 0; i < musics.Length; i++)
            {
                if (musics[i].musicTracks == audioToPlay)
                {
                    if (musics[i].soundtrackClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, musics[i].soundtrackClipList.Count);
                        AudioClip clipToPlay = musics[i].soundtrackClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = musics[i].soundtrackClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Scary Methods

        public void PlayScarySound(ScaryAudioTracks audioToPlay)
        {
            for (int i = 0; i < scarySounds.Length; i++)
            {
                if (scarySounds[i].scaryTracks == audioToPlay)
                {
                    if (scarySounds[i].scaryClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, scarySounds[i].scaryClipList.Count);
                        AudioClip clipToPlay = scarySounds[i].scaryClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = scarySounds[i].scaryClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion
    }
}