using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Bags : MonoBehaviour
{
    private Inventory bagInventory;
    private int _bagID;
    public int BagID { get => _bagID; set => _bagID = value; }
    private int daysAlive = 0;

    private void Awake()
    {
        //Makes sure the bag has a tag of "Bag" so that it can be used with the players task scripts.
        gameObject.tag = "Bag";

        //Creates a new inventory for the bag to store items in.
        bagInventory = new Inventory();
    }

    private void Start()
    {
        //Gives the bags UI the inventory to work from
        CustomEvents.InventorySystem.BagInventory.OnGetInventoryData?.Invoke(bagInventory, _bagID);
    }

    /* 
     *      All Scripts Below self explanitory.
     * 
     *      if(_bagID = bagID) used to check if this bag needs to listen to the event or can ignore it. * 
    */

    private void OnOpen(int id)
    {
        if (id == _bagID)
        {
            CustomEvents.InventorySystem.BagInventory.OnGetInventoryData?.Invoke(bagInventory, _bagID);
            CustomEvents.InventorySystem.BagInventory.ToggleUI?.Invoke(true, _bagID);
        }
    }

    private void AddNewItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal, int bagID = -1)
    {

        if (_bagID == bagID)
        {
            bagInventory.AddItem(newItem, newAmount, quality);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(_bagID);
        }
    }

    private void AddNewSplitItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal, int bagID = -1)
    {
        if (_bagID == bagID)
        {
            bagInventory.AddItem(newItem, newAmount, quality, true);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(_bagID);
        }
    }

    private void RemoveItemStack(SCR_Items selectedItem, int bagID, ItemQuality quality)
    {
        if (_bagID == bagID)
        {
            bagInventory.RemoveItem(selectedItem, quality);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(_bagID);
            if (bagInventory.GetInventory().Count == 0)
            {
                CustomEvents.InventorySystem.BagInventory.ToggleUI?.Invoke(false, _bagID);
                CustomEvents.InventorySystem.PlayerInventory.OnDeleteBag?.Invoke(gameObject);
                CustomEvents.TimeCycle.OnUnpause?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void AddToStack(SCR_Items selectedItem, int addedAmount, int bagID, ItemQuality quality)
    {
        if (_bagID == bagID)
        {
            bagInventory.AddToItem(selectedItem, quality, addedAmount);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(_bagID);
        }
    }

    private void RemoveFromStack(SCR_Items selectedItem, int removedAmount, int bagID, ItemQuality quality)
    {
        if (_bagID == bagID)
        {
            bagInventory.RemoveFromItem(selectedItem, quality, removedAmount);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(_bagID);

            //Checks if the bag is empty, if so destroys it and unpauses game
            if (bagInventory.GetInventory().Count == 0)
            {
                CustomEvents.InventorySystem.BagInventory.ToggleUI?.Invoke(false, _bagID);
                CustomEvents.InventorySystem.PlayerInventory.OnDeleteBag?.Invoke(gameObject);
                CustomEvents.TimeCycle.OnUnpause?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void RemoveStackWithSlot(InventorySlot invSlot, int bagID)
    {
        if (_bagID == bagID)
        {
            bagInventory.RemoveItem(invSlot);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(_bagID);

            //Checks if the bag is empty, if so destroys it and unpauses game.
            if (bagInventory.GetInventory().Count == 0)
            {
                CustomEvents.InventorySystem.BagInventory.ToggleUI?.Invoke(false, _bagID);
                CustomEvents.InventorySystem.PlayerInventory.OnDeleteBag?.Invoke(gameObject);
                CustomEvents.TimeCycle.OnUnpause?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void DestroyCheck()
    {
        daysAlive++;
        Debug.Log(daysAlive);
        if(daysAlive == 2)
        {
            CustomEvents.InventorySystem.PlayerInventory.OnDeleteBag?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    private void SwapInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if(id == _bagID)
        {
            bagInventory.SwapItemSlots(slotOne, slotTwo);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void MoveInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if (id == _bagID)
        {
            bagInventory.MoveSlots(slotOne, slotTwo);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void MergeAllInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if (id == _bagID)
        {
            bagInventory.RemoveItem(slotOne);
            bagInventory.AddToItem(slotTwo, slotTwo.Quality, slotOne.Amount);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void MergeInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if(id == _bagID)
        {
            bagInventory.RemoveItem(slotOne);
            bagInventory.MergeSlots(slotOne, slotTwo);
            CustomEvents.InventorySystem.BagInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.BagInventory.OnAddNewItemStack += AddNewItem;
        CustomEvents.InventorySystem.BagInventory.OnRemoveItemStack += RemoveItemStack;
        CustomEvents.InventorySystem.BagInventory.OnAddToItemStack += AddToStack;
        CustomEvents.InventorySystem.BagInventory.OnRemoveFromItemStack += RemoveFromStack;
        CustomEvents.InventorySystem.BagInventory.OnRemoveItemStackWithSlot += RemoveStackWithSlot;
        CustomEvents.InventorySystem.BagInventory.OnSplit += AddNewSplitItem;
        CustomEvents.TimeCycle.OnDayStart += DestroyCheck;
        CustomEvents.InventorySystem.BagInventory.OnOpenBag += OnOpen;

        CustomEvents.InventorySystem.BagInventory.OnSwapInventorySlots += SwapInventorySlots;
        CustomEvents.InventorySystem.BagInventory.OnMoveSlots += MoveInventorySlots;
        CustomEvents.InventorySystem.BagInventory.OnMergeSlotsToAllOfType += MergeAllInventorySlots;
        CustomEvents.InventorySystem.BagInventory.OnMergeSlots += MergeInventorySlots;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.BagInventory.OnAddNewItemStack -= AddNewItem;
        CustomEvents.InventorySystem.BagInventory.OnRemoveItemStack -= RemoveItemStack;
        CustomEvents.InventorySystem.BagInventory.OnAddToItemStack -= AddToStack;
        CustomEvents.InventorySystem.BagInventory.OnRemoveFromItemStack -= RemoveFromStack;
        CustomEvents.InventorySystem.BagInventory.OnRemoveItemStackWithSlot -= RemoveStackWithSlot;
        CustomEvents.InventorySystem.BagInventory.OnSplit -= AddNewSplitItem;
        CustomEvents.TimeCycle.OnDayStart -= DestroyCheck;
        CustomEvents.InventorySystem.BagInventory.OnOpenBag -= OnOpen;

        CustomEvents.InventorySystem.BagInventory.OnSwapInventorySlots -= SwapInventorySlots;
        CustomEvents.InventorySystem.BagInventory.OnMoveSlots -= MoveInventorySlots;
        CustomEvents.InventorySystem.BagInventory.OnMergeSlotsToAllOfType -= MergeAllInventorySlots;
        CustomEvents.InventorySystem.BagInventory.OnMergeSlots -= MergeInventorySlots;
    }
}
