using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioController;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public TMP_Text versionText;
    private void Start()
    {
        AudioManager.Instance.PlayMusic(MusicAudioTracks.M_Menu);


        versionText.text = "Version : " + Application.version;
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Tuto()
    {
        SceneManager.LoadScene("Tutorial");
    }

   
}
