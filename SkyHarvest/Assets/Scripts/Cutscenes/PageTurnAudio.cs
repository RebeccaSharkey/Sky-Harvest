using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurnAudio : MonoBehaviour
{

    public GameObject ObjectToUnhide;
    public GameObject ObjectToHide;

    public AudioSource Clip;

    void Start()
    {

    }


    void Update()
    {
        if (Clip.isPlaying == false)
        {
            ObjectToUnhide.SetActive(true);
            ObjectToHide.SetActive(false);
        }
    }
}
