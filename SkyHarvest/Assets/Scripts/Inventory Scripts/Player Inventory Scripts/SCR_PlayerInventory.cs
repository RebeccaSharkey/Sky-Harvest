using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerInventory : MonoBehaviour, iSaveable
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject bagPrefab;
    public List<GameObject> bags;
    private int bagIDcount = 1;
    private int siloID = -1;

    [SerializeField] private int inventorySize = 40;
    public int InventorySize { get => inventorySize; set => inventorySize = value; }

    private void Awake()
    {
        //playerInventory = new Inventory();
        bags = new List<GameObject>();
    }

    private void Start()
    {
        SetInventory();
    }

    private void ChangeSiloID(int newID)
    {
        siloID = newID;
    }

    private void AddNewItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal)
    {
        playerInventory.AddItem(newItem, newAmount, quality);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
        if (playerInventory.GetInventory().Count > inventorySize)
        {
            Debug.Log("Inventory Too Full");
            CustomEvents.InventorySystem.OnInventoryTooFull?.Invoke();
            if (siloID == -1)
            {
                CustomEvents.InventorySystem.PlayerInventory.DropItem?.Invoke(playerInventory.GetInventory()[playerInventory.GetInventory().Count - 1]);
            }
            else
            {
                CustomEvents.InventorySystem.PlayerInventory.OnMoveToSilo?.Invoke(playerInventory.GetInventory()[playerInventory.GetInventory().Count - 1], siloID);
            }
        }
    }

    private void AddNewItemOnLoad(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal)
    {
        playerInventory.AddItem(newItem, newAmount, quality);
        if (playerInventory.GetInventory().Count > inventorySize)
        {
            Debug.Log("Inventory Too Full");
            CustomEvents.InventorySystem.OnInventoryTooFull?.Invoke();
            if (siloID == -1)
            {
                CustomEvents.InventorySystem.PlayerInventory.DropItem?.Invoke(playerInventory.GetInventory()[playerInventory.GetInventory().Count - 1]);
            }
            else
            {
                CustomEvents.InventorySystem.PlayerInventory.OnMoveToSilo?.Invoke(playerInventory.GetInventory()[playerInventory.GetInventory().Count - 1], siloID);
            }
        }
    }

    private void AddNewSplitItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal)
    {
        playerInventory.AddItem(newItem, newAmount, quality, true);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
        if (playerInventory.GetInventory().Count > inventorySize)
        {
            Debug.Log("Inventory Too Full");
            CustomEvents.InventorySystem.OnInventoryTooFull?.Invoke();
            if (siloID == -1)
            {
                CustomEvents.InventorySystem.PlayerInventory.DropItem?.Invoke(playerInventory.GetInventory()[playerInventory.GetInventory().Count - 1]);
            }
            else
            {
                CustomEvents.InventorySystem.PlayerInventory.OnMoveToSilo?.Invoke(playerInventory.GetInventory()[playerInventory.GetInventory().Count - 1], siloID);
            }
        }
    }

    private void RemoveItemStack(SCR_Items selectedItem, ItemQuality quality)
    {
        playerInventory.RemoveItem(selectedItem, quality);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void AddToStack(SCR_Items selectedItem, int addedAmount, ItemQuality quality)
    {
        playerInventory.AddToItem(selectedItem, quality, addedAmount);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void RemoveFromStack(SCR_Items selectedItem, int removedAmount, ItemQuality quality)
    {
        playerInventory.RemoveFromItem(selectedItem, quality, removedAmount);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void RemoveWithNoQuality(SCR_Items selectedItem, int removedAmount)
    {
        TwoKeyDictionary<SCR_Items, ItemQuality, int> tempList = new TwoKeyDictionary<SCR_Items, ItemQuality, int>();
        foreach(InventorySlot item in playerInventory.GetInventory())
        {
            if (item.Item == selectedItem)
            { 
                if(tempList.ContainsKey(item.Item, item.Quality))
                {
                    tempList[item.Item, item.Quality] += item.Amount;
                }
                else
                {
                    tempList.Add(item.Item, item.Quality, item.Amount);
                }
            }
        }

        int amount = removedAmount;
        while (amount != 0)
        {
            if(tempList.ContainsKey(selectedItem, ItemQuality.Bad))
            {
                amount -= tempList[selectedItem, ItemQuality.Bad];

                if(amount < 0)
                {
                    tempList[selectedItem, ItemQuality.Bad] += amount;
                    amount = 0;
                }

                RemoveFromStack(selectedItem, tempList[selectedItem, ItemQuality.Bad], ItemQuality.Bad);

                if(amount > 0)
                {
                    tempList.Remove(selectedItem, ItemQuality.Bad);
                }
            }
            else if (tempList.ContainsKey(selectedItem, ItemQuality.Normal))
            {
                amount -= tempList[selectedItem, ItemQuality.Normal];

                if (amount < 0)
                {
                    tempList[selectedItem, ItemQuality.Normal] += amount;
                    amount = 0;
                }

                RemoveFromStack(selectedItem, tempList[selectedItem, ItemQuality.Normal], ItemQuality.Normal);

                if (amount > 0)
                {
                    tempList.Remove(selectedItem, ItemQuality.Normal);
                }
            }
            else if (tempList.ContainsKey(selectedItem, ItemQuality.Good))
            {
                amount -= tempList[selectedItem, ItemQuality.Good];

                if (amount < 0)
                {
                    tempList[selectedItem, ItemQuality.Good] += amount;
                    amount = 0;
                }

                RemoveFromStack(selectedItem, tempList[selectedItem, ItemQuality.Good], ItemQuality.Good);

                if (amount > 0)
                {
                    tempList.Remove(selectedItem, ItemQuality.Good);
                }
            }
            else if (tempList.ContainsKey(selectedItem, ItemQuality.Perfect))
            {
                amount -= tempList[selectedItem, ItemQuality.Perfect];

                if (amount < 0)
                {
                    tempList[selectedItem, ItemQuality.Perfect] += amount;
                    amount = 0;
                }

                RemoveFromStack(selectedItem, tempList[selectedItem, ItemQuality.Perfect], ItemQuality.Perfect);

                if (amount > 0)
                {
                    tempList.Remove(selectedItem, ItemQuality.Perfect);
                }
            }
        }
    }

    private void RemoveStackWithSlot(InventorySlot invSlot)
    {
        playerInventory.RemoveItem(invSlot);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void CreateBag()
    {
        bags.Add(Instantiate(bagPrefab, transform.position, Quaternion.identity));
        bags[bags.Count - 1].transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
        bags[bags.Count - 1].transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        bags[bags.Count - 1].GetComponent<SCR_Bags>().BagID = bagIDcount;
        bagIDcount++;
    }

    private int GetClosestBagID()
    {
        if (bags.Count == 0)
        {
            return -1;
        }
        else
        {
            GameObject closestBag = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject potentialTarget in bags)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestBag = potentialTarget;
                }
            }

            if (Vector3.Distance(gameObject.transform.position, closestBag.transform.position) <= 5f)
            {
                return closestBag.GetComponent<SCR_Bags>().BagID;
            }
            else
            {
                return -1;
            }
        }
    }

    private void GetAllItemsOfType(ItemTypes itemType)
    {
        Inventory typeInventory = new Inventory();

        foreach(InventorySlot items in playerInventory.GetInventory())
        {
            if(items.Item.ItemType == itemType)
            {
                typeInventory.AddItem(items.Item, items.Amount, items.Quality);
            }
        }
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData?.Invoke(typeInventory);
    }

    private void SetInventory()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData?.Invoke(playerInventory);
    }

    private void DeleteBagFromList(GameObject currentBag)
    {
        bags.Remove(currentBag);        
    }
    
    private void Organise()
    {
        Inventory tempInventory = new Inventory();
        foreach (InventorySlot item in playerInventory.GetInventory())
        {
            tempInventory.AddItem(item.Item, item.Amount, item.Quality);
        }
        playerInventory = new Inventory(tempInventory.GetInventory());
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData.Invoke(playerInventory);
    }

    private Inventory GetInventory()
    {
        return playerInventory;
    }

    private void SwapInventorySlots(InventorySlot slotOne, InventorySlot slotTwo)
    {
        playerInventory.SwapItemSlots(slotOne, slotTwo);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void MoveInventorySlots(InventorySlot slotOne, InventorySlot slotTwo)
    {
        playerInventory.MoveSlots(slotOne, slotTwo);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void MergeAllInventorySlots(InventorySlot slotOne, InventorySlot slotTwo)
    {
        playerInventory.RemoveItem(slotOne);
        playerInventory.AddToItem(slotTwo, slotTwo.Quality, slotOne.Amount);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void MergeInventorySlots(InventorySlot slotOne, InventorySlot slotTwo)
    {
        playerInventory.RemoveItem(slotOne);
        playerInventory.MergeSlots(slotOne, slotTwo);
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
    }

    private void IncreaseInventorySize(int incriment)
    {
        inventorySize += incriment;
    }

    private int ReturnInventorySize()
    {
        return inventorySize;
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack += AddNewItem;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStack += RemoveItemStack;
        CustomEvents.InventorySystem.PlayerInventory.OnAddToItemStack += AddToStack;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack += RemoveFromStack;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStackNoQuality += RemoveWithNoQuality;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot += RemoveStackWithSlot;
        CustomEvents.InventorySystem.PlayerInventory.CreatBag += CreateBag;
        CustomEvents.InventorySystem.PlayerInventory.FindClosestBagID += GetClosestBagID;
        CustomEvents.InventorySystem.PlayerInventory.OnSiloOpened += ChangeSiloID;
        CustomEvents.InventorySystem.PlayerInventory.onGetTypeOnlyInventory += GetAllItemsOfType;
        CustomEvents.InventorySystem.PlayerInventory.OnSetInventory += SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnDeleteBag += DeleteBagFromList;
        CustomEvents.InventorySystem.PlayerInventory.OnSplit += AddNewSplitItem;
        CustomEvents.InventorySystem.PlayerInventory.OnOrganise += Organise;
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventory += GetInventory;

        CustomEvents.InventorySystem.PlayerInventory.OnSwapInventorySlots += SwapInventorySlots;
        CustomEvents.InventorySystem.PlayerInventory.OnMoveSlots += MoveInventorySlots;
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlotsToAllOfType += MergeAllInventorySlots;
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlots += MergeInventorySlots;

        CustomEvents.InventorySystem.PlayerInventory.OnIncreaseInventorySize += IncreaseInventorySize;
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize += ReturnInventorySize;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack -= AddNewItem;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStack -= RemoveItemStack;
        CustomEvents.InventorySystem.PlayerInventory.OnAddToItemStack -= AddToStack;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack -= RemoveFromStack;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStackNoQuality -= RemoveWithNoQuality;
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot -= RemoveStackWithSlot;
        CustomEvents.InventorySystem.PlayerInventory.CreatBag -= CreateBag;
        CustomEvents.InventorySystem.PlayerInventory.FindClosestBagID -= GetClosestBagID;
        CustomEvents.InventorySystem.PlayerInventory.OnSiloOpened -= ChangeSiloID;
        CustomEvents.InventorySystem.PlayerInventory.onGetTypeOnlyInventory -= GetAllItemsOfType;
        CustomEvents.InventorySystem.PlayerInventory.OnSetInventory -= SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnDeleteBag -= DeleteBagFromList;
        CustomEvents.InventorySystem.PlayerInventory.OnSplit -= AddNewSplitItem;
        CustomEvents.InventorySystem.PlayerInventory.OnOrganise -= Organise;
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventory -= GetInventory;

        CustomEvents.InventorySystem.PlayerInventory.OnSwapInventorySlots -= SwapInventorySlots;
        CustomEvents.InventorySystem.PlayerInventory.OnMoveSlots -= MoveInventorySlots;
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlots -= MergeInventorySlots;
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlots -= MergeInventorySlots;

        CustomEvents.InventorySystem.PlayerInventory.OnIncreaseInventorySize -= IncreaseInventorySize;
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize -= ReturnInventorySize;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        foreach(InventorySlot slot in playerInventory.GetInventory())
        {
            data.Add($"{slot.Name}-{slot.Item.name}-{slot.Amount.ToString()}-{slot.Quality.ToString()}");
        }

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        playerInventory.ClearInventory();

        if (_data.Count <= 0) return;

        foreach(string item in _data)
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

            AddNewItemOnLoad(SO, int.Parse(values[2]), (ItemQuality)Enum.Parse(typeof(ItemQuality), values[3]));
        }
    }
}
