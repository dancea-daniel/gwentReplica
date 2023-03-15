using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public GameObject pauseMenu;

    public GameObject menuButtons;
    public GameObject optionsButtons;
    public GameObject endingButtons;

    public GameObject helpPanel;

    public void PlayButton()
    {
        Debug.LogError("kovetkezo scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void Options()
    {
        menuButtons.SetActive(false);
        optionsButtons.SetActive(true);
    }
    
    public void RestartButton()
    {
        GameObject.Find("SceneManager").GetComponent<SceneController>().ResetMatch();
        pauseMenu.SetActive(false);
    }

    public void Back()
    {
        menuButtons.SetActive(true);
        optionsButtons.SetActive(false);

    }

    public void ChangeDeck()
    {
        SceneManager.LoadScene(1);
        menuButtons.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void HelpButton()
    {
        helpPanel.SetActive(true);
    }

    public void HelpButtonBack()
    {
        helpPanel.SetActive(false);
    }

}

