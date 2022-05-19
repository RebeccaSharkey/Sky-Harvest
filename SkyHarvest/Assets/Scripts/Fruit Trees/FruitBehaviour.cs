using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class FruitBehaviour : MonoBehaviour
{
    [SerializeField] private FruitTreeSO fruitData;
    public FruitTreeSO GetFruitTreeSO { get => fruitData; }
    [SerializeField] private GameObject sapling, growing, grown, withered;
    
    public int daysMatured = 0;
    public int daysNotWatered = 0;
    public bool bIsWatered = false;
    public bool bIsHarvestable = false;
    public bool bIsWithered = false;
 
    public void HandleNewDay() 
    {
        CustomEvents.TimeCycle.OnUpdating?.Invoke();
        if(bIsWatered)
        {
            daysMatured++; //if crop is watered, increments days matured
            bIsWatered = false;
            transform.parent.GetComponent<FruitTreePlotBehaviour>().SetPlotState(FruitPlotState.Unwatered);
            daysNotWatered = 0;
            if (daysMatured >= fruitData.daysToFullyGrow) //if the crops growth cycle has been completed, mark as harvestable
            {
                bIsHarvestable = true;
                CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
                transform.parent.GetComponent<FruitTreePlotBehaviour>().SetPlotState(FruitPlotState.Harvestable);

                growing.SetActive(false);
                grown.SetActive(true);

            }
            else if (daysMatured >= fruitData.daysToMiddleGrow && daysMatured < fruitData.daysToFullyGrow) //sets half grown model when specific days have passed
            {
                sapling.SetActive(false);
                growing.SetActive(true);
            }
        }
        else
        {
            daysNotWatered++; //if crop is not watered, increments days not watered
            if(daysNotWatered >= fruitData.daysNotWateredToWither) //if the number of consecutive non watered days for the crop to wither has been reached, mark crop as withered and harvestable
            {
                bIsWithered = true;
                bIsHarvestable = true;
                CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
                sapling.SetActive(false);
                growing.SetActive(false);
                grown.SetActive(false);
                withered.SetActive(true);
                transform.parent.GetComponent<FruitTreePlotBehaviour>().SetPlotState(FruitPlotState.Harvestable);
            }
        }
        CustomEvents.TimeCycle.OnUpdated?.Invoke();
    }

    public void Water()
    {
        bIsWatered = true;
    }

    public void Harvest()
    {
        if (!bIsHarvestable)
        {
            Debug.Log("Not ready for harvest");
        }
        else
        {

            if (bIsWithered)
            {
                SCR_CropItems deadPlant = Resources.Load<SCR_CropItems>("Items/Produce/Dead Crop");
                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(deadPlant, 2, ItemQuality.Normal);
                transform.parent.GetComponent<FruitTreePlotBehaviour>().CutDown();
            }
            else
            {
                foreach (SCR_Items item in fruitData.harvestables)
                {
                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, fruitData._minAmountOfProduce, ItemQuality.Normal);
                }
                CustomEvents.TimeCycle.OnNewDaySetup += HandleNewDay;
                growing.SetActive(true);
                grown.SetActive(false);
                daysNotWatered = 0;
                bIsHarvestable = false;
                bIsWatered = false;
                daysMatured = 2;
            }
        }
    }

    public void CutDown()
    {
        if (bIsHarvestable)
        {
            foreach (SCR_Items item in fruitData.cutDownHarvest)
            {
                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, fruitData._minAmountOfProduce, ItemQuality.Normal);
            }
            //CustomEvents.FruitTrees.OnSetPlotState?.Invoke(FruitPlotState.Empty);
            Destroy(gameObject);
        }
    }

    public void LoadData(int _daysMatured, int _daysNotWatered)
    {
        daysMatured = _daysMatured;
        daysNotWatered = _daysNotWatered;

        if (daysNotWatered >= fruitData.daysNotWateredToWither) //if the number of consecutive non watered days for the crop to wither has been reached, mark crop as withered and harvestable
        {
            bIsWithered = true;
            bIsHarvestable = true;
            CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
            sapling.SetActive(false);
            withered.SetActive(true);
            transform.parent.GetComponent<FruitTreePlotBehaviour>().SetPlotState(FruitPlotState.Harvestable);
        }
        else if (daysMatured >= fruitData.daysToFullyGrow) //if the crops growth cycle has been completed, mark as harvestable
        {
            bIsHarvestable = true;
            CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
            sapling.SetActive(false);
            grown.SetActive(true);
            transform.parent.GetComponent<FruitTreePlotBehaviour>().SetPlotState(FruitPlotState.Harvestable);

        }
        else if (daysMatured >= fruitData.daysToMiddleGrow) //sets half grown model when specific days have passed
        {
            sapling.SetActive(false);
            growing.SetActive(true);
        }
    }

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnNewDaySetup += HandleNewDay;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnNewDaySetup -= HandleNewDay;
    }
}
