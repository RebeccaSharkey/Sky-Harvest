using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipes
{
    [SerializeField] private SCR_Items componentOne;
    [HideInInspector] public SCR_Items ComponentOne { get => componentOne; }
    [SerializeField] private SCR_Items componentTwo;
    [HideInInspector] public SCR_Items ComponentTwo { get => componentTwo; }
    [SerializeField] private SCR_Items product;
    [HideInInspector] public SCR_Items Product { get => product; }

    [SerializeField] private int componentOneAmount = 1;
    [HideInInspector] public int ComponentOneAmount { get => componentOneAmount; }
    [SerializeField] private int componentTwoAmount = 1;
    [HideInInspector] public int ComponentTwoAmount { get => componentTwoAmount; }
    [SerializeField] private int productAmount = 1;
    [HideInInspector] public int ProductAmount { get => productAmount; }

    private List<KeyValuePair<SCR_Items, int>> components = new List<KeyValuePair<SCR_Items, int>>();

    public Recipes()
    {
        componentOne = null;
        componentTwo = null;
        product = null;
        componentOneAmount = 1;
        componentTwoAmount = 1;
        productAmount = 1;
    }

    public Recipes(SCR_Items compOne, SCR_Items compTwo, SCR_Items prod, int compOneAmount, int compTwoAmount, int prodAmount)
    {
        componentOne = compOne;
        componentTwo = compTwo;
        product = prod;
        componentOneAmount = compOneAmount;
        componentTwoAmount = compTwoAmount;
        productAmount = prodAmount;
    }

    public bool CheckCanMake(InventorySlot itemOne, InventorySlot itemTwo)
    {
        if (itemOne.Item == componentOne)
        {
            if (itemTwo.Item == componentTwo)
            {
                if(itemOne.Amount >= componentOneAmount && itemTwo.Amount >= componentTwoAmount)
                {
                    return true;
                }
                else if(componentOne == componentTwo)
                {
                    if (itemTwo.Amount >= componentOneAmount && itemOne.Amount >= componentTwoAmount)
                    {
                        return true;
                    }
                }
            }
        }
        else if (itemOne.Item == componentTwo)
        {
            if(itemTwo.Item == componentOne)
            {
                if (itemOne.Amount >= componentTwoAmount && itemTwo.Amount >= componentOneAmount)
                    return true;
            }
        }
        return false;
    }

    public SCR_Items GetProduct()
    {
        return product;
    }

    public int GetProductAmount()
    {
        return productAmount;
    }

    public List<KeyValuePair<SCR_Items, int>> GetComponents()
    {
        if (components.Count > 0)
        {
            components.Clear();
        }
        components.Add(new KeyValuePair<SCR_Items, int>(componentOne, componentOneAmount));
        components.Add(new KeyValuePair<SCR_Items, int>(componentTwo, componentTwoAmount));
        return components;
    }

    public List<InventorySlot> CraftItem(InventorySlot itemOne, InventorySlot itemTwo)
    {
        List<InventorySlot> tempList = new List<InventorySlot>();

        if (itemOne.Item == componentOne)
        {
            if (itemTwo.Item == componentTwo)
            {
                if (itemOne.Amount >= componentOneAmount && itemTwo.Amount >= componentTwoAmount)
                {
                    if(itemOne.Amount - componentOneAmount != 0)
                    {
                        tempList.Add(new InventorySlot(itemOne.Item, itemOne.Quality, itemOne.Amount - componentOneAmount));
                    }
                    if(itemTwo.Amount - componentTwoAmount != 0)
                    {
                        tempList.Add(new InventorySlot(itemTwo.Item, itemTwo.Quality, itemTwo.Amount - componentTwoAmount));
                    }
                    return tempList;
                }
                else if (componentOne == componentTwo)
                {
                    if (itemTwo.Amount >= componentOneAmount && itemOne.Amount >= componentTwoAmount)
                    {
                        if (itemOne.Amount - componentTwoAmount != 0)
                        {
                            tempList.Add(new InventorySlot(itemOne.Item, itemOne.Quality, itemOne.Amount - componentTwoAmount));
                        }
                        if (itemTwo.Amount - componentOneAmount != 0)
                        {
                            tempList.Add(new InventorySlot(itemTwo.Item, itemTwo.Quality, itemTwo.Amount - componentOneAmount));
                        }
                        return tempList;
                    }
                }
            }
        }
        else if (itemOne.Item == componentTwo)
        {
            if (itemTwo.Item == componentOne)
            {
                if (itemOne.Amount >= componentTwoAmount && itemTwo.Amount >= componentOneAmount)
                {
                    if (itemOne.Amount - componentTwoAmount != 0)
                    {
                        tempList.Add(new InventorySlot(itemOne.Item, itemOne.Quality, itemOne.Amount - componentTwoAmount));
                    }
                    if (itemTwo.Amount - componentOneAmount != 0)
                    {
                        tempList.Add(new InventorySlot(itemTwo.Item, itemTwo.Quality, itemTwo.Amount - componentOneAmount));
                    }
                    return tempList;
                }
            }
        }

        return tempList;
    }
}
