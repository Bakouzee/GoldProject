using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioController;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMusic(MusicAudioTracks.M_Menu);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
