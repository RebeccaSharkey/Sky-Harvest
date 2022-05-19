using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWarning : MonoBehaviour
{
    [SerializeField] private GameObject warning;

    private void StartWarning()
    {
        StopAllCoroutines();
        StartCoroutine(PlayWarning());
    }

    IEnumerator PlayWarning()
    {
        warning.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        warning.SetActive(false);
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.OnInventoryTooFull += StartWarning;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.OnInventoryTooFull -= StartWarning;
    }

}
