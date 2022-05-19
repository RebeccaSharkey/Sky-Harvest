using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_EmptyMachineSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Merge and Move Options")]
    private bool moving = false;
    private InventorySlot movingInvSlot;

    public void OnClick()
    {
        if (moving)
        {
            movingInvSlot = (InventorySlot)CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventorySlot?.Invoke();
            int tempAmount = (int)CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventoryID?.Invoke();
            CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
            if ((bool)CustomEvents.CraftMachine.OnAddNewItemStack?.Invoke(movingInvSlot.Item, movingInvSlot.Amount, tempAmount, movingInvSlot.Quality))
            {
                CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot(movingInvSlot);
            }
        }
    }

    public void OnMovingStateChange(bool newState)
    {
        moving = newState;
        if (!moving)
        {
            gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (moving)
        {
            gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (moving)
        {
            gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    private void OnEnable()
    {
        CustomEvents.CraftMachine.OnMovingChanged += OnMovingStateChange;
    }

    private void OnDisable()
    {
        CustomEvents.CraftMachine.OnMovingChanged -= OnMovingStateChange;
    }
}
