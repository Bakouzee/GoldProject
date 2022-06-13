using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

        [System.Serializable]
        public struct MapSounds
        {
            public MapAudioTracks mapTracks;
            public List<AudioClip> mapClipList;
        }
        [SerializeField] private MapSounds[] mapSounds;

        private AudioSource sourceMusic;
        private AudioSource sourceFoliage;

        [Header("!!! DO NOT TOUCH !!!")]
        [SerializeField] private AudioMixerGroup mixerSFX;
        [SerializeField] private AudioMixerGroup mixerMusic;
        
        private float rPicth;
        public float minPitch;
        public float maxPitch;

        private void Start()
        {
            sourceMusic = GetComponent<AudioSource>();
            sourceMusic.loop = true;
            //Add audio mixer music
            sourceMusic.outputAudioMixerGroup = mixerMusic;

            sourceFoliage = gameObject.AddComponent<AudioSource>();
            //Add audio mixer sfx
            sourceFoliage.outputAudioMixerGroup = mixerSFX;
        }

        #region Player Methods

        public void PlayPlayerSound(PlayerAudioTracks audioToPlay)
        {
            sourceFoliage.pitch = RandomPitch();
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
            sourceFoliage.pitch = RandomPitch();

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

        
        #endregion

        #region Window Methods

        public void PlayWindowSound(WindowAudioTracks audioToPlay)
        {
            sourceFoliage.pitch = RandomPitch();

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
            sourceFoliage.pitch = RandomPitch();

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
            if (sourceMusic.isPlaying)
            {
                sourceMusic.Stop();
            }

            for (int i = 0; i < musics.Length; i++)
            {
                if (musics[i].musicTracks == audioToPlay)
                {
                    if (musics[i].soundtrackClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, musics[i].soundtrackClipList.Count);
                        AudioClip clipToPlay = musics[i].soundtrackClipList[randomSound];
                        sourceMusic.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = musics[i].soundtrackClipList[0];
                        sourceMusic.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Scary Methods

        public void PlayScarySound(ScaryAudioTracks audioToPlay)
        {
            sourceFoliage.pitch = RandomPitch();

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

        #region Map Methods

        public void PlayMapSound(MapAudioTracks audioToPlay)
        {
            for (int i = 0; i < mapSounds.Length; i++)
            {
                if (mapSounds[i].mapTracks == audioToPlay)
                {
                    if (mapSounds[i].mapClipList.Count > 1)
                    {
                        int randomSound = UnityEngine.Random.Range(0, mapSounds[i].mapClipList.Count);
                        AudioClip clipToPlay = mapSounds[i].mapClipList[randomSound];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                    else
                    {
                        AudioClip clipToPlay = mapSounds[i].mapClipList[0];
                        sourceFoliage.PlayOneShot(clipToPlay);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Stop Every sounds

        public void StopEverySound()
        {
            sourceFoliage.Stop();
            sourceMusic.Stop();
            sourceMusic.clip = mapSounds[1].mapClipList[0];
            sourceMusic.Play();
        }

        #endregion

        private float RandomPitch()
        {
            return rPicth = UnityEngine.Random.Range(minPitch, maxPitch);
        }
    }
}