using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Manager", menuName = "Inventory Items/Item Manager")]
public class ItemManager : SO_ItemManager 
{    

    public void AddToList(SCR_Items newItem)
    {
        switch(newItem.ItemType)
        {
            case ItemTypes.crop:
                if(!allCropItems.Contains((SCR_CropItems)newItem))
                {
                    allCropItems.Add((SCR_CropItems)newItem);
                }
                break;
            case ItemTypes.seed:
                if (!allSeedItems.Contains((SCR_SeedItems)newItem))
                {
                    allSeedItems.Add((SCR_SeedItems)newItem);
                }
                break;
            case ItemTypes.treeSeeds:
                if (!allSeedItems.Contains((SCR_SeedItems)newItem))
                {
                    allSeedItems.Add((SCR_SeedItems)newItem);
                }
                break;
            case ItemTypes.fertilizer:
                if(!allFertilizerItems.Contains((SCR_FertilizerItems)newItem))
                {
                    allFertilizerItems.Add((SCR_FertilizerItems)newItem);
                }
                break;
            case ItemTypes.seedPacket:
                if(!allSeedPacketItems.Contains((SCR_SeedPacketItems)newItem))
                {
                    allSeedPacketItems.Add((SCR_SeedPacketItems)newItem);
                }
                break;
            case ItemTypes.fish:
                if(!allFishItems.Contains((SO_Fish)newItem))
                {
                    allFishItems.Add((SO_Fish)newItem);
                }
                break;
        }            
    }

    public void Refresh()
    {
        allCropItems = new List<SCR_CropItems>();
        SCR_CropItems[] cropItems = Resources.LoadAll<SCR_CropItems>("Items/Produce");
        foreach (SCR_CropItems cropItem in cropItems)
        {
            AddToList(cropItem);
        }

        allSeedItems = new List<SCR_SeedItems>();
        SCR_SeedItems[] seedItems = Resources.LoadAll<SCR_SeedItems>("Items/Seeds");
        foreach (SCR_SeedItems seedItem in seedItems)
        {
            AddToList(seedItem);
        }

        allSeedPacketItems = new List<SCR_SeedPacketItems>();
        SCR_SeedPacketItems[] seedPacketItems = Resources.LoadAll<SCR_SeedPacketItems>("Items/SeedPackets");
        foreach (SCR_SeedPacketItems seedPacketItem in seedPacketItems)
        {
            AddToList(seedPacketItem);
        }

        allFertilizerItems = new List<SCR_FertilizerItems>();
        SCR_FertilizerItems[] fertilizerItems = Resources.LoadAll<SCR_FertilizerItems>("Items/Fertilizer");
        foreach (SCR_FertilizerItems fertilizerItem in fertilizerItems)
        {
            AddToList(fertilizerItem);
        }

        allFishItems = new List<SO_Fish>();
        SO_Fish[] fishItems = Resources.LoadAll<SO_Fish>("Items/Fish");
        foreach (SO_Fish fishItem in fishItems)
        {
            AddToList(fishItem);
        }
    }
}