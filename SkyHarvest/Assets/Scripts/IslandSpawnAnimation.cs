using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawnAnimation : MonoBehaviour
{
    [SerializeField] private float startScale;
    
    private bool bHasFullyScaled = true;
    private float xScale;
    private float yScale;
    private float zScale;

    private void Start()
    {
        transform.localScale = new Vector3(startScale, startScale, startScale);
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
        zScale = transform.localScale.z;
    }

    private void Update()
    {
        if (!bHasFullyScaled)
        {
            if (xScale < 1 && yScale < 1 && zScale < 1)
            {
                transform.localScale *= Time.deltaTime;
            }
            else
            {
                bHasFullyScaled = true;
            }

        }
    }

    private void ScaleUp()
    {
        bHasFullyScaled = false;
    }
}
