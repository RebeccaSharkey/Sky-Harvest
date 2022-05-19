using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SpecialShopInventory : MonoBehaviour
{
    private List<SCR_Items> _items = new List<SCR_Items>();
    [SerializeField] private Inventory _shopInventroy;
    [HideInInspector] public int _shopID = -1;

    private void OnOpen(int id)
    {
        if(id == _shopID)
        {
            CustomEvents.ShopSystem.SpecialShop.OnGetInventoryData?.Invoke(_shopInventroy, _shopID);
            CustomEvents.ShopSystem.SpecialShop.ToggleUI?.Invoke(true, _shopID);
        }
    }

    public void ResetInventroy()
    {
        _shopInventroy.GetInventory().Clear();
        _items = (List<SCR_Items>)CustomEvents.AvailableItems.OnGetSpecialShopList?.Invoke();
        CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);

        //Random Inventory
        while (_shopInventroy.GetInventory().Count < 3)
        {
            foreach (SCR_Items item in _items)
            {
                float probability = Random.Range(0f, 1f);
                if (probability >= 0.25f)
                {
                    if (item.Stackable)
                    {
                        int amount = Random.Range(1, 10);
                        _shopInventroy.AddItem(item, amount);
                    }
                    else
                    {
                        if ((SCR_UniqueItems)item)
                        {
                            SCR_UniqueItems currentItem = (SCR_UniqueItems)item;
                            if (currentItem.Desciption == UniqueItems.recipe)
                            {
                                _shopInventroy.AddItem(item, 1);

                            }
                            else
                            {
                                int amount = Random.Range(1, 5);
                                _shopInventroy.AddItem(item, amount);
                            }
                        }
                        else
                        {
                            int amount = Random.Range(1, 5);
                            _shopInventroy.AddItem(item, amount);
                        }
                    }
                }
            }
        }
        CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);
    }

    private void AddNewItem(SCR_Items newItem, int newAmount = 1, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.AddItem(newItem, newAmount, quality);
            CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);
        }
    }

    private void RemoveItemStack(SCR_Items selectedItem, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.RemoveItem(selectedItem, quality);
            CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);
        }
    }

    private void AddToStack(SCR_Items selectedItem, int addedAmount, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.AddToItem(selectedItem, quality, addedAmount);
            CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);
        }
    }

    private void RemoveFromStack(SCR_Items selectedItem, int removedAmount, ItemQuality quality = ItemQuality.Normal, int shopID = -1)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.RemoveFromItem(selectedItem, quality, removedAmount);
            CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);
        }
    }

    private void RemoveStackWithSlot(InventorySlot invSlot, int shopID)
    {
        if (_shopID == shopID)
        {
            _shopInventroy.RemoveItem(invSlot);
            CustomEvents.ShopSystem.SpecialShop.OnUpdateUI?.Invoke(_shopID);
        }
    }

    public void OnEnable()
    {
        CustomEvents.ShopSystem.SpecialShop.OnAddNewItemStack += AddNewItem;
        CustomEvents.ShopSystem.SpecialShop.OnRemoveItemStack += RemoveItemStack;
        CustomEvents.ShopSystem.SpecialShop.OnAddToItemStack += AddToStack;
        CustomEvents.ShopSystem.SpecialShop.OnRemoveFromItemStack += RemoveFromStack;
        CustomEvents.ShopSystem.SpecialShop.OnRemoveItemStackWithSlot += RemoveStackWithSlot;
        CustomEvents.ShopSystem.SpecialShop.OnOpenSpecialShop += OnOpen;
    }

    public void OnDisable()
    {
        CustomEvents.ShopSystem.SpecialShop.OnAddNewItemStack -= AddNewItem;
        CustomEvents.ShopSystem.SpecialShop.OnRemoveItemStack -= RemoveItemStack;
        CustomEvents.ShopSystem.SpecialShop.OnAddToItemStack -= AddToStack;
        CustomEvents.ShopSystem.SpecialShop.OnRemoveFromItemStack -= RemoveFromStack;
        CustomEvents.ShopSystem.SpecialShop.OnRemoveItemStackWithSlot -= RemoveStackWithSlot;
        CustomEvents.ShopSystem.SpecialShop.OnOpenSpecialShop -= OnOpen;
    }
}
