using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Item", menuName = "Inventory Items/Fish Item")]
public class SO_Fish : SCR_Items
{
    SO_Fish()
    { 
        ItemType = ItemTypes.fish;
        Stackable = true;
        StackAmount = 5;
        IgnoreQuality = false;
    }

    [Header("Fish Items Only")]
    [SerializeField] private string lore;
    public string Lore { get => lore; }
}
