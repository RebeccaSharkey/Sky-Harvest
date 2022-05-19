/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPlotContainer : MonoBehaviour
{
    [SerializeField] private PlotBehaviour[] plotBehaviours;

    public IslandPlotSaveDataContainer SaveData()
    {
        IslandPlotSaveDataContainer dataContainer = new IslandPlotSaveDataContainer();

        foreach(PlotBehaviour plot in plotBehaviours)
        {
           // dataContainer.plotSaveDatas.Add(plot.SaveData());
        }

        return dataContainer;
    }

    public void LoadData(IslandPlotSaveDataContainer data)
    {
        for(int i = 0; i < plotBehaviours.Length; i++)
        {
            plotBehaviours[i].LoadData(data.plotSaveDatas[i]);
        }
    }
}
*/