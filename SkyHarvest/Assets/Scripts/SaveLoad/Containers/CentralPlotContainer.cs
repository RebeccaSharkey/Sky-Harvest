using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class CentralPlotContainer : MonoBehaviour
{
    [SerializeField] private IslandPlotContainer[] islandPlotContainers;

    private void OnEnable()
    {
        CustomEvents.SaveSystem.OnSavePlotData += SaveData;
        //CustomEvents.SaveSystem.OnDataLoaded += LoadData;
    }

    private void OnDisable()
    {
        CustomEvents.SaveSystem.OnSavePlotData -= SaveData;
        //CustomEvents.SaveSystem.OnDataLoaded += LoadData;
    }

    private CentralPlotSaveDataContainer SaveData()
    {
        CentralPlotSaveDataContainer data = new CentralPlotSaveDataContainer();

        foreach(IslandPlotContainer islandPlotContainer in islandPlotContainers)
        {
            data.islandPlotSaveDataContainers.Add(islandPlotContainer.SaveData());
        }

        return data;
    }

    /*private void LoadData()
    {
        Debug.Log("Loading data to crops");

        if (SaveLoadManager.loadedData.plotData == null)
        {
            Debug.LogError("No plot data in save data");
            return;
        }
        CentralPlotSaveDataContainer data = SaveLoadManager.loadedData.plotData;

        for(int i = 0; i < islandPlotContainers.Length; i++)
        {
            islandPlotContainers[i].LoadData(data.islandPlotSaveDataContainers[i]);
        }
    }
}
*/