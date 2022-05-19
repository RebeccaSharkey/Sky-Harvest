using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Seed Item", menuName = "Inventory Items/Seed Item")]
public class SCR_SeedItems : SCR_Items
{
    public SCR_SeedItems()
    {
        ItemType = ItemTypes.seed;
        Stackable = true;
        StackAmount = 999;
        IgnoreQuality = true;
    }
    //Other Variables seeds may need...(Please Use Headers)
}
