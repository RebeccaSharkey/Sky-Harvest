using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles logic for the end of day screen, including displaying the summary info, and closing it
/// </summary>
public class EndOfDayScreen : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TextMeshProUGUI moneyEarnedText, cropsPlantedText, cropsWateredText, cropsFertilisedText, cropsHarvestedText, fishCaughtText;

    private int moneyEarned;
    private int cropsPlanted;
    private int cropsWatered;
    private int cropsFertilised;
    private int cropsHarvested;
    private int fishCaught;

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnDayEnd += EnableScreen;
        CustomEvents.TimeCycle.OnReadyForDayStart += EnableButton;
        CustomEvents.Currency.OnAddCurrency += MoneyEarned;
        CustomEvents.Crops.OnCropPlanted += CropPlanted;
        CustomEvents.Crops.OnCropWatered += CropWatered;
        CustomEvents.Crops.OnCropFertilised += CropFertilised;
        CustomEvents.Crops.OnCropHarvested += CropHarvested;
        CustomEvents.Fishing.OnFishCaught += FishCaught;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnDayEnd -= EnableScreen;
        CustomEvents.TimeCycle.OnReadyForDayStart -= EnableButton;
        CustomEvents.Currency.OnAddCurrency -= MoneyEarned;
        CustomEvents.Crops.OnCropPlanted -= CropPlanted;
        CustomEvents.Crops.OnCropWatered -= CropWatered;
        CustomEvents.Crops.OnCropFertilised -= CropFertilised;
        CustomEvents.Crops.OnCropHarvested -= CropHarvested;
        CustomEvents.Fishing.OnFishCaught -= FishCaught;
    }

    private void EnableScreen()
    {
        SetText();
        content.SetActive(true);
    }

    /// <summary>
    /// Closes screen
    /// </summary>
    public void DisableScreen()
    {
        continueButton.SetActive(false);
        content.SetActive(false);
        CustomEvents.UI.OnRandomFadeIn?.Invoke();
        CustomEvents.TimeCycle.OnDayStart?.Invoke();
        ResetValues();
    }

    /// <summary>
    /// Sets summary info
    /// </summary>
    private void SetText()
    {
        moneyEarnedText.text = $"Money earned: {moneyEarned}";
        cropsPlantedText.text = $"Crops planted: {cropsPlanted}";
        cropsWateredText.text = $"Crops watered: {cropsWatered}";
        cropsFertilisedText.text = $"Crops fertilised: {cropsFertilised}";
        cropsHarvestedText.text = $"Crops harvested: {cropsHarvested}";
        fishCaughtText.text = $"Fish caught: {fishCaught}";
        
        CustomEvents.Achievements.OnAddToTotalCropsPlanted?.Invoke(cropsPlanted);
        CustomEvents.Achievements.OnAddToTotalCropsFertilised?.Invoke(cropsFertilised);
        CustomEvents.Achievements.OnAddToTotalFishCaught?.Invoke(fishCaught);
    }

    private void EnableButton()
    {
        continueButton.SetActive(true);
    }

    private void MoneyEarned(int _money)
    {
        moneyEarned += _money;
    }

    private void CropPlanted()
    {
        cropsPlanted++;
    }

    private void CropWatered()
    {
        cropsWatered++;
    }

    private void CropFertilised()
    {
        cropsFertilised++;
    }

    private void CropHarvested()
    {
        cropsHarvested++;
    }

    private void FishCaught(int _null)
    {
        fishCaught++;
    }

    /// <summary>
    /// Reset all values for  next day
    /// </summary>
    private void ResetValues()
    {
        moneyEarned = 0;
        cropsPlanted = 0;
        cropsWatered = 0;
        cropsFertilised = 0;
        cropsHarvested = 0;
        fishCaught = 0;
    }
}
