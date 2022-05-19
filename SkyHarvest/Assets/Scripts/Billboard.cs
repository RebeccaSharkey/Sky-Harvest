using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform mainCam;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCam.forward); 
    }
}
