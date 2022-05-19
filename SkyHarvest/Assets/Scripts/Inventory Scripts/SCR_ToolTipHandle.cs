using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ToolTipHandle : MonoBehaviour
{
    private void DestroyThis(bool bBool)
    {
        Destroy(gameObject);
    }

    private void DestroyThisObject(bool bBool, int thing)
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.ToggleUI += DestroyThis;
        CustomEvents.InventorySystem.BagInventory.ToggleUI += DestroyThisObject;
        CustomEvents.InventorySystem.SiloInventory.ToggleUI += DestroyThisObject;
        CustomEvents.ShopSystem.SpecialShop.ToggleUI += DestroyThisObject;
    }

    private void OnDestroy()
    {
        CustomEvents.InventorySystem.PlayerInventory.ToggleUI -= DestroyThis;
        CustomEvents.InventorySystem.BagInventory.ToggleUI -= DestroyThisObject;
        CustomEvents.InventorySystem.SiloInventory.ToggleUI -= DestroyThisObject;
        CustomEvents.ShopSystem.SpecialShop.ToggleUI -= DestroyThisObject;
    }
}
