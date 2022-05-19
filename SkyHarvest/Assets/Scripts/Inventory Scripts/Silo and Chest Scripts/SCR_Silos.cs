using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Silos : MonoBehaviour, iSaveable
{
    [SerializeField] private Inventory siloInventory;
    private int _siloID = -1;
    public int SiloID { get => _siloID; set => _siloID = value; }

    public void Awake()
    {
        GameObject[] silosInGame;
        silosInGame = GameObject.FindGameObjectsWithTag("Silo");
        if (silosInGame.Length == 0)
        {
            _siloID = 1;
        }
        else
        {
            //Loops through the array of silos and sets the Task boards ID to the gameObjects place in the array
            for (int i = 0; i < silosInGame.Length; i++)
            {
                if (gameObject == silosInGame[i].gameObject)
                {
                    _siloID = i + 1;
                }
            }
        }
    }

    private void Start()
    {
        //CustomEvents.InventorySystem.SiloInventory.OnGetInventoryData?.Invoke(siloInventory, _siloID);
    }

    private void OnOpen(int id)
    {
        if(id == _siloID)
        {
            CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Silo");
            CustomEvents.InventorySystem.SiloInventory.OnGetInventoryData?.Invoke(siloInventory, _siloID);
            CustomEvents.InventorySystem.SiloInventory.ToggleUI?.Invoke(true, _siloID);
        }
    }

    private void AddNewItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal, int bagID = -1)
    {
        if (_siloID == bagID)
        {
            siloInventory.AddItem(newItem, newAmount, quality);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(_siloID);
        }
    }

    private void AddNewSplitItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal, int bagID = -1)
    {
        if (_siloID == bagID)
        {
            siloInventory.AddItem(newItem, newAmount, quality, true);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(_siloID);
        }
    }

    private void RemoveItemStack(SCR_Items selectedItem, ItemQuality quality, int bagID)
    {
        if (_siloID == bagID)
        {
            siloInventory.RemoveItem(selectedItem, quality);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(_siloID);
        }
    }

    private void AddToStack(SCR_Items selectedItem, int addedAmount, ItemQuality quality, int bagID)
    {
        if (_siloID == bagID)
        {
            siloInventory.AddToItem(selectedItem, quality, addedAmount);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(_siloID);
        }
    }

    private void RemoveFromStack(SCR_Items selectedItem, int removedAmount, ItemQuality quality, int bagID)
    {
        if (_siloID == bagID)
        {
            siloInventory.RemoveFromItem(selectedItem, quality, removedAmount);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(_siloID);
        }
    }

    private void RemoveStackWithSlot(InventorySlot invSlot, int bagID)
    {
        if (_siloID == bagID)
        {
            siloInventory.RemoveItem(invSlot);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(_siloID);
        }
    }

    private void SwapInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if (id == _siloID)
        {
            siloInventory.SwapItemSlots(slotOne, slotTwo);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void MoveInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if (id == _siloID)
        {
            siloInventory.MoveSlots(slotOne, slotTwo);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void MergeAllInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if (id == _siloID)
        {
            siloInventory.RemoveItem(slotOne);
            siloInventory.AddToItem(slotTwo, slotTwo.Quality, slotOne.Amount);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(id);
        }
    }

    private void MergeInventorySlots(InventorySlot slotOne, InventorySlot slotTwo, int id)
    {
        if (id == _siloID)
        {
            siloInventory.RemoveItem(slotOne);
            siloInventory.MergeSlots(slotOne, slotTwo);
            CustomEvents.InventorySystem.SiloInventory.OnUpdateUI?.Invoke(id);
        }
    }



    private void OnEnable()
    {
        CustomEvents.InventorySystem.SiloInventory.OnAddNewItemStack += AddNewItem;
        CustomEvents.InventorySystem.SiloInventory.OnRemoveItemStack += RemoveItemStack;
        CustomEvents.InventorySystem.SiloInventory.OnAddToItemStack += AddToStack;
        CustomEvents.InventorySystem.SiloInventory.OnRemoveFromItemStack += RemoveFromStack;
        CustomEvents.InventorySystem.SiloInventory.OnRemoveItemStackWithSlot += RemoveStackWithSlot;
        CustomEvents.InventorySystem.SiloInventory.OnSplit += AddNewSplitItem;
        CustomEvents.InventorySystem.SiloInventory.OnOpenSilo += OnOpen;

        CustomEvents.InventorySystem.SiloInventory.OnSwapInventorySlots += SwapInventorySlots;
        CustomEvents.InventorySystem.SiloInventory.OnMoveSlots += MoveInventorySlots;
        CustomEvents.InventorySystem.SiloInventory.OnMergeSlotsToAllOfType += MergeAllInventorySlots;
        CustomEvents.InventorySystem.SiloInventory.OnMergeSlots += MergeInventorySlots;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.SiloInventory.OnAddNewItemStack -= AddNewItem;
        CustomEvents.InventorySystem.SiloInventory.OnRemoveItemStack -= RemoveItemStack;
        CustomEvents.InventorySystem.SiloInventory.OnAddToItemStack -= AddToStack;
        CustomEvents.InventorySystem.SiloInventory.OnRemoveFromItemStack -= RemoveFromStack;
        CustomEvents.InventorySystem.SiloInventory.OnRemoveItemStackWithSlot -= RemoveStackWithSlot;
        CustomEvents.InventorySystem.SiloInventory.OnSplit -= AddNewSplitItem;
        CustomEvents.InventorySystem.SiloInventory.OnOpenSilo -= OnOpen;

        CustomEvents.InventorySystem.SiloInventory.OnSwapInventorySlots -= SwapInventorySlots;
        CustomEvents.InventorySystem.SiloInventory.OnMoveSlots -= MoveInventorySlots;
        CustomEvents.InventorySystem.SiloInventory.OnMergeSlotsToAllOfType -= MergeAllInventorySlots;
        CustomEvents.InventorySystem.SiloInventory.OnMergeSlots -= MergeInventorySlots;
    }

    public SerializableList SaveData()
    {
        Debug.Log("Saving silo");

        SerializableList data = new SerializableList();

        foreach (InventorySlot slot in siloInventory.GetInventory())
        {
            data.Add($"{slot.Name}-{slot.Item.name}-{slot.Amount.ToString()}-{slot.Quality.ToString()}");
        }

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        Debug.Log("Loading silo");

        siloInventory.ClearInventory();

        if (_data.Count <= 0) return;

        foreach (string item in _data)
        {
            if (string.IsNullOrEmpty(item)) return;

            string[] values = item.Split('-');

            //SCR_Items SO = Resources.Load(values[1], typeof(SCR_Items)) as SCR_Items;
            SCR_Items SO = Resources.Load<SCR_Items>($"Items/Fertilizer/{values[1].ToString()}");
            if (SO == null) SO = Resources.Load<SCR_Items>($"Items/Fish/{values[1].ToString()}");
            if (SO == null) SO = Resources.Load<SCR_Items>($"Items/Produce/{values[1].ToString()}");
            if (SO == null) SO = Resources.Load<SCR_Items>($"Items/SeedPackets/{values[1].ToString()}");
            if (SO == null) SO = Resources.Load<SCR_Items>($"Items/Seeds/{values[1].ToString()}");
            if (SO == null)
            {
                Debug.LogError($"Null SO object for {values[1]}");
                continue;
            }

            AddNewItem(SO, int.Parse(values[2]), (ItemQuality)Enum.Parse(typeof(ItemQuality), values[3]));
        }
    }
}
