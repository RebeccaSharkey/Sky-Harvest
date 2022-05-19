using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fertilizer Item", menuName = "Inventory Items/Fertilizer Item")]
public class SCR_FertilizerItems : SCR_Items
{
    public SCR_FertilizerItems()
    {
        ItemType = ItemTypes.fertilizer;
        Stackable = true;
        StackAmount = 999;
        IgnoreQuality = true;
    }
}
