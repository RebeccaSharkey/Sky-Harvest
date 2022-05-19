using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sfxSoundsToPlay;
    [SerializeField] private Sound[] themeSoundsToPlay;
    private AudioClip clipToPlay;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource themeSource;
    [SerializeField] private AudioSource menuPopSource;
    
    private void PlaySoundEffect(string audioName)
    {
        foreach (Sound s in sfxSoundsToPlay)
        {
            if (s.soundName == audioName)
            {
                clipToPlay = s.soundClip;
                sfxSource.clip = clipToPlay;
                sfxSource.Play();
            }
        }
        Debug.Log("No compatible sound found");
    }

    private void PlayThemeSong(string audioName)
    {
        foreach (Sound s in themeSoundsToPlay)
        {
            if (s.soundName == audioName)
            {
                clipToPlay = s.soundClip;
                themeSource.clip = clipToPlay;
                themeSource.Play();
            }
        }
        Debug.Log("No compatible sound found");
    }

    public void PlayMenuPopSound()
    {
        menuPopSource.Play();
    }

    private void StopAllSound()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
        }

        if (themeSource.isPlaying)
        {
            themeSource.Stop();
        }
    }

    private void OnEnable()
    {
        CustomEvents.Audio.OnPlaySoundEffect += PlaySoundEffect;
        CustomEvents.Audio.OnPlayThemeSong += PlayThemeSong;
        CustomEvents.Audio.OnStopAllSound += StopAllSound;
    }

    private void OnDisable()
    {
        CustomEvents.Audio.OnPlaySoundEffect -= PlaySoundEffect;
        CustomEvents.Audio.OnPlayThemeSong -= PlayThemeSong;
        CustomEvents.Audio.OnStopAllSound -= StopAllSound;
    }
}
