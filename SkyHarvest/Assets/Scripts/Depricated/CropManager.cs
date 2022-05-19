//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CropManager : MonoBehaviour
//{
//    [SerializeField] private List<CropBehaviour> growingCrops;
//    [SerializeField] private List<CropBehaviour> harvestableCrops;

//    private void OnEnable()
//    {
//        CustomEvents.Crops.OnCropPlanted += OnCropPlanted;
//        CustomEvents.TimeCycle.OnNewDaySetup += HandleCropGrowth; 
//        CustomEvents.Crops.OnCropHarvestable += OnCropHarvestable;
//        CustomEvents.Crops.OnCropHarvested += OnCropHarvested;
//    }

//    private void OnDisable()
//    {
//        CustomEvents.Crops.OnCropPlanted -= OnCropPlanted;
//        CustomEvents.TimeCycle.OnNewDaySetup -= HandleCropGrowth;
//        CustomEvents.Crops.OnCropHarvestable -= OnCropHarvestable;
//        CustomEvents.Crops.OnCropHarvested -= OnCropHarvested;
//    }

//    private void OnCropPlanted(CropBehaviour _newCrop) //adds crop to list of currently growing crops when planted
//    {
//        if(!growingCrops.Contains(_newCrop))
//        {
//            growingCrops.Add(_newCrop);
//        }
//    }

//    private void OnCropHarvestable(CropBehaviour _harvestableCrop) //adds crop to list of harvestable crops and removes from list of growing crops
//    {
//        if(growingCrops.Contains(_harvestableCrop))
//        {
//            growingCrops.Remove(_harvestableCrop);
//        }
//        if(!harvestableCrops.Contains(_harvestableCrop))
//        {
//            harvestableCrops.Add(_harvestableCrop);
//        }
//    }

//    private void OnCropHarvested(CropBehaviour _harvestedCrop) //removed crop from list of harvestable crops
//    {
//        if(harvestableCrops.Contains(_harvestedCrop))
//        {
//            harvestableCrops.Remove(_harvestedCrop);
//        }
//    }

//    private void HandleCropGrowth() //on a new day, loops through all growing crops to have them handle their growth logic
//    {
//        List<CropBehaviour> cropsToHandle = growingCrops;

//        for (int i = 0; i < cropsToHandle.Count; i++)
//        {
//            cropsToHandle[i].HandleNewDay();
//        }
//    }
//}
