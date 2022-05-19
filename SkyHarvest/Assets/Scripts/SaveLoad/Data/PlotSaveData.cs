using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlotSaveData
{
    public bool isBlocked;
    //public bool isPlanted;
    public FertiliserState fertiliserState;
    public CropSaveData cropSaveData;

    public PlotSaveData(bool _isBlocked, FertiliserState _fertiliserState, CropSaveData _cropSaveData)
    {
        isBlocked = _isBlocked;
        fertiliserState = _fertiliserState;
        cropSaveData = _cropSaveData;
    }
}

[System.Serializable]
public class CropSaveData
{
    public string cropPrefabToLoad;
    public int daysMatured;
    public int daysNotWatered;

    public CropSaveData(string _cropPrefabToLoad, int _daysMatured, int _daysNotWatered)
    {
        cropPrefabToLoad = _cropPrefabToLoad;
        daysMatured = _daysMatured;
        daysNotWatered = _daysNotWatered;
    }
}

[System.Serializable]
public class IslandPlotSaveDataContainer
{
    public List<PlotSaveData> plotSaveDatas;

    public IslandPlotSaveDataContainer()
    {
        plotSaveDatas = new List<PlotSaveData>();
    }
}

[System.Serializable]
public class CentralPlotSaveDataContainer
{
    public List<IslandPlotSaveDataContainer> islandPlotSaveDataContainers;

    public CentralPlotSaveDataContainer()
    {
        islandPlotSaveDataContainers = new List<IslandPlotSaveDataContainer>();
    }
}
