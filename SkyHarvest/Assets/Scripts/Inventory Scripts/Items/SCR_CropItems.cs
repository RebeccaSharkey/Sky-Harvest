using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Crop Item", menuName = "Inventory Items/Crop Item")]
public class SCR_CropItems : SCR_Items
{
    public SCR_CropItems()
    {
        ItemType = ItemTypes.crop;
        Stackable = true;
        StackAmount = 999;
        IgnoreQuality = false;
    }

    [Header("Crop Items Only")]
    [SerializeField] private string lore;
    public string Lore { get => lore; }

    //Other Variables crops may need...(Please Use Headers)

}
