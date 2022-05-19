using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPlay : MonoBehaviour
{
    private float timeLeft;
    private ParticleSystem mainParticleSystem;
    private ParticleSystem childParticleSystem;

    private void Start()
    {
        mainParticleSystem = GetComponent<ParticleSystem>();
        timeLeft = mainParticleSystem.main.duration;
        if(transform.childCount > 0)
        {
            childParticleSystem = GetComponentInChildren<ParticleSystem>();
        }
        else
        {
            childParticleSystem = null;
        }

        mainParticleSystem.Play();
        if(childParticleSystem != null)
        {
            childParticleSystem.Play();
        }

        Destroy(this, timeLeft);
    }
}
