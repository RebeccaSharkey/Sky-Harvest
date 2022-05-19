using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_SpecialShopInventoryUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;

    [Header("Shop Data")]
    [SerializeField] private GameObject shopInventoryPanel;
    [SerializeField] private GameObject uiShopInventorySlot;
    [SerializeField] private Transform shopContainer;
    private Inventory shopInventory;

    [Header("Player Data")]    
    [SerializeField] private GameObject uiInventorySlot;
    [SerializeField] private Transform playerContainer;
    private Inventory playerInventory;
    [SerializeField] private TextMeshProUGUI sizePlayer;
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Inspection Data Shop")]
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

    private int _shopID;
    public int ShopId { get => _shopID; set => _shopID = value; }

    [Header("Warning Data")]
    [SerializeField] private GameObject warning;

    private SCR_CropItems currentInspectedCrop;
    private SCR_CropItems currentInspectedCrop2;
    [SerializeField] private GameObject cropPediaButton;
    [SerializeField] private GameObject cropPediaButton2;

    private void Awake()
    {
        //_shopID = GetComponentInParent<SCR_SpecialShopInventory>()._shopID;
    }

    private void ToggleUI(bool state, int shopID)
    {
        if (!state)
        {
            if (isInspectionOpen)
            {
                OnCloseInspectionShop();
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

        if (shopID == _shopID)
        {
            shopInventoryPanel.SetActive(state);
        }
        CustomEvents.UI.OnMoneyChanged?.Invoke((int)CustomEvents.Currency.OnGetCurrency?.Invoke());
    }

    public void OnExitClick()
    {
        foreach (Transform item in playerContainer)
        {
            item.GetComponent<SCR_SpecialShopPlayerUISlot>().OnCloseSplit();
        }
        foreach (Transform item in shopContainer)
        {
            item.GetComponent<SCR_SpecialShopUISlot>().OnCloseSplit();
        }
        ToggleUI(false, _shopID);
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
                currentSlot.GetComponent<SCR_SpecialShopPlayerUISlot>().SetInventorySlot(slot);

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

        int total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        sizePlayer.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }

    //For Silo UI
    private void SetShopInventory(Inventory newInventory, int siloID)
    {
        shopInventory = newInventory;
        _shopID = siloID;
        UpdateShop(_shopID);
        UpdatePlayerInventory();
    }

    private void UpdateShop(int siloID)
    {
        if (siloID == _shopID)
        {
            foreach (Transform item in shopContainer)
            {
                Destroy(item.gameObject);
            }
/*
            int columns = 4;
            int xSpace = 110;
            int ySpace = 110;
            int currentCollumnSlot = 0;
            int currentRowSlot = 0;*/

            if (shopInventory != null)
            {
                foreach (InventorySlot slot in shopInventory.GetInventory())
                {
                    GameObject currentSlot = Instantiate(uiShopInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                    currentSlot.GetComponent<SCR_SpecialShopUISlot>().SetInventorySlot(slot);

                    currentSlot.GetComponent<RectTransform>().SetParent(shopContainer.gameObject.GetComponent<RectTransform>());
                    currentSlot.GetComponent<RectTransform>().localScale = new Vector3(2.5f, 2.5f, 2.5f);
                    /*currentSlot.GetComponent<RectTransform>().localPosition = new Vector3(-245f + (currentRowSlot * xSpace), 320f - (currentCollumnSlot * ySpace), 0f);

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

    private void OnWarning()
    {
        warning.SetActive(true);
    }

    public void OnCloseWarning()
    {
        warning.SetActive(false);
        CustomEvents.TimeCycle.OnUnpause.Invoke();
    }

    public void OnOpenInspectionShop(InventorySlot inventorySlot, bool openInspection, int shopID)
    {
        if (shopID == _shopID)
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

    public void OnOpenInspectionPlayer(InventorySlot inventorySlot, bool openInspection, int shopID)
    {
        if (shopID == _shopID)
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

    public void OnCloseInspectionShop()
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

    private void OnUpdateInventorySize()
    {
        int total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        sizePlayer.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }

    private void UpdateCurrencyUI(int _newAmount)
    {
        currencyText.text = $"Currency: {_newAmount}";
    }

    private void ShowCroppediaButton(SCR_CropItems thisCrop, int button)
    {
        if (button == 1)
        {
            currentInspectedCrop = thisCrop;
            cropPediaButton.SetActive(true);
            cropPediaButton2.SetActive(false);
        }
        else
        {
            currentInspectedCrop2 = thisCrop;
            cropPediaButton.SetActive(false);
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

    public void OpenCroppedia()
    {
        ToggleUI(false, _shopID);
        CustomEvents.FarmersHandbook.OnOpenCroppedia?.Invoke(currentInspectedCrop);
    }
    public void OpenCroppedia2()
    {
        ToggleUI(false, _shopID);
        CustomEvents.FarmersHandbook.OnOpenCroppedia?.Invoke(currentInspectedCrop2);
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData += SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI += UpdatePlayerInventory;

        CustomEvents.ShopSystem.SpecialShop.OnGetInventoryData += SetShopInventory;
        CustomEvents.ShopSystem.SpecialShop.OnUpdateUI += UpdateShop;
        CustomEvents.ShopSystem.SpecialShop.ToggleUI += ToggleUI;
        CustomEvents.ShopSystem.SpecialShop.OnWarning += OnWarning;

        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionPlayer += OnOpenInspectionPlayer;
        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionShop += OnOpenInspectionShop;

        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize += OnUpdateInventorySize;
        CustomEvents.UI.OnMoneyChanged += UpdateCurrencyUI;

        CustomEvents.InventorySystem.OnShowCropPediaButtons += ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton += DisableCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton2 += DisableCropButton2;

    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData -= SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI -= UpdatePlayerInventory;

        CustomEvents.ShopSystem.SpecialShop.OnGetInventoryData -= SetShopInventory;
        CustomEvents.ShopSystem.SpecialShop.OnUpdateUI -= UpdateShop;
        CustomEvents.ShopSystem.SpecialShop.ToggleUI -= ToggleUI;
        CustomEvents.ShopSystem.SpecialShop.OnWarning -= OnWarning;

        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionPlayer -= OnOpenInspectionPlayer;
        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionShop -= OnOpenInspectionShop;

        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize -= OnUpdateInventorySize;
        CustomEvents.UI.OnMoneyChanged -= UpdateCurrencyUI;

        CustomEvents.InventorySystem.OnShowCropPediaButtons -= ShowCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton -= DisableCroppediaButton;
        CustomEvents.InventorySystem.OnDisableCroppediaButton2 -= DisableCropButton2;
    }

}
