using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_SilosInventoryUI : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;

    [SerializeField] private GameObject siloInventoryPanel;
    [SerializeField] private GameObject uiInventorySlot;
    [SerializeField] private GameObject uiSiloInventorySlot;
    [SerializeField] private Transform siloContainer;
    [SerializeField] private Transform playerContainer;
    private Inventory siloInventory;
    private Inventory playerInventory;
    private int _siloID;
    public int SiloID { get => _siloID; set => _siloID = value; }
    [SerializeField] private TextMeshProUGUI sizePlayer;
    //[SerializeField] private TextMeshProUGUI sizeSilo;

    [Header("Inspection Data Silo")]
    [SerializeField] private GameObject contentBox;
    [SerializeField] private GameObject inspectionBox;
    private bool isInspectionOpen = false;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI qaulity;

    [Header("Inspection Data Player")]
    [SerializeField] private GameObject contentBoxPlayer;
    [SerializeField] private GameObject inspectionBoxPlayer;
    private bool isInspectionOpenPlayer = false;
    [SerializeField] private GameObject iconPlayer;
    [SerializeField] private TextMeshProUGUI itemNamePlayer;
    [SerializeField] private TextMeshProUGUI amountPlayer;
    [SerializeField] private TextMeshProUGUI qaulityPlayer;

    private SCR_CropItems currentInspectedCrop;
    private SCR_CropItems currentInspectedCrop2;
    [SerializeField] private GameObject cropPediaButton;
    [SerializeField] private GameObject cropPediaButton2;

    private void ToggleUI(bool state, int siloID)
    {
        CustomEvents.InventorySystem.SiloInventory.OnMovingChanged?.Invoke(false);
        if (!state)
        {
            if (isInspectionOpen)
            {
                OnCloseInspectionSilo();
            }
            if (isInspectionOpenPlayer)
            {
                OnCloseInspectionPlayer();
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

        if (siloID == _siloID)
        {
            siloInventoryPanel.SetActive(state);
        }
    }

    public void OnExitClick()
    {
        foreach (Transform item in playerContainer)
        {
            item.GetComponent<SCR_PlayerSIloSlotUI>().OnCloseSplit();
        }
        foreach (Transform item in siloContainer)
        {
            item.GetComponent<SCR_SiloSlotUI>().OnCloseSplit();
        }

        ToggleUI(false, _siloID);
        CustomEvents.InventorySystem.PlayerInventory.OnSiloOpened?.Invoke(-1);
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
    }

    //For Player UI
    private void SetInventory(Inventory newInventory)
    {
        playerInventory = newInventory;
        UpdatePlayerInventory();
    }
    
    private void UpdatePlayerInventory()
    {
        CustomEvents.InventorySystem.SiloInventory.OnMovingChanged?.Invoke(false);
        foreach (Transform item in playerContainer)
        {
            Destroy(item.gameObject);
        }

        /*int columns = 4;
        int xSpace = 110;
        int ySpace = 110;
        int currentCollumnSlot = 0;
        int currentRowSlot = 0;*/

        if (playerInventory != null)
        {
            foreach (InventorySlot slot in playerInventory.GetInventory())
            {
                GameObject currentSlot = Instantiate(uiInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<SCR_PlayerSIloSlotUI>().SetInventorySlot(slot);

                currentSlot.GetComponent<RectTransform>().SetParent(playerContainer.gameObject.GetComponent<RectTransform>());
                currentSlot.GetComponent<RectTransform>().localScale = new Vector3(2.5f, 2.5f, 2.5f);/*
                currentSlot.GetComponent<RectTransform>().localPosition = new Vector3(-245f + (currentRowSlot * xSpace), 320f - (currentCollumnSlot * ySpace), 0f);

                currentRowSlot++;

                if (currentRowSlot > columns)
                {
                    currentRowSlot = 0;
                    currentCollumnSlot++;
                }*/
            }
        }
        int total = CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize.Invoke();
        sizePlayer.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }

    //For Silo UI
    private void SetSiloInventory(Inventory newInventory, int siloID)
    {
        siloInventory = newInventory;
        _siloID = siloID;
        CustomEvents.InventorySystem.PlayerInventory.OnSiloOpened?.Invoke(_siloID);
        UpdateSilos(_siloID);
        UpdatePlayerInventory();
    }

    private void UpdateSilos(int siloID)
    {
        CustomEvents.InventorySystem.SiloInventory.OnMovingChanged?.Invoke(false);
        if (siloID == _siloID)
        {
            foreach (Transform item in siloContainer)
            {
                Destroy(item.gameObject);
            }

            /*int columns = 4;
            int xSpace = 110;
            int ySpace = 110;
            int currentCollumnSlot = 0;
            int currentRowSlot = 0;*/

            if (siloInventory != null)
            {
                foreach (InventorySlot slot in siloInventory.GetInventory())
                {
                    GameObject currentSlot = Instantiate(uiSiloInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                    currentSlot.GetComponent<SCR_SiloSlotUI>().SetInventorySlot(slot);

                    currentSlot.GetComponent<RectTransform>().SetParent(siloContainer.gameObject.GetComponent<RectTransform>());
                    currentSlot.GetComponent<RectTransform>().localScale = new Vector3(2.5f, 2.5f, 2.5f);/*
                    currentSlot.GetComponent<RectTransform>().localPosition = new Vector3(-245f + (currentRowSlot * xSpace), 320f - (currentCollumnSlot * ySpace), 0f);

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

    public void OnOpenInspectionSilo(InventorySlot inventorySlot, bool openInspection, int siloID)
    {
        if (siloID == _siloID)
        {
            if (!isInspectionOpen && openInspection)
            {
                contentBox.GetComponent<RectTransform>().offsetMin += new Vector2(0, 275);
                inspectionBox.SetActive(true);
                isInspectionOpen = true;
            }
            icon.GetComponent<Image>().sprite = inventorySlot.Item.ItemSprite;
            itemName.text = "Name: " + inventorySlot.Item.name;
            amount.text = "Amount in slot: " + inventorySlot.Amount.ToString();
            qaulity.text = "Quality: " + inventorySlot.Quality.ToString();
        }
    }

    public void OnOpenInspectionPlayer(InventorySlot inventorySlot, bool openInspection, int siloID)
    {
        if (siloID == _siloID)
        {
            if (!isInspectionOpenPlayer && openInspection)
            {
                contentBoxPlayer.GetComponent<RectTransform>().offsetMin += new Vector2(0, 275);
                inspectionBoxPlayer.SetActive(true);
                isInspectionOpenPlayer = true;
            }
            iconPlayer.GetComponent<Image>().sprite = inventorySlot.Item.ItemSprite;
            itemNamePlayer.text = "Name: " + inventorySlot.Item.name;
            amountPlayer.text = "Amount in slot: " + inventorySlot.Amount.ToString();
            qaulityPlayer.text = "Quality: " + inventorySlot.Quality.ToString();
        }
    }

    public void OnCloseInspectionSilo()
    {
        contentBox.GetComponent<RectTransform>().offsetMin += new Vector2(0, -275);
        inspectionBox.SetActive(false);
        isInspectionOpen = false;
    }

    public void OnCloseInspectionPlayer()
    {
        contentBoxPlayer.GetComponent<RectTransform>().offsetMin += new Vector2(0, -275);
        inspectionBoxPlayer.SetActive(false);
        isInspectionOpenPlayer = false;
    }

    public void OnSwapTwoItems(InventorySlot itemToPlayer, InventorySlot itemToSilo)
    {
        CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot?.Invoke(itemToSilo);
        CustomEvents.InventorySystem.SiloInventory.OnRemoveItemStackWithSlot?.Invoke(itemToPlayer, _siloID);

        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(itemToPlayer.Item, itemToPlayer.Amount, itemToPlayer.Quality);
        CustomEvents.InventorySystem.SiloInventory.OnAddNewItemStack?.Invoke(itemToSilo.Item, itemToSilo.Amount, itemToSilo.Quality, _siloID);

        UpdatePlayerInventory();
        UpdateSilos(_siloID);
    }

    private void OnUpdateInventorySize()
    {
        int total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        sizePlayer.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }

    private void ShowCroppediaButton(SCR_CropItems thisCrop, int button)
    {
        if (button == 1)
        {
            currentInspectedCrop = thisCrop;
            cropPediaButton.SetActive(true);
        }
        else
        {
            currentInspectedCrop2 = thisCrop;
            cropPediaButton2.SetActive(true);
        }
    }

    private void DisableCroppediaButton()
    {
        cropPediaButton.SetActive(false);
    }

    private void DisableCropButton2()
    {
        cropPediaButton2.SetActive(false);
    }

    public void OpenCroppedia1()
    {
        ToggleUI(false, _siloID);
        CustomEvents.FarmersHandbook.OnOpenCroppedia?.Invoke(currentInspectedCrop);
    }
    public void OpenCroppedia2()
    {
        ToggleUI(false, _siloID);
        CustomEvents.FarmersHandbook.OnOpenCroppedia?.Invoke(currentInspectedCrop2);
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData += SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI += UpdatePlayerInventory;

        CustomEvents.InventorySystem.SiloInventory.OnGetInventoryData += SetSiloInventory;
        CustomEvents.InventorySystem.SiloInventory.OnUpdateUI += UpdateSilos;
        CustomEvents.InventorySystem.SiloInventory.ToggleUI += ToggleUI;

        CustomEvents.InventorySystem.SiloInventory.OnOpenInspectionPlayer += OnOpenInspectionPlayer;
        CustomEvents.InventorySystem.SiloInventory.OnOpenInspectionSilo += OnOpenInspectionSilo;

        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize += OnUpdateInventorySize;

        CustomEvents.InventorySystem.OnShowCropPediaButtons += ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton += DisableCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton2 += DisableCropButton2;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData -= SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI -= UpdatePlayerInventory;

        CustomEvents.InventorySystem.SiloInventory.OnGetInventoryData -= SetSiloInventory;
        CustomEvents.InventorySystem.SiloInventory.OnUpdateUI -= UpdateSilos;
        CustomEvents.InventorySystem.SiloInventory.ToggleUI -= ToggleUI;

        CustomEvents.InventorySystem.SiloInventory.OnOpenInspectionPlayer -= OnOpenInspectionPlayer;
        CustomEvents.InventorySystem.SiloInventory.OnOpenInspectionSilo -= OnOpenInspectionSilo;

        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize -= OnUpdateInventorySize;

        CustomEvents.InventorySystem.OnShowCropPediaButtons -= ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton -= DisableCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton2 -= DisableCropButton2;
    }
}
