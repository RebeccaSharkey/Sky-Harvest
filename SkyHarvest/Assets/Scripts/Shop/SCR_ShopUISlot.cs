using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_ShopUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Inventory Slot Data")]
    [SerializeField] private InventorySlot _inventorySlot;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;

    [Header("Implementation Data")]
    [SerializeField] private GameObject selectionBox;
    private int _shopID;
    [SerializeField] private GameObject toolTip;
    private GameObject _toolTip = null;

    [Header("Split Data")]
    [SerializeField] private GameObject splitButton;
    [SerializeField] private GameObject splitMenu;
    [SerializeField] private Slider splitSlider;
    [SerializeField] private TMP_InputField stackOneTextInput;
    [SerializeField] private TMP_InputField stackTwoTextInput;
    [SerializeField] private TextMeshProUGUI totalPrice;
    [SerializeField] private GameObject warning;
    private bool isSplitting = false;
    private int stackOne = 1;
    private int stackTwo = 0;

    private bool isHovering = false;
    public void SetIsHovering(bool value) { isHovering = value; }

    private void Awake()
    {
        _inventorySlot = new InventorySlot();
        UpdateInventorySlot();
    }

    private void Start()
    {
        _shopID = GetComponentInParent<SCR_ShopInventoryUI>().ShopId;
    }

    private void UpdateInventorySlot()
    {
        if (_inventorySlot.Item != null)
        {
            _itemImage.enabled = true;
            _amountText.enabled = true;
            _itemImage.sprite = _inventorySlot.Item.ItemSprite;
            _amountText.text = _inventorySlot.Amount.ToString();
        }
        else
        {
            _itemImage.enabled = false;
            _amountText.enabled = false;
        }
    }

    public void SetInventorySlot(InventorySlot inventorySlot)
    {
        _inventorySlot = inventorySlot;
        UpdateInventorySlot();
    }

    public void OnClick()
    {
        if (!isSplitting)
        {
            selectionBox.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
            selectionBox.SetActive(!selectionBox.activeSelf);
            selectionBox.GetComponent<RectTransform>().SetAsLastSibling();
            selectionBox.GetComponent<RectTransform>().position = gameObject.transform.position + new Vector3(160f, 0f, 0f);
        }
        CustomEvents.ShopSystem.OnOpenInspectionShop?.Invoke(_inventorySlot, false, _shopID);
        if (_inventorySlot.Item.ItemType == ItemTypes.crop)
        {
            CustomEvents.InventorySystem.OnShowCropPediaButtons?.Invoke((SCR_CropItems)_inventorySlot.Item, 2);
        }
        else
        {
            CustomEvents.InventorySystem.OnDisableCroppediaButton2?.Invoke();
        }
    }

    public void OnBuy()
    {
        if (_inventorySlot.Amount > 1)
        {
            isSplitting = true;
            splitMenu.SetActive(!splitMenu.activeSelf);
            warning.SetActive(false);
            splitMenu.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
            splitMenu.GetComponent<RectTransform>().SetAsLastSibling();
            splitMenu.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            splitSlider.maxValue = _inventorySlot.Amount;
            splitSlider.minValue = 0;
            splitSlider.value = 0;
            stackOne = _inventorySlot.Amount;
            stackTwo = 0;
            stackOneTextInput.text = stackTwo.ToString();
            stackTwoTextInput.text = stackOne.ToString();
            switch (_inventorySlot.Quality)
            {
                case ItemQuality.Bad:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValueBad * stackTwo);
                    break;
                case ItemQuality.Normal:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValueNormal * stackTwo);
                    break;
                case ItemQuality.Good:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValueGood * stackTwo);
                    break;
                case ItemQuality.Perfect:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValuePerfect * stackTwo);
                    break;
            }
        }
        else
        {
            stackTwo = 1;
            BuyStack();
        }
    }

    public void OnChangeSellSlider()
    {
        stackOne = _inventorySlot.Amount - (int)splitSlider.value;
        stackTwo = (int)splitSlider.value;
        stackOneTextInput.text = stackTwo.ToString();
        stackTwoTextInput.text = stackOne.ToString();
        switch (_inventorySlot.Quality)
        {
            case ItemQuality.Bad:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValueBad * stackTwo);
                break;
            case ItemQuality.Normal:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValueNormal * stackTwo);
                break;
            case ItemQuality.Good:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValueGood * stackTwo);
                break;
            case ItemQuality.Perfect:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.BuyValuePerfect * stackTwo);
                break;
        }
    }

    public void OnStackOneEndEdit()
    {
        string temp = stackOneTextInput.text.Replace("\n", "");
        int tempNo;
        int.TryParse(temp, out tempNo);
        if (tempNo < 0)
        {
            stackTwo = 0;
        }
        else if (tempNo > _inventorySlot.Amount)
        {
            stackTwo = _inventorySlot.Amount;
        }
        else
        {
            stackTwo = tempNo;
        }
        stackOne = _inventorySlot.Amount - stackTwo;
        stackOneTextInput.text = stackTwo.ToString();
        stackTwoTextInput.text = stackOne.ToString();
        splitSlider.value = stackTwo;
    }

    public void OnStackTwoEndEdit()
    {
        string temp = stackTwoTextInput.text.Replace("\n", "");
        int tempNo;
        int.TryParse(temp, out tempNo);
        if (tempNo < 0)
        {
            stackOne = 0;
        }
        else if (tempNo > _inventorySlot.Amount)
        {
            stackOne = _inventorySlot.Amount;
        }
        else
        {
            stackOne = tempNo;
        }
        stackTwo = _inventorySlot.Amount - stackOne;
        stackOneTextInput.text = stackTwo.ToString();
        stackTwoTextInput.text = stackOne.ToString();
        splitSlider.value = stackTwo;
    }

    public void OnBuyStack()
    {
        BuyStack();
        isSplitting = false;
        splitMenu.SetActive(!splitMenu.activeSelf);
    }

    private void BuyStack()
    {
        Destroy(_toolTip);
        _toolTip = null;
        int tempPrice = 0;
        switch (_inventorySlot.Quality)
        {
            case ItemQuality.Bad:
                tempPrice = (_inventorySlot.Item.BuyValueBad * stackTwo);
                break;
            case ItemQuality.Normal:
                tempPrice = (_inventorySlot.Item.BuyValueNormal * stackTwo);
                break;
            case ItemQuality.Good:
                tempPrice = (_inventorySlot.Item.BuyValueGood * stackTwo);
                break;
            case ItemQuality.Perfect:
                tempPrice = (_inventorySlot.Item.BuyValuePerfect * stackTwo);
                break;
        }
        int tempCurrencyAmount = (int)CustomEvents.Currency.OnGetCurrency();

        if (tempPrice <= tempCurrencyAmount)
        {
            if (_inventorySlot.Item.ItemType != ItemTypes.unique)
            {
                if (stackTwo >= 1 && _inventorySlot.Amount > 1)
                {
                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(_inventorySlot.Item, stackTwo, _inventorySlot.Quality);
                    CustomEvents.Currency.OnRemoveCurrency?.Invoke(tempPrice);
                    _inventorySlot.DecreaseAmount(stackTwo);
                    if (_inventorySlot.Amount == 0)
                    {
                        CustomEvents.ShopSystem.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot, _shopID);
                    }
                    CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
                    stackOne = 1;
                    stackTwo = 0;
                }
                else if (stackTwo == 0)
                {
                    stackOne = 1;
                    stackTwo = 0;
                }
                else
                {
                    CustomEvents.ShopSystem.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot, _shopID);
                    stackOne = 1;
                    stackTwo = 0;
                }
            }
            else
            {
                SCR_UniqueItems currentItem = (SCR_UniqueItems)_inventorySlot.Item;

                if (currentItem.Desciption == UniqueItems.recipe)
                {
                    CustomEvents.AvailableItems.OnRemoveFromSpecialShops?.Invoke(_inventorySlot.Item);
                    CustomEvents.AvailableItems.OnRemoveFromShops?.Invoke(_inventorySlot.Item);
                }

                currentItem.PerformLinkedAction();
                CustomEvents.Currency.OnRemoveCurrency?.Invoke(tempPrice);
                CustomEvents.ShopSystem.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot, _shopID);
                CustomEvents.ShopSystem.OnUpdateInventoryUI?.Invoke(_shopID);
            }
        }
        else
        {
            warning.SetActive(true);
            warning.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
            warning.GetComponent<RectTransform>().SetAsLastSibling();
            warning.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        }
        CustomEvents.ShopSystem.OnOpenInspectionShop?.Invoke(_inventorySlot, false, _shopID);
    }

    public void OnCloseWarning()
    {
        warning.SetActive(false);
        isSplitting = false;
        splitMenu.SetActive(!splitMenu.activeSelf);
    }

    public void OnCloseSplit()
    {
        splitMenu.SetActive(false);
        isSplitting = false;
    }

    public void OnInspect()
    {
        selectionBox.SetActive(false);
        CustomEvents.ShopSystem.OnOpenInspectionShop?.Invoke(_inventorySlot, true, _shopID);
        if (_inventorySlot.Item.ItemType == ItemTypes.crop)
        {
            CustomEvents.InventorySystem.OnShowCropPediaButtons?.Invoke((SCR_CropItems)_inventorySlot.Item, 2);
        }
        else
        {
            CustomEvents.InventorySystem.OnDisableCroppediaButton2?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Vector3 mousePos = data.position;
        mousePos.x += 110f;
        mousePos.y -= 75f;
        _toolTip = Instantiate(toolTip, mousePos, Quaternion.identity, GetComponentInParent<Canvas>().gameObject.transform);
        if (_inventorySlot.Item.IgnoreQuality)
        {
            _toolTip.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("Name: {0}\nAmount: {1}", _inventorySlot.Item.name, _inventorySlot.Amount.ToString());
        }
        else
        {
            _toolTip.GetComponentInChildren<TextMeshProUGUI>().text = string.Format("Name: {0}\nQuality: {1}\nAmount: {2}", _inventorySlot.Item.name, _inventorySlot.Quality.ToString(), _inventorySlot.Amount.ToString());
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        StartCoroutine(CheckExitTime());
        Destroy(_toolTip);
        _toolTip = null;
    }

    public void OnPointerMove(PointerEventData data)
    {
        if (_toolTip != null)
        {
            Vector3 mousePos = data.position;
            mousePos.x += 110f;
            mousePos.y -= 75f;
            _toolTip.transform.position = mousePos;
        }
    }

    private void OnDestroy()
    {
        Destroy(selectionBox);
        Destroy(_toolTip);
        Destroy(splitMenu);
    }

    private IEnumerator CheckExitTime()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        if (!isHovering)
        {
            selectionBox.SetActive(false);
        }
    }
}
