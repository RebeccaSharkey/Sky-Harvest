using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;

    [Header("Sub Menues")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    //[SerializeField] private GameObject saveMenu;
    //[SerializeField] private GameObject loadMenu;

    [Header("Pop Ups")]
    [SerializeField] private GameObject exitPopUp;

    public void PauseGame()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        if((bool)CustomEvents.TimeCycle.OnGetPaused?.Invoke())
        {
            CustomEvents.TimeCycle.OnUnpause?.Invoke();
            pauseMenu.SetActive(false);
        }
        else
        {
            CustomEvents.TimeCycle.OnPause?.Invoke();
            pauseMenu.SetActive(true);
        }
    }

    //public void OnSaveGameButton()
    //{
    //    saveMenu.SetActive(true);
    //    mainMenu.SetActive(false);
    //    if (exitPopUp.activeSelf)
    //    {
    //        exitPopUp.SetActive(false);
    //    }
    //}

    //public void OnLoadGameButton()
    //{
    //    loadMenu.SetActive(true);
    //    mainMenu.SetActive(false);
    //}

    public void OnOptionsButton()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OnMainMenuButton()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
        pauseMenu.SetActive(false);
        CustomEvents.SceneManagement.OnLoadNewScene?.Invoke("Main Menu");
    }

    public void OnExitGameButton()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        if(exitPopUp.activeSelf)
        {
            Application.Quit();
        }
        else
        {
            exitPopUp.SetActive(true);
        }
    }

    public void OnBackPress()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        optionsMenu.SetActive(false);
        //saveMenu.SetActive(false);
        //loadMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void TogglePauseButton(bool newState)
    {
        if(pauseButton)
        {
            pauseButton.SetActive(newState);
        }
    }

    private void OnEnable()
    {
        CustomEvents.UI.OnTogglePauseButton += TogglePauseButton;
    }

    private void OnDisable()
    {
        CustomEvents.UI.OnTogglePauseButton -= TogglePauseButton;
    }

}
