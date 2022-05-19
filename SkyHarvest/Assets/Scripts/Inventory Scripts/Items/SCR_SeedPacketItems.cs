using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Seed Packet Item", menuName = "Inventory Items/Seed Packet Item")]
public class SCR_SeedPacketItems : SCR_Items
{
    public SCR_SeedPacketItems()
    {
        ItemType = ItemTypes.seedPacket;
        Stackable = false;
        IgnoreQuality = true;
    }

    //Other Variables seed packets may need...(Please Use Headers)
    [Header("Seed Pack Data")]
    [SerializeField] private List<SCR_SeedItems> contents;
    public List<SCR_SeedItems> GetContents()
    {
        return contents;
    }
}
