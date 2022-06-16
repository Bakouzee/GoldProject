using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioController;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject settingMenuUI;

    public GameObject pauseButton;

    public bool buttonActivated = false;

    private void Update()
    {
        //if(buttonActivated)
        //{
        //    if (gameIsPaused)
        //    {
        //        Resume();
                
        //    }
        //    else
        //    {
        //        Pause();
        //    }
        //}
    }

    public void ButtonDown()
    {
        buttonActivated = true;
        if (buttonActivated)
        {
            if (gameIsPaused)
            {
                Resume();

            }
            else
            {
                Pause();

            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        AudioManager.Instance.ResumeMusic();
        Time.timeScale = 1f;
        gameIsPaused = false;


    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        AudioManager.Instance.PauseMusic();
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        if(GameObject.FindObjectOfType<TutorialManager>() != null)
            SceneManager.LoadScene("Tutorial");
        else
            SceneManager.LoadScene("Victoria_LD");
        
    }

    public void Settings()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(true);
        
    }

    public void BackFromSettings()
    {
        pauseMenuUI.SetActive(true);
        settingMenuUI.SetActive(false);
    }
}
