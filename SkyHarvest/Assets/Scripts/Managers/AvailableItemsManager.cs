using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AvailableItemsManager : MonoBehaviour, iSaveable
{
    [SerializeField] private List<SCR_Items> shopsAvailableItems;
    [SerializeField] private List<SCR_Items> specialShopsAvailableItems;
    [SerializeField] private List<SCR_Items> weeklyTasksAvaialbleItems;

    private void AddItemToShops(SCR_Items newItem)
    {
        if (!shopsAvailableItems.Contains(newItem))
        {
            shopsAvailableItems.Add(newItem);
        }
    }
    private void RemoveFromShops(SCR_Items item)
    {
        if (shopsAvailableItems.Contains(item))
        {
            shopsAvailableItems.Remove(item);
        }
    }
    private List<SCR_Items> GetShopsAvailabileItems()
    {
        return shopsAvailableItems;
    }

    private void AddItemToSpecialShops(SCR_Items newItem)
    {
        if (!specialShopsAvailableItems.Contains(newItem))
        {
            specialShopsAvailableItems.Add(newItem);
        }
    }
    private void RemoveFromSpecialShops(SCR_Items item)
    {
        if (specialShopsAvailableItems.Contains(item))
        {
            specialShopsAvailableItems.Remove(item);
        }
    }
    private List<SCR_Items> GetSpecialShopsAvailabileItems()
    {
        return specialShopsAvailableItems;
    }

    private void AddItemToWeeklyTasks(SCR_Items newItem)
    {
        if (!weeklyTasksAvaialbleItems.Contains(newItem))
        {
            weeklyTasksAvaialbleItems.Add(newItem);
        }
    }
    private void RemoveFromWeeklyTasks(SCR_Items item)
    {
        if (weeklyTasksAvaialbleItems.Contains(item))
        {
            weeklyTasksAvaialbleItems.Remove(item);
        }
    }
    private List<SCR_Items> GetWeeklyTasksAvailabileItems()
    {
        return weeklyTasksAvaialbleItems;
    }

    private void OnEnable()
    {
        CustomEvents.AvailableItems.OnAddToShops += AddItemToShops;
        CustomEvents.AvailableItems.OnRemoveFromShops += RemoveFromShops;
        CustomEvents.AvailableItems.OnGetShopList += GetShopsAvailabileItems;

        CustomEvents.AvailableItems.OnAddToSpecialShop += AddItemToSpecialShops;
        CustomEvents.AvailableItems.OnRemoveFromSpecialShops += RemoveFromSpecialShops;
        CustomEvents.AvailableItems.OnGetSpecialShopList += GetSpecialShopsAvailabileItems;

        CustomEvents.AvailableItems.OnAddToWeeklyTasks += AddItemToWeeklyTasks;
        CustomEvents.AvailableItems.OnRemoveFromWeeklyTasks += RemoveFromWeeklyTasks;
        CustomEvents.AvailableItems.OnGetWeeklyTasks += GetWeeklyTasksAvailabileItems;
    }
    private void OnDisable()
    {
        CustomEvents.AvailableItems.OnAddToShops -= AddItemToShops;
        CustomEvents.AvailableItems.OnRemoveFromShops -= RemoveFromShops;
        CustomEvents.AvailableItems.OnGetShopList -= GetShopsAvailabileItems;

        CustomEvents.AvailableItems.OnAddToSpecialShop -= AddItemToSpecialShops;
        CustomEvents.AvailableItems.OnRemoveFromSpecialShops -= RemoveFromSpecialShops;
        CustomEvents.AvailableItems.OnGetSpecialShopList -= GetSpecialShopsAvailabileItems;

        CustomEvents.AvailableItems.OnAddToWeeklyTasks -= AddItemToWeeklyTasks;
        CustomEvents.AvailableItems.OnRemoveFromWeeklyTasks -= RemoveFromWeeklyTasks;
        CustomEvents.AvailableItems.OnGetWeeklyTasks -= GetWeeklyTasksAvailabileItems;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        string tempString = "";
        int iteration = 0;
        foreach (SCR_Items item in shopsAvailableItems)
        {
            iteration++;
            tempString += item.name.ToString();
            if (iteration < shopsAvailableItems.Count)
            {
                tempString += "#";
            }
        }
        data.Add(tempString);

        tempString = "";
        iteration = 0;
        foreach (SCR_Items item in specialShopsAvailableItems)
        {
            iteration++;
            tempString += item.name.ToString();
            if (iteration < specialShopsAvailableItems.Count)
            {
                tempString += "#";
            }
        }
        data.Add(tempString);

        tempString = "";
        iteration = 0;
        foreach (SCR_Items item in weeklyTasksAvaialbleItems)
        {
            iteration++;
            tempString += item.name.ToString();
            if (iteration < weeklyTasksAvaialbleItems.Count)
            {
                tempString += "#";
            }
        }
        data.Add(tempString);

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        if (_data.Count <= 0) return;

        shopsAvailableItems.Clear();
        string[] tempStringArray = _data[0].Split('#');
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
                Debug.LogError($"Null SO object for {item}");
                continue;
            }

            shopsAvailableItems.Add(thisItem);
        }

        specialShopsAvailableItems.Clear();
        string[] tempStringArray2 = _data[1].Split('#');
        foreach (string item in tempStringArray2)
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
                Debug.LogError($"Null SO object for {item}");
                continue;
            }

            specialShopsAvailableItems.Add(thisItem);
        }

        weeklyTasksAvaialbleItems.Clear();
        string[] tempStringArray3 = _data[2].Split('#');
        foreach (string item in tempStringArray3)
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
                Debug.LogError($"Null SO object for {item}");
                continue;
            }

            weeklyTasksAvaialbleItems.Add(thisItem);
        }

    }
}
