using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlotState { blocked, empty, unwatered, watered, harvestable }
public enum FertiliserState { unfertilised, growthSpeed, cropYield, seedYield, quality}

/// <summary>
/// Monobehaviour for each plot, handles player interaction with crops
/// </summary>
public class PlotBehaviour : MonoBehaviour, iSaveable
{
    [SerializeField] private bool isStartingBlocked = false;
    [SerializeField] private GameObject blockingObject;

    [SerializeField] private Vector3 offset;
    private GameObject waterParticleSystem;
    private GameObject fertiliseParticleSystem;
    private GameObject explosionParticleSystem;

    private bool isBlocked = false;
    private bool isPlanted = false;
    private CropBehaviour plantedCrop;
    private FertiliserState fertiliserState;

    private void Start()
    {
        isBlocked = isStartingBlocked;
        waterParticleSystem = Resources.Load<GameObject>("Prefabs/Particle Effects/WaterDropsFX");
        fertiliseParticleSystem = Resources.Load<GameObject>("Prefabs/Particle Effects/FertiliseFX");
        explosionParticleSystem = Resources.Load<GameObject>("Prefabs/Particle Effects/SmokeExplosionFX");
    }

    /// <summary>
    /// Instantiates specified crop prefab on the plot if nothing else is planted there
    /// </summary>
    /// <param name="_cropName">Name of the crop to be planted</param>
    public void PlantCrop(string _cropName)
    {
        if(!isPlanted)
        {
            isPlanted = true;
            fertiliserState = FertiliserState.unfertilised;
            GameObject temp = Instantiate(Resources.Load($"Prefabs/Crops/{_cropName}", typeof(GameObject)) as GameObject, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.z), transform);
            plantedCrop = temp.GetComponent<CropBehaviour>();
            CustomEvents.Crops.OnCropPlanted?.Invoke();
            CustomEvents.Achievements.OnAddToTotalCropsPlanted?.Invoke(1);
        }
    }

    /// <summary>
    /// Marks plant as watered and handles watering vfx
    /// </summary>
    public void WaterCrop()
    {
        plantedCrop.Water();
        GameObject temp = Instantiate(waterParticleSystem, transform.position + offset, Quaternion.identity);
        Destroy(temp, 1f);
        CustomEvents.Crops.OnCropWatered?.Invoke();
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Water Crops");
        //soil model/texture swap here
    }

    /// <summary>
    /// Sets fertiliser state and handles fertiliser vfx
    /// </summary>
    /// <param name="_fertiliserType">Type of fertiliser used</param>
    public void FertiliseCrop(string _fertiliserType)
    {
        switch(_fertiliserType)
        {
            case "growthSpeed":
                fertiliserState = FertiliserState.growthSpeed;
                plantedCrop.GrowthFertilise();
                break;
            case "cropYield":
                fertiliserState = FertiliserState.cropYield;
                break;
            case "seedYield":
                fertiliserState = FertiliserState.seedYield;
                break;
            case "quality":
                fertiliserState = FertiliserState.quality;
                break;
            default:
                Debug.LogError($"Unhandled fertiliser type: {_fertiliserType}");
                return;
        }
        GameObject temp = Instantiate(fertiliseParticleSystem, transform.position + offset, Quaternion.identity);
        Destroy(temp, 1f);
        CustomEvents.Crops.OnCropFertilised?.Invoke();
        CustomEvents.Achievements.OnAddToTotalCropsFertilised?.Invoke(1);
    }

    /// <summary>
    /// Handles crop harvesting and crop harvesting vfx
    /// </summary>
    public void HarvestCrop()
    {
        plantedCrop.Harvest(fertiliserState);
        //soil model/texture swap here
        isPlanted = false;
        plantedCrop = null;
        CustomEvents.Crops.OnCropHarvested?.Invoke();
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Harvesting");
    }

    /// <summary>
    /// Handles clearing blocked plots and clearing vfx
    /// </summary>
    public void ClearPlot()
    {
        if (!isBlocked) return;
        isBlocked = false;
        if (blockingObject != null)
        {
            blockingObject.SetActive(false);
            GameObject temp = Instantiate(explosionParticleSystem, transform.position + offset, Quaternion.identity);
            Destroy(temp, 1f);
        }
        else
        {
            Debug.LogError("No blocking object assigned");
        }
    }

    /// <summary>
    /// Determines plot state and returns it
    /// </summary>
    /// <returns>Current state of the plot as PlotState enum</returns>
    public PlotState GetPlotState()
    {
        if (isBlocked) return PlotState.blocked;
        if (!isPlanted) return PlotState.empty;
        else if (plantedCrop.isHarvestable) return PlotState.harvestable;
        else if (!plantedCrop.isWatered) return PlotState.unwatered;
        else return PlotState.watered;
    }

    /// <summary>
    /// Determines whether plot is fertilised or not and returns result
    /// </summary>
    /// <returns>True if fertilised and vice-versa</returns>
    public bool GetIsFertilised()
    {
        if (fertiliserState == FertiliserState.unfertilised) return false;
        return true;
    }

    /// <summary>
    /// Handles gathering and saving data relating to specific plot
    /// </summary>
    /// <returns>Populated PlotSaveData object</returns>
    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        if(plantedCrop != null)
        {
            data.Add(isBlocked.ToString());
            data.Add(fertiliserState.ToString());
            data.Add(plantedCrop.gameObject.name.Substring(0, plantedCrop.gameObject.name.Length - 7));
            data.Add(plantedCrop.daysMatured.ToString());
            data.Add(plantedCrop.daysNotWatered.ToString());
        }
        else
        {
            data.Add(isBlocked.ToString());
            data.Add(fertiliserState.ToString());
            data.Add("null");
            data.Add("0");
            data.Add("0");
        }

        return data;
    }

    ///// <summary>
    ///// Handles populating local variables with values from loaded data
    ///// </summary>
    ///// <param name="data">Loaded save data relating to specific plot</param>
    public void LoadData(SerializableList _data)
    {

        if (isStartingBlocked && !bool.Parse(_data[0])) ClearPlot();


        fertiliserState = (FertiliserState)Enum.Parse(typeof(FertiliserState), _data[1]);

        if (_data[2] != "null") PlantCrop(_data[2]);

        if(isPlanted) plantedCrop.LoadData(int.Parse(_data[3]), int.Parse(_data[4]), fertiliserState == FertiliserState.growthSpeed ? true : false);
    }
}
