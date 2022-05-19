using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Animal Produce Item", menuName = "Inventory Items/Animal Produce Item")]
public class SCR_AnimalProduce : SCR_Items
{
    public SCR_AnimalProduce()
    {
        ItemType = ItemTypes.animalProduce;
        Stackable = true;
        StackAmount = 999;
        IgnoreQuality = false;
    }
}
