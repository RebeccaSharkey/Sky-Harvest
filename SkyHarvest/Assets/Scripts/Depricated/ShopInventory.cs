using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopInventory
{
    [SerializeField] private List<SCR_Items> itemsToSell;
    [SerializeField] private List<SCR_Items> shopList;

    private List<InventorySlot> shopInventory;
    private int itemPicker = 0;


    public ShopInventory()
    {
        shopInventory = new List<InventorySlot>();
        shopList = new List<SCR_Items>();
        itemsToSell = new List<SCR_Items>();
    }

    void Awake()
    {
        shopInventory.Add(new InventorySlot(shopList[0]));
    }

    //Returns the inventory data
    public List<InventorySlot> GetInventory()
    {
        return shopInventory;
    }

    //Checks each inventory slot in the shop inventory and returns it
    private InventorySlot GetInventorySlot(SCR_Items currentItem)
    {
        foreach (InventorySlot invSlot in shopInventory)
        {
            if (invSlot.Item == currentItem)
            {
                return invSlot;
            }
        }
        return null;
    }

    private InventorySlot GetNextInventorySlot(InventorySlot currentSlot, SCR_Items currentItem)
    {
        bool currentSlotFound = false;
        foreach (InventorySlot invSlot in shopInventory)
        {
            if (!currentSlotFound)
            {
                if (invSlot == currentSlot)
                {
                    currentSlotFound = true;
                }
            }
            else
            {
                if (invSlot.Item == currentItem)
                {
                    if (invSlot.Amount < 999)
                    {
                        return invSlot;
                    }
                }
            }
        }
        return null;
    }

    //public void BuyItem(SCR_Items item)
    //{
    //    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, item.StackAmount);
    //}

    //public void SellItem(SCR_Items item)
    //{
    //    currency.AddAmount(item.SellValue);
    //    CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(item, item.StackAmount);
    //}

    public void AddItem(SCR_Items item)
    {
        shopInventory.Add(new InventorySlot(item, ItemQuality.Normal, 1));
    }

    public void AddItemsToShop()
    {
        foreach(SCR_Items item in shopList)
        {
            shopInventory.Add(new InventorySlot(item, ItemQuality.Normal, 1));
        }
    }

    public void ResetShop()
    {
        if (itemsToSell.Count > 0)
        {
            for (int i = 0; i < shopList.Count; i++)
            {
                itemsToSell.RemoveAt(i);
            }
        }
    }

    public List<SCR_Items> RandomiseShop()
    {
        for (int i = 0; i < itemsToSell.Count; i++)
        {
            itemPicker = Random.Range(0, itemsToSell.Count);
            SCR_Items itemPicked = itemsToSell[itemPicker];
            shopList.Add(itemPicked);
        }

        return shopList;
    }
}