using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioController;

public class FrighteningAnimationEvents : MonoBehaviour
{
    // animation event
    public void MakeScarySound(ScaryAudioTracks audioToPlay)
    {
        AudioManager.Instance.PlayScarySound(audioToPlay);
    }
}
