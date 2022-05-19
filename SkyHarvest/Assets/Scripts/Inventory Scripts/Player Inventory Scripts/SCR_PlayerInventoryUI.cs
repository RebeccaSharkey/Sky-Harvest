using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_PlayerInventoryUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject uiInventorySlot;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI size;
    [SerializeField] private GameObject organiseButton;

    [Header("Inspection Data")]
    [SerializeField] private GameObject contentBox;
    [SerializeField] private GameObject inspectionBox;
    private bool isInspectionOpen = false;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI qaulity;

    private Inventory playerInventory;
    private bool inventoryAllowed = false;

    private SCR_CropItems currentInspectedCrop;
    [SerializeField] private GameObject cropPediaButton;

    void SetInventory(Inventory newInventory)
    {
        playerInventory = newInventory;
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }

        /*int columns = 7;
        int xSpace = 140;
        int ySpace = 140;
        int currentCollumnSlot = 0;
        int currentRowSlot = 0;*/

        if (playerInventory != null)
        {
            foreach (InventorySlot slot in playerInventory.GetInventory())
            {
                GameObject currentSlot = Instantiate(uiInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<SCR_ItemSlotUI>().SetInventorySlot(slot);

                currentSlot.GetComponent<RectTransform>().SetParent(container.gameObject.GetComponent<RectTransform>());
                /*currentSlot.GetComponent<RectTransform>().localPosition = new Vector3(-495f + (currentRowSlot * xSpace), 265f - (currentCollumnSlot * ySpace), 0f);

                currentRowSlot++;

                if (currentRowSlot > columns)
                {
                    currentRowSlot = 0;
                    currentCollumnSlot++;
                }*/
            }
        }

        int total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        size.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }

    private void ToggleUI(bool state)
    {
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
        inventoryPanel.SetActive(state);
        if(state)
        {
            CustomEvents.TimeCycle.OnPause?.Invoke(); 
            foreach (GameObject objecty in objectsToHide)
            {
                objecty.SetActive(false);
            }
        }
        else
        {
            CustomEvents.TimeCycle.OnUnpause?.Invoke();
            foreach (GameObject objecty in objectsToHide)
            {
                objecty.SetActive(true);
            }
        }
    }

    private bool GetPaused()
    {
        bool temp = (bool)CustomEvents.TimeCycle.OnGetPaused?.Invoke();
        return (bool)CustomEvents.TimeCycle.OnGetPaused?.Invoke();
    }

    private void SetAllowInventory(bool newBool)
    {
        inventoryAllowed = newBool;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !GetPaused() && inventoryAllowed)
        {
            ToggleUI(!inventoryPanel.activeSelf);
        }
    }

    private void ShowPlantUI(PlotBehaviour newPlot)
    {
        CustomEvents.InventorySystem.PlayerInventory.onGetTypeOnlyInventory?.Invoke(ItemTypes.seed);
        foreach (Transform item in container)
        {
            item.gameObject.GetComponent<SCR_ItemSlotUI>().IsPlanting = true;
            item.gameObject.GetComponent<SCR_ItemSlotUI>().Plot = newPlot;
        }
        inventoryPanel.SetActive(true);
        organiseButton.SetActive(false);
    }

    private void ShowFertiliserUI(PlotBehaviour newPlot)
    {
        CustomEvents.InventorySystem.PlayerInventory.onGetTypeOnlyInventory?.Invoke(ItemTypes.fertilizer);
        foreach (Transform item in container)
        {
            item.gameObject.GetComponent<SCR_ItemSlotUI>().IsFertilizing = true;
            item.gameObject.GetComponent<SCR_ItemSlotUI>().Plot = newPlot;
        }
        inventoryPanel.SetActive(true);
        organiseButton.SetActive(false);
    }

    private void ShowTreeUI(FruitTreePlotBehaviour newPlot)
    {
        CustomEvents.InventorySystem.PlayerInventory.onGetTypeOnlyInventory?.Invoke(ItemTypes.treeSeeds);
        foreach (Transform item in container)
        {
            item.gameObject.GetComponent<SCR_ItemSlotUI>().IsPlantingTree = true;
            item.gameObject.GetComponent<SCR_ItemSlotUI>().TreePlot = newPlot;
        }
        inventoryPanel.SetActive(true);
        organiseButton.SetActive(false);
    }

    public void ResetInventory()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (isInspectionOpen)
        {
            contentBox.GetComponent<RectTransform>().offsetMax -= new Vector2(-335, 0);
            GetComponentInChildren<GridLayoutGroup>().constraintCount = 7;
            inspectionBox.SetActive(false);
            isInspectionOpen = false;
        }
        ToggleUI(false);
        CustomEvents.InventorySystem.PlayerInventory.OnSetInventory?.Invoke();
        foreach (Transform item in container)
        {
            item.gameObject.GetComponent<SCR_ItemSlotUI>().IsPlanting = false;
            item.gameObject.GetComponent<SCR_ItemSlotUI>().IsFertilizing = false;
        }
        organiseButton.SetActive(true);
    }

    public void OnOrganise()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        CustomEvents.InventorySystem.PlayerInventory.OnOrganise?.Invoke();
    }

    public void OnOpenInspection(InventorySlot inventorySlot, bool openInspection)
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

    public void OnCloseInspection()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        contentBox.GetComponent<RectTransform>().offsetMax -= new Vector2(-335, 0);
        GetComponentInChildren<GridLayoutGroup>().constraintCount = 7;
        inspectionBox.SetActive(false);
        isInspectionOpen = false;
    }

    private void OnUpdateInventorySize()
    {
        int total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        size.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
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
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        ToggleUI(false);
        CustomEvents.FarmersHandbook.OnOpenCroppedia?.Invoke(currentInspectedCrop);
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData += SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI += UpdateInventoryUI;
        CustomEvents.InventorySystem.PlayerInventory.ToggleUI += ToggleUI;
        CustomEvents.InventorySystem.PlayerInventory.OnPlant += ShowPlantUI;
        CustomEvents.InventorySystem.PlayerInventory.OnFertilize += ShowFertiliserUI;
        CustomEvents.InventorySystem.PlayerInventory.OnPlantTree += ShowTreeUI;
        CustomEvents.InventorySystem.PlayerInventory.OnResetUIInventory += ResetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnSetAllowanceOfInventory += SetAllowInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection += OnOpenInspection;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize += OnUpdateInventorySize;
        CustomEvents.InventorySystem.OnShowCropPediaButton += ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton += DisableCroppediaButton;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData -= SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI -= UpdateInventoryUI;
        CustomEvents.InventorySystem.PlayerInventory.ToggleUI -= ToggleUI;
        CustomEvents.InventorySystem.PlayerInventory.OnPlant -= ShowPlantUI;
        CustomEvents.InventorySystem.PlayerInventory.OnFertilize -= ShowFertiliserUI;
        CustomEvents.InventorySystem.PlayerInventory.OnPlantTree -= ShowTreeUI;
        CustomEvents.InventorySystem.PlayerInventory.OnResetUIInventory -= ResetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnSetAllowanceOfInventory -= SetAllowInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection -= OnOpenInspection;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize -= OnUpdateInventorySize;
        CustomEvents.InventorySystem.OnShowCropPediaButton -= ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton -= DisableCroppediaButton;
    }
}