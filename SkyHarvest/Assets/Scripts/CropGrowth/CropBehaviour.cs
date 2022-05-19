using UnityEngine;

/// <summary>
/// Monobehaviour on each planted crop, handles crop life cycle
/// </summary>
public class CropBehaviour : MonoBehaviour
{
    [SerializeField] private CropDataSO cropData;
    public CropDataSO GetCropData { get => cropData; }
    [SerializeField] private GameObject seedModel, growingModel, grownModel, witheredModel;

    public int daysMatured = 0;
    public int daysNotWatered = 0;
    public bool isWatered = false;
    public bool isHarvestable = false;
    private bool isWithered = false;
    private bool isGrowthFertilised = false;

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnNewDaySetup += HandleNewDay;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
    }

    /// <summary>
    /// Handles growth cycle each new day
    /// </summary>
    public void HandleNewDay() 
    {
        CustomEvents.TimeCycle.OnUpdating?.Invoke();
        if(isWatered)
        {
            daysMatured++; //if crop is watered, increments days matured
            isWatered = false;
            daysNotWatered = 0;
            if ((isGrowthFertilised && daysMatured >= cropData.fertilisedDaysToGrow) || (!isGrowthFertilised && daysMatured >= cropData.baseDaysToGrow)) //if the crops growth cycle has been completed, mark as harvestable
            {
                isHarvestable = true;
                CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
                growingModel.SetActive(false);
                grownModel.SetActive(true);

            }
            else if (!growingModel.activeSelf && (isGrowthFertilised && daysMatured >= cropData.fertilisedDaysToMiddleStage) || (!isGrowthFertilised && daysMatured >= cropData.baseDaysToMiddleStage)) //sets half grown model when specific days have passed
            {
                seedModel.SetActive(false);
                growingModel.SetActive(true);
            }
        }
        else
        {
            daysNotWatered++; //if crop is not watered, increments days not watered
            if(daysNotWatered >= cropData.daysNotWateredToWither) //if the number of consecutive non watered days for the crop to wither has been reached, mark crop as withered and harvestable
            {
                isWithered = true;
                isHarvestable = true;
                CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
                seedModel.SetActive(false);
                growingModel.SetActive(false);
                grownModel.SetActive(false);
                witheredModel.SetActive(true);
            }
        }
        CustomEvents.TimeCycle.OnUpdated?.Invoke();
    }

    /// <summary>
    /// Marks watered
    /// </summary>
    public void Water()
    {
        isWatered = true;
    }

    /// <summary>
    /// Marks growth speed fertiliser
    /// </summary>
    public void GrowthFertilise()
    {
        isGrowthFertilised = true;
    }

    /// <summary>
    /// Handles harvesting crop, calculates drops based on crop data and fertiliser state
    /// </summary>
    /// <param name="_fertiliserState"></param>
    public void Harvest(FertiliserState _fertiliserState)
    {
        if(!isHarvestable) return;
        if(isWithered) //if crop is dead, give dead crop item
        {
            SCR_CropItems deadPlant = Resources.Load<SCR_CropItems>("Items/Produce/Dead Crop");
            Debug.Log(deadPlant.name);
            CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(deadPlant, 2, ItemQuality.Normal);
        }
        else
        {
            foreach (SCR_Items item in cropData.harvestables)
            { 
                switch(item.ItemType)
                {
                    case ItemTypes.crop:
                        if(_fertiliserState == FertiliserState.cropYield)
                        {
                            CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.fertilisedMinHarvest, (cropData.fertilisedMaxHarvest + 1)), ItemQuality.Normal);
                        }
                        else
                        {
                            float roll = Random.Range(0.0f, 1.0f);

                            if (_fertiliserState == FertiliserState.quality)
                            {
                                if ( roll <= cropData.fertilisedNormalQualityChance)
                                {
                                    //give normal quality version
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinHarvest, (cropData.baseMaxHarvest + 1)), ItemQuality.Normal);
                                }
                                else if (roll <= (cropData.fertilisedNormalQualityChance + cropData.fertilisedGoodQualityChance))
                                {
                                    //give normal quality version
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinHarvest, (cropData.baseMaxHarvest + 1)), ItemQuality.Good);
                                }
                                else
                                {
                                    //give normal quality version
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinHarvest, (cropData.baseMaxHarvest + 1)), ItemQuality.Perfect);
                                }
                            }
                            else
                            {
                                if (roll <= cropData.baseBadQualityChance)
                                {
                                    //give bad quality version
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinHarvest, (cropData.baseMaxHarvest + 1)), ItemQuality.Bad);
                                }
                                else if (roll <= (cropData.baseBadQualityChance + cropData.baseNormalQualityChance)) 
                                {
                                    //give normal quality version
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinHarvest, (cropData.baseMaxHarvest + 1)), ItemQuality.Normal);
                                }
                                else
                                {
                                    //give good quality version
                                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinHarvest, (cropData.baseMaxHarvest + 1)), ItemQuality.Good);
                                }
                            }
                        }
                        break;
                    case ItemTypes.seed:
                        bool isSeedsReturned = false;
                        if(_fertiliserState == FertiliserState.seedYield)
                        {
                            if (Random.Range(0.0f, 1.0f) <= cropData.fertilisedSeedReturnChance)
                            {
                                isSeedsReturned = true;
                            }
                        }
                        else
                        {
                            if (Random.Range(0.0f, 1.0f) <= cropData.baseSeedReturnChance)
                            {
                                isSeedsReturned = true;
                            }
                        }

                        if(isSeedsReturned)
                        {
                            CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, Random.Range(cropData.baseMinSeedsReturned, (cropData.baseMaxSeedsReturned + 1)), ItemQuality.Normal);
                        }
                        break;
                    default:
                        Debug.LogError("Unhandled item type in harvest");
                        break;
                }
            }
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Handles gathering and saving data relating to specific crop
    /// </summary>
    /// <returns>Populated CropSaveData object</returns>
    public CropSaveData SaveData()
    {
        return new CropSaveData(gameObject.name.Substring(0, gameObject.name.Length - 7), daysMatured, daysNotWatered); //substring removes the (Clone) from the end of the object name
    }

    /// <summary>
    /// Handles populating local variables with values from loaded data
    /// </summary>
    /// <param name="data">Loaded save data relating to specific crop</param>
    /// <param name="_isGrowthFertilised">Determined and passed in by plot behaviour</param>
    public void LoadData(int _daysMatured, int _daysNotWatered, bool _isGrowthFertilised)
    {
        daysMatured = _daysMatured;
        daysNotWatered = _daysNotWatered;
        isGrowthFertilised = _isGrowthFertilised;

        //if (daysNotWatered >= cropData.daysNotWateredToWither) isWithered = true;
        //else isWithered = false;

        //if (isGrowthFertilised)
        //{
        //    if (daysMatured >= cropData.fertilisedDaysToGrow) isHarvestable = true;
        //    else isHarvestable = false;
        //}
        //else
        //{
        //    if (daysMatured >= cropData.baseDaysToGrow) isHarvestable = true;
        //    else isHarvestable = false;
        //}

        if (daysNotWatered >= cropData.daysNotWateredToWither) //if the number of consecutive non watered days for the crop to wither has been reached, mark crop as withered and harvestable
        {
            isWithered = true;
            isHarvestable = true;
            CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
            seedModel.SetActive(false);
            witheredModel.SetActive(true);
        }
        else if ((isGrowthFertilised && daysMatured >= cropData.fertilisedDaysToGrow) || (!isGrowthFertilised && daysMatured >= cropData.baseDaysToGrow)) //if the crops growth cycle has been completed, mark as harvestable
        {
            isHarvestable = true;
            CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
            seedModel.SetActive(false);
            grownModel.SetActive(true);

        }
        else if (!growingModel.activeSelf && (isGrowthFertilised && daysMatured >= cropData.fertilisedDaysToMiddleStage) || (!isGrowthFertilised && daysMatured >= cropData.baseDaysToMiddleStage)) //sets half grown model when specific days have passed
        {
            seedModel.SetActive(false);
            growingModel.SetActive(true);
        }
    }
}
