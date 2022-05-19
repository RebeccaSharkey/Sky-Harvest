using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryMono : MonoBehaviour
{
    [SerializeField] private ShopInventory shopInv;
    [SerializeField] private FrozenTears currency;
    [SerializeField] private SCR_Items randomItem;
    private void Start()
    {
        shopInv.AddItemsToShop();
        CustomEvents.ShopSystem.OnGetShopInventory?.Invoke(shopInv);
        CustomEvents.ShopSystem.OnUpdateUI?.Invoke();
        
    }

    private void ResetShop()
    {
        shopInv.ResetShop();
        shopInv.RandomiseShop();
        CustomEvents.ShopSystem.OnUpdateUI?.Invoke();
    }
    private void GetShopInventory(ShopInventory newInventory)
    {
        shopInv = newInventory;
        ResetShop();
    }

    private void BuyItem(SCR_Items newItem)
    {
        if(currency.amount >= newItem.BuyValueNormal)
        {
            currency.RemoveAmount(newItem.BuyValueNormal);
            CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(newItem, 1, ItemQuality.Normal);
        }
    }

    private void RemoveItem(SCR_Items newItem)
    {
        currency.AddAmount(newItem.SellValueNormal);
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(newItem, 1, ItemQuality.Normal);
    }

    private void OnEnable()
    {
        CustomEvents.ShopSystem.OnResetShop += ResetShop;
        CustomEvents.ShopSystem.OnGetShopInventory += GetShopInventory;
        CustomEvents.ShopSystem.OnBuyItem += BuyItem;
        CustomEvents.ShopSystem.OnSellItem += RemoveItem;
    }

    private void OnDisable()
    {
        CustomEvents.ShopSystem.OnResetShop -= ResetShop;
        CustomEvents.ShopSystem.OnGetShopInventory -= GetShopInventory;
        CustomEvents.ShopSystem.OnBuyItem -= BuyItem;
        CustomEvents.ShopSystem.OnSellItem -= RemoveItem;
    }
}
