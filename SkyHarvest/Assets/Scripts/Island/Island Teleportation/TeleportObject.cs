using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that all teleportation post objects utilise
/// Toggles the UI on or off
/// </summary>
public class TeleportObject : MonoBehaviour, iSaveable
{
    [SerializeField] private GameObject teleportOptionsUI;

    private Vector3 initialPos;
    private Vector3 _newPos;

    [SerializeField] private bool bCanShake;
    [SerializeField] private bool bIsStartingPole;
    
    [SerializeField] private Vector3 spawnOffset;

    private float timeToMove;
    [SerializeField] private float totemMoveTime;

    private GameObject dustFX;
    private bool bIsLerping = false;

    private void Start()
    {
        if (bIsStartingPole)
        {
            CustomEvents.IslandSystem.Teleportation.OnAddTeleportPos?.Invoke(transform.position, "Starter Island");
        }
    }

    private void Update()
    {
        if(bCanShake)
        {
            bCanShake = false;
            initialPos = transform.position + spawnOffset;
            _newPos = initialPos - spawnOffset;
            transform.position = initialPos;
            dustFX = Resources.Load<GameObject>("Prefabs/Particle Effects/TotemDustFX");
            StartCoroutine(LerpPos());
        }
    }

    /// <summary>
    /// Lerps the camera to the new position of the bought island
    /// Yields the code for a few seconds after the Lerp has finished
    /// </summary>
    /// <returns>WaitForSeconds - Amount of seconds to wait for</returns>
    private IEnumerator LerpPos()
    {
        yield return new WaitForSeconds(2f);
        bIsLerping = true;
        while (bIsLerping)
        {
            timeToMove += (Time.deltaTime) * (1 / totemMoveTime);
            transform.position = Vector3.Lerp(initialPos, _newPos, timeToMove);
            yield return null;
            if (timeToMove >= 1)
            {
                bIsLerping = false;
                timeToMove = 0;
                break;
            }
        }
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
        CustomEvents.Camera.CameraShake.OnCameraShake?.Invoke(0.5f, 0.2f);
        if(dustFX != null)
        {
            GameObject fx = Instantiate(dustFX, transform.position, Quaternion.identity);
            Destroy(fx, 1f);
        }
    }

    /// <summary>
    /// Toggles the UI on or off depending on the bool passed in
    /// </summary>
    /// <param name="state">Determines whether the UI is toggles or not</param>
    private void ToggleUI(bool state)
    {
        teleportOptionsUI.SetActive(state);
    }

    public void SetCanShake(bool state)
    {
        bCanShake = state;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        data.Add(bCanShake.ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        bCanShake = bool.Parse(_data[0]);
    }

    private void OnEnable()
    {
        CustomEvents.IslandSystem.Teleportation.OnToggleUI += ToggleUI;
    }

    private void OnDisable()
    {
        CustomEvents.IslandSystem.Teleportation.OnToggleUI -= ToggleUI;

    }
}
