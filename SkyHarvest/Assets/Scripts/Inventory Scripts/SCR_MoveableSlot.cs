using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_MoveableSlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    private bool moving = false;
    private InventorySlot inventorySlot;
    private string inventoryOrigin = null;
    private int _ID = -1;

    public void SetValues(InventorySlot slot, string origin, int id = -1)
    {
        inventorySlot = slot;
        _itemImage.sprite = inventorySlot.Item.ItemSprite;
        _amountText.text = inventorySlot.Amount.ToString();
        moving = true;
        inventoryOrigin = origin;
        _ID = id;
    }

    private void Update()
    {
        if(moving)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x += 50f;
            mousePos.y -= 50f;
            gameObject.transform.position = mousePos;
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(RemoveMoveable());
            }
        }
    }

    private InventorySlot GetSlot()
    {
        return inventorySlot;
    }

    private string GetSlotOrigin()
    {
        return inventoryOrigin;
    }

    private int GetSlotID()
    {
        return _ID;
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventorySlot += GetSlot;
        CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventoryOrigin += GetSlotOrigin;
        CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventoryID += GetSlotID;
    }
    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventorySlot -= GetSlot;
        CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventoryOrigin -= GetSlotOrigin;
        CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventoryID -= GetSlotID;
    }

    IEnumerator RemoveMoveable()
    {
        yield return new WaitForSecondsRealtime(0.10f);
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
        CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
        CustomEvents.InventorySystem.SiloInventory.OnMovingChanged?.Invoke(false);
    }
}
