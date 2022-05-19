using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Sets up the island UI to display the correct information
/// </summary>
public class IslandUI : MonoBehaviour
{
    [SerializeField] private GameObject islandToBuyUI;
    [SerializeField] private TextMeshProUGUI islandNameUI;
    [SerializeField] private TextMeshProUGUI islandDescriptionUI;
    [SerializeField] private TextMeshProUGUI islandCostUI;

    private int islandID;

    /// <summary>
    /// Toggles the island UI based on the bool passed in
    /// </summary>
    /// <param name="state">Bool passed in which controls the state of the UI</param>
    public void ToggleUI(bool state)
    {
        islandToBuyUI.SetActive(state);
        if (state)
        {
            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(false);
        }
        else
        {
            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(true);
        }
    }

    /// <summary>
    /// Sets up the UI with the correct information to be displayed
    /// </summary>
    /// <param name="islandName">String passed in to set up the correct island name</param>
    /// <param name="islandDescription">String passed in to set up the correct island description</param>
    /// <param name="islandCost">String passed in to set up the correct island cost</param>
    /// <param name="_islandID">Int passed in to compare island IDs to use the UI</param>
    public void SetIslandUI(string islandName, string islandDescription, int islandCost, int _islandID)
    {
        islandID = _islandID;
        islandDescriptionUI.text = islandDescription;
        islandCostUI.text = "Cost: " + islandCost.ToString();
        islandNameUI.text = islandName;
    }

    /// <summary>
    /// Function to buy the island and spawn the correct one in depending on the ID passed in
    /// </summary>
    public void BuyIsland()
    {
        ToggleUI(false);
        CustomEvents.IslandSystem.OnBuyIsland?.Invoke(islandID);
    }

    private void OnEnable()
    {
        CustomEvents.IslandSystem.OnToggleUI += ToggleUI;
        CustomEvents.IslandSystem.OnSetUI += SetIslandUI;
    }

    private void OnDisable()
    {
        CustomEvents.IslandSystem.OnToggleUI -= ToggleUI;
        CustomEvents.IslandSystem.OnSetUI -= SetIslandUI;
    }
}
