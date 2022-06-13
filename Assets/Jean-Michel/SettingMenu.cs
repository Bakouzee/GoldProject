using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
    public void SetSFX(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }
}
