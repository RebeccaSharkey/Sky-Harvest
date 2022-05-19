using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which moves an object to give a floating effect
/// </summary>
public class FloatingAnimation : MonoBehaviour
{
    private float posY;
    [SerializeField] private float floatStrength;

    private void Start()
    {
        posY = transform.position.y;
    }

    /// <summary>
    /// Moves an object positively and negatively in the direction of the Y axis
    /// Uses a sine wave mutliplied via a strength to control the dip of the wave
    /// </summary>
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, posY + ((float)Mathf.Sin(Time.time) * floatStrength), transform.position.z);
    }
}
