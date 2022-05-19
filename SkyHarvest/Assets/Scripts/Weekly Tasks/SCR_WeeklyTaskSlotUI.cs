using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TwoKeyDictionary<TKey1, TKey2, TValue> : Dictionary<Tuple<TKey1, TKey2>, TValue>, IDictionary<Tuple<TKey1, TKey2>, TValue>
{
    public TValue this[TKey1 key1, TKey2 key2]
    {
        get { return base[Tuple.Create(key1, key2)]; }
        set { base[Tuple.Create(key1, key2)] = value; }
    }

    public void Add(TKey1 key1, TKey2 key2, TValue value)
    {
        base.Add(Tuple.Create(key1, key2), value);
    }

    public void Remove(TKey1 key1, TKey2 key2)
    {
        base.Remove(Tuple.Create(key1, key2));
    }

    public bool ContainsKey(TKey1 key1, TKey2 key2)
    {
        return base.ContainsKey(Tuple.Create(key1, key2));
    }
}

public class SCR_WeeklyTaskSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] private GameObject completeButton;

    private PlayerTask playerTask;
    [SerializeField] private TwoKeyDictionary<SCR_Items, ItemQuality, int> allTaskItemsInInventory;
    [SerializeField] private Dictionary<SCR_Items, int> allTaskItemsInInventoryNoQuality;

    public int _taskBoardID = -1;

    public void Awake()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI += UpdateCropList;
    }

    public void SetPlayerTask(PlayerTask _playerTask, int taskboardID)
    {
        playerTask = _playerTask;
        info.text = playerTask.name;
        _taskBoardID = taskboardID;
        allTaskItemsInInventory = new TwoKeyDictionary<SCR_Items, ItemQuality, int>();
        allTaskItemsInInventoryNoQuality = new Dictionary<SCR_Items, int>();
        UpdateCropList();
    }

    private void UpdateCropList()
    {
        Inventory playerInv = CustomEvents.InventorySystem.PlayerInventory.OnGetInventory?.Invoke();
        allTaskItemsInInventory.Clear();
        if (playerInv != null)
        {
            foreach (InventorySlot items in playerInv.GetInventory())
            {    
                switch(items.Item.ItemType)
                {
                    case ItemTypes.crop:
                    case ItemTypes.fertilizer:
                    case ItemTypes.fish:
                        if(playerTask.RequireQuality)
                        {
                            if (allTaskItemsInInventory.ContainsKey(items.Item, items.Quality))
                            {
                                allTaskItemsInInventory[items.Item, items.Quality] += items.Amount;
                            }
                            else
                            {
                                allTaskItemsInInventory.Add(items.Item, items.Quality, items.Amount);
                            }
                        }
                        else
                        {

                            if (allTaskItemsInInventoryNoQuality.ContainsKey(items.Item))
                            {
                                allTaskItemsInInventoryNoQuality[items.Item] += items.Amount;
                            }
                            else
                            {
                                allTaskItemsInInventoryNoQuality.Add(items.Item, items.Amount);
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("No Player Inventory");
            completeButton.SetActive(false);
        }
        CheckCompleteAllow();
    }

    public void CheckCompleteAllow()
    {
        completeButton.SetActive(false);
        if(playerTask.RequireQuality)
        {
            if (allTaskItemsInInventory.ContainsKey(playerTask.Item, playerTask.Quality))
            {
                if (allTaskItemsInInventory[playerTask.Item, playerTask.Quality] >= playerTask.Amount)
                {
                    completeButton.SetActive(true);
                }
            }
        }
        else
        {
            if (allTaskItemsInInventoryNoQuality.ContainsKey(playerTask.Item))
            {
                if (allTaskItemsInInventoryNoQuality[playerTask.Item] >= playerTask.Amount)
                {
                    completeButton.SetActive(true);
                }
            }
        }
    }

    public void OnCompletePress()
    {
        if(playerTask.RequireQuality)
        {
            CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(playerTask.Item, playerTask.Amount, playerTask.Quality);
        }
        else
        {
            CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStackNoQuality?.Invoke(playerTask.Item, playerTask.Amount);
        }
        
        CustomEvents.WeeklyTasks.OnTaskCompleted?.Invoke();
        CustomEvents.WeeklyTasks.OnDeleteTask?.Invoke(playerTask, _taskBoardID);
    }

    private void OnDestroy()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI -= UpdateCropList;
    }
}
