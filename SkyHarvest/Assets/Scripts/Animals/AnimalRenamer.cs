using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalRenamer : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject inputWindow;
    private AnimalBehaviour animal;
    private string nameField;

    public void RenameAnimalBtn(bool state)
    {
        if (state)
        {
            RenameAnimal();
            ToggleRenamer(false);
        }
        else
        {
            ToggleRenamer(false);
        }
    }
    private void RenameAnimal()
    {
        nameField = inputField.text;
        animal.Rename(nameField);
    }

    private void ToggleRenamer(bool state)
    {
        inputWindow.SetActive(state);
        if (state)
        {
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
            CustomEvents.InventorySystem.PlayerInventory.OnSetAllowanceOfInventory?.Invoke(false);
            //CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(true);
        }
        else
        {
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
            CustomEvents.InventorySystem.PlayerInventory.OnSetAllowanceOfInventory?.Invoke(true);
            //CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(false);
        }
        
    }

    private void GetAnimalScript(AnimalBehaviour _script)
    {
        animal = _script;
    }

    private void OnEnable()
    {
        CustomEvents.AnimalPens.OnToggleRenamer += ToggleRenamer;
        CustomEvents.AnimalPens.OnGetAnimalScript += GetAnimalScript;
    }

    private void OnDisable()
    {
        CustomEvents.AnimalPens.OnToggleRenamer -= ToggleRenamer;
        CustomEvents.AnimalPens.OnGetAnimalScript -= GetAnimalScript;
    }
}
