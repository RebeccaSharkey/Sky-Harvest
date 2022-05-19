using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script that changes what will be displayed based off of what is passed in from the AnimalPenBuying.cs
/// Toggles the UI on or off depending on what calls that function
/// </summary>
public class AnimalPenUI : MonoBehaviour
{
    [SerializeField] private GameObject penToBuyUI;
    [SerializeField] private TextMeshProUGUI penNameUI;
    [SerializeField] private TextMeshProUGUI penDescriptionUI;
    [SerializeField] private TextMeshProUGUI penCostUI;

    private int penID;

    /// <summary>
    /// Toggles the UI on or off depending on the bool passed in
    /// Unpauses the game if the UI is toggled off
    /// </summary>
    /// <param name="state">Bool passed in to determine the toggle of the UI</param>
    public void ToggleUI(bool state)
    {
        if(!state)
        {
            CustomEvents.TimeCycle.OnUnpause?.Invoke();
        }
        penToBuyUI.SetActive(state);
    }

    /// <summary>
    /// Sets up the UI to show information depending on what AnimalPenBuying.cs passed in.
    /// </summary>
    /// <param name="penName">String passed in to show the pen name</param>
    /// <param name="penDescription">String passed in to show the pen description</param>
    /// <param name="penCost">String passed in to show the pen cost</param>
    /// <param name="_penID">Int passed in to make sure the correct ID has been passed in</param>
    public void SetPenUI(string penName, string penDescription, int penCost, int _penID)
    {
        penID = _penID;
        penNameUI.text = penName;
        penDescriptionUI.text = penDescription;
        penCostUI.text = penCost.ToString();
    }

    /// <summary>
    /// UI is toggled off and the animal pen is spawned in based off of the value of the pen ID
    /// </summary>
    public void BuyPen()
    {
        ToggleUI(false);
        CustomEvents.AnimalPens.OnBuyAnimalPen?.Invoke(penID);
    }

    /// <summary>
    /// Subscribes and unsubscribes from the event systems depending on the objects state
    /// </summary>
    private void OnEnable()
    {
        CustomEvents.AnimalPens.OnToggleUI += ToggleUI;
        CustomEvents.AnimalPens.OnSetUI += SetPenUI;
    }

    private void OnDisable()
    {
        CustomEvents.AnimalPens.OnToggleUI -= ToggleUI;
        CustomEvents.AnimalPens.OnSetUI -= SetPenUI;
    }
}
