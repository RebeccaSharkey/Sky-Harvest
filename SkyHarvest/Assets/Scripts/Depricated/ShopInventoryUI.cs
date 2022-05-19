using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject uiInventorySlot;
    [SerializeField] private Transform container;
    private ShopInventory shopInventory;


    void SetInventory(ShopInventory newInventory)
    {
        shopInventory = newInventory;
        Debug.Log(shopInventory.GetInventory().Count);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }

        int columns = 7;
        int xSpace = 140;
        int ySpace = 140;
        int currentCollumnSlot = 0;
        int currentRowSlot = 0;

        if (shopInventory != null)
        {
            foreach (InventorySlot slot in shopInventory.GetInventory())
            {
                GameObject currentSlot = Instantiate(uiInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<SCR_ItemSlotUI>().SetInventorySlot(slot);

                currentSlot.GetComponent<RectTransform>().SetParent(container.gameObject.GetComponent<RectTransform>());
                currentSlot.GetComponent<RectTransform>().localPosition = new Vector3(-465f + (currentRowSlot * xSpace), 135f - (currentCollumnSlot * ySpace), 0f);

                currentRowSlot++;

                if (currentRowSlot > columns)
                {
                    currentRowSlot = 0;
                    currentCollumnSlot++;
                }
            }
        }
    }

    private void ToggleUI(bool state)
    {
        inventoryPanel.SetActive(state);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleUI(!inventoryPanel.activeSelf);
        }
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    private void OnEnable()
    {
        CustomEvents.ShopSystem.OnGetShopInventory += SetInventory;
        CustomEvents.ShopSystem.OnUpdateUI += UpdateInventoryUI;
        CustomEvents.ShopSystem.ToggleUI += ToggleUI;
    }

    private void OnDisable()
    {
        CustomEvents.ShopSystem.OnGetShopInventory -= SetInventory;
        CustomEvents.ShopSystem.OnUpdateUI -= UpdateInventoryUI;
        CustomEvents.ShopSystem.ToggleUI -= ToggleUI;
    }
}
