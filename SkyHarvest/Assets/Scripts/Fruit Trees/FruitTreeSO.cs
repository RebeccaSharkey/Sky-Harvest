using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fruit", menuName = "SkyHarvest/ScriptableObjects/FruitTreeSO")]
public class FruitTreeSO : ScriptableObject
{
    public string fruitName;
    public int daysToMiddleGrow;
    public int daysToFullyGrow;
    public int daysNotWateredToWither;

    public int _minAmountOfProduce = 1;

    [Header("Harvesting Data")]
    public SCR_Items[] harvestables;

    public SCR_Items[] cutDownHarvest;

}
