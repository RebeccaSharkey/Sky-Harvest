using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SCR_BagUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;
    [SerializeField] private GameObject uiInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform container;
    private Inventory bagInventory;
    private int _bagID = -1;
    private bool isOpen = false;
    public bool IsOpen { get => isOpen; }

    [Header("Inspection Data")]
    [SerializeField] private GameObject contentBox;
    [SerializeField] private GameObject inspectionBox;
    private bool isInspectionOpen = false;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI qaulity;

    private SCR_CropItems currentInspectedCrop;
    [SerializeField] private GameObject cropPediaButton;

    void SetInventory(Inventory newInventory, int bagID)
    {
        bagInventory = newInventory;
        _bagID = bagID;
        UpdateInventoryUI(_bagID);
    }

    void UpdateInventoryUI(int bagID)
    {
        if (bagID == _bagID)
        {
            foreach (Transform item in container)
            {
                Destroy(item.gameObject);
            }
            /*

                        int columns = 7;
                        int xSpace = 140;
                        int ySpace = 140;
                        int currentCollumnSlot = 0;
                        int currentRowSlot = 0;*/

            if (bagInventory != null)
            {
                foreach (InventorySlot slot in bagInventory.GetInventory())
                {
                    GameObject currentSlot = Instantiate(uiInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                    currentSlot.GetComponent<SCR_BagSlotUI>().SetInventorySlot(slot);
                    currentSlot.GetComponent<SCR_BagSlotUI>().BagID = _bagID;

                    currentSlot.GetComponent<RectTransform>().SetParent(container.gameObject.GetComponent<RectTransform>());
                    /*
                    currentSlot.GetComponent<RectTransform>().localPosition = new Vector3(-495f + (currentRowSlot * xSpace), 265f - (currentCollumnSlot * ySpace), 0f);

                    currentRowSlot++;

                    if (currentRowSlot > columns)
                    {
                        currentRowSlot = 0;
                        currentCollumnSlot++;
                    }*/
                }
            }
        }

    }

    private void ToggleUI(bool state, int bagID)
    {
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
        if (bagID == _bagID)
        {
            if(!state)
            {
                if(isInspectionOpen)
                {
                    OnCloseInspection();
                }
                foreach (GameObject objecty in objectsToHide)
                {
                    objecty.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject objecty in objectsToHide)
                {
                    objecty.SetActive(false);
                }
            }
            inventoryPanel.SetActive(state);
            isOpen = state;
        }
    }

    public void OnOpenInspection(InventorySlot inventorySlot, bool openInspection, int bagID)
    {
        if (bagID == _bagID)
        {
            if (!isInspectionOpen && openInspection)
            {
                contentBox.GetComponent<RectTransform>().offsetMax -= new Vector2(335, 0);
                GetComponentInChildren<GridLayoutGroup>().constraintCount = 5;
                inspectionBox.SetActive(true);
                isInspectionOpen = true;
            }
            icon.GetComponent<Image>().sprite = inventorySlot.Item.ItemSprite;
            itemName.text = "Name: " + inventorySlot.Item.name;
            amount.text = "Amount in slot: " + inventorySlot.Amount.ToString();
            qaulity.text = "Quality: " + inventorySlot.Quality.ToString();
        }
    }

    public void OnCloseInspection()
    {
        contentBox.GetComponent<RectTransform>().offsetMax -= new Vector2(-335, 0);
        GetComponentInChildren<GridLayoutGroup>().constraintCount = 7;
        inspectionBox.SetActive(false);
        isInspectionOpen = false;
    }

    public void ExitClick()
    {
        foreach (Transform item in container)
        {
            item.GetComponent<SCR_BagSlotUI>().OnCloseSplit();
        }
        ToggleUI(false, _bagID);
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
    }

    private void ShowCroppediaButton(SCR_CropItems thisCrop)
    {
        currentInspectedCrop = thisCrop;
        cropPediaButton.SetActive(true);
    }

    private void DisableCroppediaButton()
    {
        cropPediaButton.SetActive(false);
    }

    public void OpenCroppedia()
    {
        ToggleUI(false, _bagID);
        CustomEvents.FarmersHandbook.OnOpenCroppedia?.Invoke(currentInspectedCrop);
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.BagInventory.OnGetInventoryData += SetInventory;
        CustomEvents.InventorySystem.BagInventory.OnUpdateUI += UpdateInventoryUI;
        CustomEvents.InventorySystem.BagInventory.ToggleUI += ToggleUI;
        CustomEvents.InventorySystem.BagInventory.OnOpenInspection += OnOpenInspection;

        CustomEvents.InventorySystem.OnShowCropPediaButton += ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton += DisableCroppediaButton;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.BagInventory.OnGetInventoryData -= SetInventory;
        CustomEvents.InventorySystem.BagInventory.OnUpdateUI -= UpdateInventoryUI;
        CustomEvents.InventorySystem.BagInventory.ToggleUI -= ToggleUI;
        CustomEvents.InventorySystem.BagInventory.OnOpenInspection -= OnOpenInspection;

        CustomEvents.InventorySystem.OnShowCropPediaButton -= ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton -= DisableCroppediaButton;
    }
}
