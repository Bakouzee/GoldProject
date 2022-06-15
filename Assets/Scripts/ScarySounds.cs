using AudioController;
using UnityEngine;

namespace GoldProject
{
    public class ScarySounds : MonoBehaviour
    {
        [Space(10)] public ScaryAudioTracks scaryAudioTracks;
        public void PlayScarySound() => AudioManager.Instance.PlayScarySound(scaryAudioTracks);
    }
}