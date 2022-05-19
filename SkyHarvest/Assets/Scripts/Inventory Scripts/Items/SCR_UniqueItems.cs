using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UniqueItems
{
    _default,
    bag,
    recipe,
    fishingRod
};

[CreateAssetMenu(fileName = "New Unique Item", menuName = "Inventory Items/Unique Item")]
public class SCR_UniqueItems : SCR_Items
{
    [SerializeField] private UniqueItems _description;
    public UniqueItems Desciption { get => _description; }

    [Header("Data For Bags")]
    [SerializeField] private int incrimentAmount;

    [Header("Data For Recipes")]
    [SerializeField] private Recipes recipe;

    public SCR_UniqueItems()
    {
        ItemType = ItemTypes.unique;
        Stackable = false;
        StackAmount = 1;
        IgnoreQuality = false;
        _description = UniqueItems._default;
        incrimentAmount = 10;
    }

    public void PerformLinkedAction()
    {
        switch(_description)
        {
            case UniqueItems._default:
                Debug.Log("No Liked Action To This Item");
                break;
            case UniqueItems.bag:
                CustomEvents.InventorySystem.PlayerInventory.OnIncreaseInventorySize?.Invoke(incrimentAmount);
                CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize?.Invoke();
                break;
            case UniqueItems.recipe:
                CustomEvents.CraftMachine.OnAddToList?.Invoke(recipe);
                CustomEvents.AvailableItems.OnAddToWeeklyTasks?.Invoke(recipe.GetProduct());
                break;
            case UniqueItems.fishingRod:
                break;
        }
    }
}
