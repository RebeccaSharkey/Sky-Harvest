using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CraftMachine : MonoBehaviour
{    
    [SerializeField] private Inventory craftMachineInventory;
    [HideInInspector] public int _machineID = 0;
    private Recipes currentRecipe = null;
    private List<Recipes> listOfRecipes;

    private void Awake()
    {
        //craftMachineInventory = new Inventory();
        GameObject[] craftMachines;
        craftMachines = GameObject.FindGameObjectsWithTag("CraftingMachine");
        if (craftMachines.Length == 0)
        {
            _machineID = 1;
        }
        else
        {
            //Loops through the array of Weekly task boards and sets the Task boards ID to the gameObjects place in the array
            for (int i = 0; i < craftMachines.Length; i++)
            {
                if (this.gameObject == craftMachines[i])
                {
                    _machineID = i;
                }
            }
        }
    }

    private void Open(int id)
    {
        if(_machineID == id)
        {
            CustomEvents.CraftMachine.OnSetInventory?.Invoke(craftMachineInventory, _machineID);
            CustomEvents.CraftMachine.OnToggleUI?.Invoke(true);
            listOfRecipes = (List<Recipes>)CustomEvents.CraftMachine.OnGetListOfRecipes?.Invoke();
        }
    }

    private bool AddToCraftMachine(SCR_Items newItem, int amount, int machineID, ItemQuality quality)
    {
        if(_machineID == machineID)
        {
            if (craftMachineInventory.GetInventory().Count < 2)
            {
                craftMachineInventory.AddItem(newItem, amount, quality, overrideMethod: true);
                CustomEvents.CraftMachine.OnUpdateUI?.Invoke();

                if(craftMachineInventory.GetInventory().Count == 2)
                {
                    foreach(Recipes recipe in listOfRecipes)
                    {
                        if(recipe.CheckCanMake(craftMachineInventory.GetInventory()[0], craftMachineInventory.GetInventory()[1]))
                        {
                            currentRecipe = recipe;
                            craftMachineInventory.AddItem(recipe.GetProduct(), recipe.GetProductAmount(), quality, true);
                            CustomEvents.CraftMachine.OnUpdateUI?.Invoke();
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }

    private void RemoveFromCraftMachine(InventorySlot item, int machineID)
    {
        if (_machineID == machineID)
        {
            if(craftMachineInventory.GetInventory().Count == 3)
            {
                if(item == craftMachineInventory.GetInventory()[2])
                {
                    List<InventorySlot> tempList = currentRecipe.CraftItem(craftMachineInventory.GetInventory()[0], craftMachineInventory.GetInventory()[1]);
                    craftMachineInventory.GetInventory().Clear();
                    if(tempList.Count != 0)
                    {
                        foreach (InventorySlot slot in tempList)
                        {
                            AddToCraftMachine(slot.Item, slot.Amount, _machineID, slot.Quality);
                        }
                    }
                }
                else
                {
                    craftMachineInventory.RemoveItem(craftMachineInventory.GetInventory()[2]);
                }
            }
            craftMachineInventory.RemoveItem(item);
            CustomEvents.CraftMachine.OnUpdateUI?.Invoke();
        }
    }

    private void OnEnable()
    {
        CustomEvents.CraftMachine.OnOpenCraftingMachine += Open;
        CustomEvents.CraftMachine.OnAddNewItemStack += AddToCraftMachine;
        CustomEvents.CraftMachine.OnRemoveItemStackWithSlot += RemoveFromCraftMachine;
    }

    private void OnDisable()
    {
        CustomEvents.CraftMachine.OnOpenCraftingMachine -= Open;
        CustomEvents.CraftMachine.OnAddNewItemStack -= AddToCraftMachine;
        CustomEvents.CraftMachine.OnRemoveItemStackWithSlot -= RemoveFromCraftMachine;
    }
}
