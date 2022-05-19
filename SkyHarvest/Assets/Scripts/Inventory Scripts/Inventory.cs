using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    const int MAX_SLOT_AMOUNT = 999;
    [SerializeField] private List<InventorySlot> _inventory;
    public List<InventorySlot> GetInventory()
    {
        return _inventory;
    }

    public Inventory(List<InventorySlot> invList = null)
    {
        _inventory = new List<InventorySlot>();
        if(invList != null)
        {
            _inventory = invList;
        }
    }

    private InventorySlot GetInventorySlot(SCR_Items currentItem, ItemQuality quality)
    {
        foreach (InventorySlot invSlot in _inventory)
        {
            if (invSlot.Item == currentItem && invSlot.Quality == quality)
            {
                return invSlot;
            }
        }
        return null;
    }

    /*private InventorySlot GetNextInventorySlot(InventorySlot currentSlot, SCR_Items currentItem)
    {
        bool currentSlotFound = false;
        foreach (InventorySlot invSlot in _inventory)
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
                    if (invSlot.Amount < MAX_SLOT_AMOUNT)
                    {
                        return invSlot;
                    }
                }
            }
        }
        return null;
    }*/

    public void AddItem(SCR_Items newItem = null, int newAmount = 1, ItemQuality newQuality = ItemQuality.Normal, bool overrideMethod = false)
    {
        if (GetInventorySlot(newItem, newQuality) == null || !GetInventorySlot(newItem, newQuality).Item.Stackable)
        {
            if (newItem.Stackable)
            {
                if(newAmount > newItem.StackAmount)
                {
                    while(newAmount > 0)
                    {
                        if(newAmount > newItem.StackAmount)
                        {
                            _inventory.Add(new InventorySlot(newItem, newQuality, newItem.StackAmount));
                            newAmount -= newItem.StackAmount;
                        }
                        else
                        {
                            _inventory.Add(new InventorySlot(newItem, newQuality, newAmount));
                            newAmount = 0;
                        }
                    }
                }
                else
                {
                    _inventory.Add(new InventorySlot(newItem, newQuality, newAmount));
                }
            }
            else
            {
                for (int i = 1; i <= newAmount; i++)
                {
                    _inventory.Add(new InventorySlot(newItem));
                }
            }
        }
        else
        {
            if (!overrideMethod)
            {
                AddToItem(newItem, newQuality, newAmount);
            }
            else
            {
                _inventory.Add(new InventorySlot(newItem, newQuality, newAmount));
            }
        }
    }

    private List<InventorySlot> GetAllSlotsWithItem(SCR_Items selectedItem, ItemQuality quality)
    {
        List<InventorySlot> tempList = new List<InventorySlot>();

        foreach (InventorySlot slot in _inventory)
        {
            if (slot.Item == selectedItem && slot.Quality == quality)
            {
                tempList.Add(slot);
            }
        }

        return tempList;
    }

    public void AddToItem(SCR_Items currentItem, ItemQuality quality, int amountAdded = 1)
    {
        InventorySlot tempSlot = GetInventorySlot(currentItem, quality);
        if (tempSlot != null)
        {
            AddToItem(tempSlot, quality, amountAdded);
        }
        else
        {
            AddItem(currentItem, amountAdded, quality, true);
        }
    }

    public void AddToItem(InventorySlot invSlot, ItemQuality quality, int amountAdded = 1)
    {
        foreach (InventorySlot slot in _inventory)
        {
            if (slot == invSlot)
            {
                if (slot.Item != null)
                {
                    if (slot.Item.Stackable)
                    {
                        for (int i = 1; i <= amountAdded; i++)
                        {
                            if ((slot.Amount + 1) <= slot.Item.StackAmount)
                            {
                                slot.Amount++;
                            }
                            else
                            {
                                List<InventorySlot> tempList = new List<InventorySlot>();
                                tempList = GetAllSlotsWithItem(slot.Item, quality);
                                if (tempList.Count == 1)
                                {
                                    AddItem(slot.Item, newQuality: slot.Quality, overrideMethod: true);
                                }
                                else
                                {
                                    foreach (InventorySlot checkSlot in tempList)
                                    {
                                        if (checkSlot.Amount < slot.Item.StackAmount)
                                        {
                                            checkSlot.Amount++;
                                            break;
                                        }

                                        if (checkSlot == tempList[tempList.Count - 1])
                                        {
                                            AddItem(slot.Item, newQuality: slot.Quality, overrideMethod: true);
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        AddItem(slot.Item, amountAdded, quality, true);
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning("Nothing in this Inventory slot");
                    return;
                }
                return;
            }
        }

        Debug.LogWarning("Inventory slot not in this Inventory");

    }

    public void RemoveFromItem(SCR_Items currentItem, ItemQuality quality, int amountRemoved = 1)
    {
        if (GetInventorySlot(currentItem, quality) != null)
        {
            RemoveFromItem(GetInventorySlot(currentItem, quality), quality, amountRemoved);
        }
        else
        {
            Debug.LogWarning("This Item is not in inventory");
        }
    }

    public void RemoveFromItem(InventorySlot invSlot, ItemQuality quality, int amountRemoved = 1)
    {       

        foreach (InventorySlot slot in _inventory)
        {
            if (slot == invSlot)
            {
                if (slot.Item != null)
                {
                    List<InventorySlot> tempList = new List<InventorySlot>();
                    tempList = GetAllSlotsWithItem(invSlot.Item, quality);

                    int tempValue = 0;
                    foreach(InventorySlot tempSlot in tempList)
                    {
                        tempValue += tempSlot.Amount;
                    }

                    if(tempValue >= amountRemoved)
                    {
                        for (int i = 1; i <= amountRemoved; i++)
                        {
                            if ((slot.Amount - 1) == 0)
                            {
                                if (i != amountRemoved)
                                {
                                    RemoveItem(slot);
                                    tempList = GetAllSlotsWithItem(invSlot.Item, quality);
                                    InventorySlot tempSlot = tempList[tempList.Count - 1];
                                    RemoveFromItem(tempSlot, quality, (amountRemoved - i));
                                    return;
                                }
                                else
                                {
                                    RemoveItem(slot);
                                    return;
                                }
                            }
                            else
                            {
                                slot.DecreaseAmount();
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("You are trying to take more items than the inventory holds");
                        return;
                    }                    
                }
                else
                {
                    Debug.LogWarning("Nothing in this Inventory slot");
                    return;
                }
                return;
            }
        }

        Debug.LogWarning("Inventory slot not in this Inventory");
    }

    public void RemoveItem(SCR_Items selectedItem, ItemQuality quality)
    {
        RemoveItem(GetInventorySlot(selectedItem, quality));
    }

    public void RemoveItem(InventorySlot invSlot)
    {
        _inventory.Remove(invSlot);
    }

    public void SwapItemSlots(InventorySlot slotOne, InventorySlot slotTwo)
    {
        int temp = _inventory.IndexOf(slotTwo);
        _inventory[_inventory.IndexOf(slotOne)] = slotTwo;
        _inventory[temp] = slotOne;
    }

    public void MoveSlots(InventorySlot slotToMove, InventorySlot slotToMoveTo)
    {
        if(_inventory.IndexOf(slotToMoveTo) > _inventory.IndexOf(slotToMove))
        {
            _inventory.Remove(slotToMove);
            _inventory.Insert(_inventory.IndexOf(slotToMoveTo) + 1, slotToMove);
        }
        else
        {
            _inventory.Remove(slotToMove);
            _inventory.Insert(_inventory.IndexOf(slotToMoveTo), slotToMove);
        }
    }

    public void MergeSlots(InventorySlot slotToMerge, InventorySlot slotToMergeTo)
    {
        if((slotToMergeTo.Amount + slotToMerge.Amount) > slotToMergeTo.Item.StackAmount)
        {
            int temp = (slotToMergeTo.Amount + slotToMerge.Amount) - slotToMergeTo.Item.StackAmount;
            slotToMergeTo.Amount = slotToMergeTo.Item.StackAmount;
            AddItem(slotToMerge.Item, temp, slotToMerge.Quality, true);
        }
        else
        {
            slotToMergeTo.Amount += slotToMerge.Amount;
        }
    }

    public void ClearInventory()
    {
        _inventory.Clear();
    }
}

public enum ItemQuality
{
    Bad,
    Normal,
    Good,
    Perfect
}

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private string _name;
    [HideInInspector] public string Name { get => _name; set => _name = value; }

    [SerializeField] private SCR_Items _item;
    [HideInInspector] public SCR_Items Item { get => _item; set => _item = value; }

    [SerializeField] private int _amount;
    [HideInInspector] public int Amount { get => _amount; set => _amount = value; }

    [SerializeField] private ItemQuality _qaulity;
    [HideInInspector] public ItemQuality Quality { get => _qaulity; set => _qaulity = value; }


    public InventorySlot(SCR_Items newItem = null, ItemQuality quality = ItemQuality.Normal, int newAmount = 1)
    {
        _item = newItem;
        if (_item != null)
        {
            _name = newItem.ItemName;
            if (newAmount <= 0)
            {
                _amount = 1;
            }
            else
            {
                _amount = newAmount;
            }
        }
        else
        {
            _name = "Null";
            _amount = 0;
        }
        _qaulity = quality;
    }

    public void IncreaseAmount(int amountAdded = 1)
    {
        _amount += amountAdded;
    }

    public void DecreaseAmount(int amountRemoved = 1)
    {
        _amount -= amountRemoved;
    }

}