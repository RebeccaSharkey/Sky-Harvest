using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

/// <summary>
/// Methods used for functionality of the options menu
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer sfxMixer;
    [SerializeField] private AudioMixer musicMixer;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        //Saves an array of all resolutions Unity can use
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        //Saves each resolution to a list of strings
        List<string> resolutionOptions = new List<string>();
        int resolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string resolution = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(resolution);

            if(resolutions[i].width == Screen.currentResolution.width)
            {
                if (resolutions[i].height == Screen.currentResolution.height)
                {
                    resolutionIndex = i;
                }
            }
        }

        //Adds the otpions to the dropdown list and refreshes to show them
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Allows a slider to be used to control a float (volume)
    /// </summary>
    /// <param name="volume">Controls the amount used within the AudioMixer slider</param>
    public void SetSfxVolume(float volume)
    {
        sfxMixer.SetFloat("Sfx Volume", volume);
    }

    /// <summary>
    /// Allows a slider to be used to control a float (volume)
    /// </summary>
    /// <param name="volume">Controls the amount used within the AudioMixer slider</param>
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("Music Volume", volume);
    }

    /// <summary>
    /// Sets the game to fullscreen when toggled
    /// </summary>
    /// <param name="bIsFullscreen">Controls whether the game is fullscreen or not</param>
    public void SetFullscreen(bool bIsFullscreen)
    {
        Screen.fullScreen = bIsFullscreen;
    }

    /// <summary>
    /// Sets the resolution of the game depending on the resolution option chosen
    /// </summary>
    /// <param name="resolutionIndex">Grabs an element from the resolution array</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
