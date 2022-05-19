using System;
using UnityEngine;

public enum FruitPlotState{Empty, Unwatered, Watered, Harvestable}

public class FruitTreePlotBehaviour : MonoBehaviour, iSaveable
{
    private FruitPlotState state = FruitPlotState.Empty;
    
    private GameObject waterParticleSystem;
    [SerializeField] private Vector3 offset;

    private bool bIsPlanted = false;
    private FruitBehaviour plantedFruitTree;

    private void Start()
    {
        waterParticleSystem = Resources.Load<GameObject>("Prefabs/Particle Effects/WaterDropsFX");
    }

    public void Plant(string _name)
    {
        bIsPlanted = true;
        state = FruitPlotState.Unwatered;
        GameObject temp = Instantiate(Resources.Load<GameObject>($"Prefabs/Crops/Fruit Trees/{_name}"), transform.position, Quaternion.Euler(-90f, 0f, 0f), transform);
        plantedFruitTree = temp.GetComponent<FruitBehaviour>();
        CustomEvents.Crops.OnCropPlanted?.Invoke();
    }

    public void Water()
    {
        plantedFruitTree.Water();
        GameObject temp = Instantiate(waterParticleSystem, transform.position + offset, Quaternion.identity);
        state = FruitPlotState.Watered;
        Destroy(temp, 1f);
        CustomEvents.Crops.OnCropWatered?.Invoke();
    }
    
    public void Harvest()
    {
        state = FruitPlotState.Unwatered;
        plantedFruitTree.Harvest();
    }

    public void CutDown()
    {
        plantedFruitTree.CutDown();
        bIsPlanted = false;
        plantedFruitTree = null;
        state = FruitPlotState.Empty;
    }

    public bool CheckTreeWithered()
    {
        return plantedFruitTree.bIsWithered;
    }

    public FruitPlotState GetFruitPlotState()
    {
        return state;
    }

    public void SetPlotState(FruitPlotState _state)
    {
        state = _state;
    }

    /// <summary>
    /// Handles gathering and saving data relating to specific plot
    /// </summary>
    /// <returns>Populated PlotSaveData object</returns>
    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        if (plantedFruitTree != null)
        {
            data.Add(plantedFruitTree.gameObject.name.Substring(0, plantedFruitTree.gameObject.name.Length - 7));
            data.Add(plantedFruitTree.daysMatured.ToString());
            data.Add(plantedFruitTree.daysNotWatered.ToString());
        }
        else
        {
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
        if (_data[0] != "null") Plant(_data[0]);

        if (bIsPlanted)
        {
            state = FruitPlotState.Unwatered;
            plantedFruitTree.LoadData(int.Parse(_data[1]), int.Parse(_data[2]));
        }
    }
}
