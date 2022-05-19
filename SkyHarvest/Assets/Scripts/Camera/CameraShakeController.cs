using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the camera shake of the cam its attached to
/// Randomly moves and rotates
/// </summary>
public class CameraShakeController : MonoBehaviour
{
    private float timeToShake;
    private float shakePower;
    private float shakeFadeTime;

    /*private float shakeRotation;
    [SerializeField] private float rotationMultiplier;*/

    private Vector3 amount;

    /// <summary>
    /// Checks to make sure there is time left to shake
    /// Moves the camera on the X and Y axis based on the power of the shake
    /// Slightly rotates the camera depending on the rotation multiplier
    /// </summary>
    private void LateUpdate()
    {
        if(timeToShake > 0f)
        {
            timeToShake -= Time.deltaTime;

            amount.x = Random.Range(-1, 1) * shakePower;
            amount.y = Random.Range(-1, 1) * shakePower;
            //amount.z = Random.Range(-1, 1) * shakePower;

            transform.position += amount;
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            /*shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));*/
        }
    }

    /// <summary>
    /// Camera shake to set the values to the parameters passed in
    /// </summary>
    /// <param name="length">How long the shake will last</param>
    /// <param name="magnitude">How powerful the shake will be</param>
    private void CameraShake(float length, float magnitude)
    {
        timeToShake = length;
        shakePower = magnitude;
        shakeFadeTime = magnitude / length;

        //shakeRotation = magnitude * rotationMultiplier;
    }

    private void OnEnable()
    {
        CustomEvents.Camera.CameraShake.OnCameraShake += CameraShake;
    }

    private void OnDisable()
    {
        CustomEvents.Camera.CameraShake.OnCameraShake -= CameraShake;
    }
}
