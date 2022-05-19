using UnityEngine;

/// <summary>
/// Contains all data about a crop
/// </summary>
[CreateAssetMenu(fileName = "CropDataSO", menuName = "SkyHarvest/ScriptableObjects/CropDataSO")]
public class CropDataSO : ScriptableObject
{
    public string cropName = "Crop";
    public int baseDaysToMiddleStage = 2;
    public int fertilisedDaysToMiddleStage = 1;
    public int baseDaysToGrow = 4;
    public int fertilisedDaysToGrow = 2;
    public int daysNotWateredToWither = 1;
    public int baseMinHarvest = 0, baseMaxHarvest = 1;
    public int fertilisedMinHarvest = 1, fertilisedMaxHarvest = 2;
    public float baseSeedReturnChance = 0.5f;
    public float fertilisedSeedReturnChance = 0.8f;
    public int baseMinSeedsReturned = 1, baseMaxSeedsReturned = 2;
    public float baseBadQualityChance = 0.2f, baseNormalQualityChance = 0.5f;
    public float fertilisedNormalQualityChance = 0.2f, fertilisedGoodQualityChance = 0.5f;

    [Header("Harvesting Data")]
    public SCR_Items[] harvestables;
    public SCR_Items[] rngHarvestables;
}
