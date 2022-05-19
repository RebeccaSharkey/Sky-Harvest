using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_ItemManager : ScriptableObject
{
    public List<SCR_CropItems> allCropItems;
    public List<SCR_SeedItems> allSeedItems;
    public List<SCR_SeedPacketItems> allSeedPacketItems;
    public List<SCR_FertilizerItems> allFertilizerItems;
    public List<SO_Fish> allFishItems;
}