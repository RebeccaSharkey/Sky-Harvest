using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ShopInventory : MonoBehaviour
{
    private List<SCR_Items> _items = new List<SCR_Items>();
    [SerializeField] private Inventory _shopInventroy;
    [HideInInspector] public int _shopID;

    private void Awake()
    {
        GameObject[] shopsInGame;
        shopsInGame = GameObject.FindGameObjectsWithTag("Shop");
        if (shopsInGame.Length == 0)
        {
            _shopID = 1;
        }
        else
        {
            //Loops through the array of silos and sets the Task boards ID to the gameObjects place in the array
            for (int i = 0; i < shopsInGame.Length; i++)
            {
                if (gameObject == shopsInGame[i].gameObject)
                {
                    _shopID = i + 1;
                }
            }
        }
    }

    public void Start()
    {
        ResetInventroy();
    }

    public void OpenShop(int shopID)
    {
        if(_shopID == shopID)
        {
            CustomEvents.ShopSystem.OnSetUpShopUI?.Invoke(_shopInventroy, _shopID);
            CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
        }
    }

    public void ResetInventroy()
    {
        _shopInventroy.GetInventory().Clear();
        _items = (List<SCR_Items>)CustomEvents.AvailableItems.OnGetShopList?.Invoke();

        foreach (SCR_Items item in _items)
        {
            if(item.Stackable)
            {
                _shopInventroy.AddItem(item, item.StackAmount);
            }
            else
            {
                _shopInventroy.AddItem(item, 1);
            }
        }
    }

    private void AddNewItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.AddItem(newItem, newAmount, quality);
            CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
        }
    }

    private void RemoveItemStack(SCR_Items selectedItem, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.RemoveItem(selectedItem, quality);
            CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
        }
    }

    private void AddToStack(SCR_Items selectedItem, int addedAmount, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.AddToItem(selectedItem, quality, addedAmount);
            CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
        }
    }

    private void RemoveFromStack(SCR_Items selectedItem, int removedAmount, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.RemoveFromItem(selectedItem, quality, removedAmount);
            CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
        }
    }

    private void RemoveStackWithSlot(InventorySlot invSlot, int shopID)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.RemoveItem(invSlot);
            CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
        }
    }

    public void OnEnable()
    {
        CustomEvents.ShopSystem.OnAddNewItemStack += AddNewItem;
        CustomEvents.ShopSystem.OnRemoveItemStack += RemoveItemStack;
        CustomEvents.ShopSystem.OnAddToItemStack += AddToStack;
        CustomEvents.ShopSystem.OnRemoveFromItemStack += RemoveFromStack;
        CustomEvents.ShopSystem.OnRemoveItemStackWithSlot += RemoveStackWithSlot;
        CustomEvents.TimeCycle.OnNewDaySetup += ResetInventroy;
    }

    public void OnDisable()
    {
        CustomEvents.ShopSystem.OnAddNewItemStack -= AddNewItem;
        CustomEvents.ShopSystem.OnRemoveItemStack -= RemoveItemStack;
        CustomEvents.ShopSystem.OnAddToItemStack -= AddToStack;
        CustomEvents.ShopSystem.OnRemoveFromItemStack -= RemoveFromStack;
        CustomEvents.ShopSystem.OnRemoveItemStackWithSlot -= RemoveStackWithSlot;
        CustomEvents.TimeCycle.OnNewDaySetup -= ResetInventroy;
    }
}
