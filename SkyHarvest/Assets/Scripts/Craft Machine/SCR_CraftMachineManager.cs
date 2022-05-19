using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CraftMachineManager : MonoBehaviour, iSaveable
{
    [SerializeField] private List<Recipes> listOfRecipes;

    private void AddRecipe(Recipes newRecipe)
    {
        listOfRecipes.Add(newRecipe);
    }

    private void RemoveRecipe(Recipes recipe)
    {
        for(int i = 0; i < listOfRecipes.Count; i++)
        {
            if(listOfRecipes[0] == recipe)
            {
                listOfRecipes.Remove(recipe);
                break;
            }
        }
    }

    private List<Recipes> GetRecipes()
    {
        return listOfRecipes;
    }

    private void OnEnable()
    {
        CustomEvents.CraftMachine.OnAddToList += AddRecipe;
        CustomEvents.CraftMachine.OnRemoveFromList += RemoveRecipe;
        CustomEvents.CraftMachine.OnGetListOfRecipes += GetRecipes;
    }

    private void OnDisable()
    {
        CustomEvents.CraftMachine.OnAddToList -= AddRecipe;
        CustomEvents.CraftMachine.OnRemoveFromList -= RemoveRecipe;
        CustomEvents.CraftMachine.OnGetListOfRecipes -= GetRecipes;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        foreach(Recipes recipe in listOfRecipes)
        {
            data.Add($"{recipe.ComponentOne.name}#{recipe.ComponentTwo.name}#{recipe.Product.name}#{recipe.ComponentOneAmount.ToString()}" +
                $"#{recipe.ComponentTwoAmount.ToString()}#{recipe.ProductAmount.ToString()}");
        }

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        if (_data.Count <= 0) return;

        listOfRecipes.Clear();

        foreach (string recipe in _data)
        {
            string[] tempStringArray = recipe.Split('#');

            List<SCR_Items> tempItem = new List<SCR_Items>();
            List<int> tempAmounts = new List<int>();
            foreach (string item in tempStringArray)
            {
                if (string.IsNullOrEmpty(item)) break;

                SCR_Items thisItem = Resources.Load<SCR_Items>($"Items/Fertilizer/{item.ToString()}");
                if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/Fish/{item.ToString()}");
                if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/Produce/{item.ToString()}");
                if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/SeedPackets/{item.ToString()}");
                if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/Seeds/{item.ToString()}");
                if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/Unique Items/{item.ToString()}");
                if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/Unique Items/Recipes/{item.ToString()}");
                if (thisItem == null)
                {
                    tempAmounts.Add(int.Parse(item));
                    continue;
                }

                tempItem.Add(thisItem);
            }

            Recipes newRecipe = new Recipes(tempItem[0], tempItem[1], tempItem[2], tempAmounts[0], tempAmounts[1], tempAmounts[2]);
            listOfRecipes.Add(newRecipe);
        }
    }
}
