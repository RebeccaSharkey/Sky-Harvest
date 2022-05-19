using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_SpecialShopPlayerUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
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
    [SerializeField] private GameObject splitMenu;
    [SerializeField] private Slider splitSlider;
    [SerializeField] private TMP_InputField stackOneText;
    [SerializeField] private TMP_InputField stackTwoText;
    [SerializeField] private TextMeshProUGUI totalPrice;
    private bool isSplitting = false;
    private int stackOne = 0;
    private int stackTwo = 1;

    private bool isHovering = false;
    public void SetIsHovering(bool value) { isHovering = value; }

    private void Awake()
    {
        _inventorySlot = new InventorySlot();
        UpdateInventorySlot();
    }

    private void Start()
    {
        _shopID = GetComponentInParent<SCR_SpecialShopInventoryUI>().ShopId;
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
        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionPlayer?.Invoke(_inventorySlot, false, _shopID);
        if (_inventorySlot.Item.ItemType == ItemTypes.crop)
        {
            CustomEvents.InventorySystem.OnShowCropPediaButtons?.Invoke((SCR_CropItems)_inventorySlot.Item, 1);
        }
        else
        {
            CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
        }
    }

    public void OnSell()
    {
        if (_inventorySlot.Amount > 1)
        {
            isSplitting = true;
            splitMenu.SetActive(!splitMenu.activeSelf);
            splitMenu.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
            splitMenu.GetComponent<RectTransform>().SetAsLastSibling();
            splitMenu.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            splitSlider.maxValue = _inventorySlot.Amount;
            splitSlider.minValue = 0;
            splitSlider.value = 0;
            stackOne = _inventorySlot.Amount - (int)splitSlider.value;
            stackTwo = (int)splitSlider.value;
            stackOneText.text = stackOne.ToString();
            stackTwoText.text = stackTwo.ToString();
            switch (_inventorySlot.Quality)
            {
                case ItemQuality.Bad:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValueBad * stackTwo);
                    break;
                case ItemQuality.Normal:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValueNormal * stackTwo);
                    break;
                case ItemQuality.Good:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValueGood * stackTwo);
                    break;
                case ItemQuality.Perfect:
                    totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValuePerfect * stackTwo);
                    break;
            }
        }
        else
        {
            SellStack();
        }
    }

    public void OnChangeSellSlider()
    {
        stackOne = _inventorySlot.Amount - (int)splitSlider.value;
        stackTwo = (int)splitSlider.value;
        stackOneText.text = stackOne.ToString();
        stackTwoText.text = stackTwo.ToString();
        switch (_inventorySlot.Quality)
        {
            case ItemQuality.Bad:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValueBad * stackTwo);
                break;
            case ItemQuality.Normal:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValueNormal * stackTwo);
                break;
            case ItemQuality.Good:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValueGood * stackTwo);
                break;
            case ItemQuality.Perfect:
                totalPrice.text = string.Format("Total Price: {0}(RD)", _inventorySlot.Item.SellValuePerfect * stackTwo);
                break;
        }
    }

    public void OnStackOneEndEdit()
    {
        string temp = stackOneText.text.Replace("\n", "");
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
        stackOneText.text = stackOne.ToString();
        stackTwoText.text = stackTwo.ToString();
        splitSlider.value = stackTwo;
    }

    public void OnStackTwoEndEdit()
    {
        string temp = stackTwoText.text.Replace("\n", "");
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
        stackOneText.text = stackOne.ToString();
        stackTwoText.text = stackTwo.ToString();
        splitSlider.value = stackTwo;
    }

    public void OnSellStack()
    {
        SellStack();
        isSplitting = false;
        splitMenu.SetActive(!splitMenu.activeSelf);
    }

    private void SellStack()
    {
        Destroy(_toolTip);
        _toolTip = null;
        CustomEvents.ShopSystem.SpecialShop.OnAddNewItemStack?.Invoke(_inventorySlot.Item, stackTwo, _inventorySlot.Quality, _shopID);
        switch (_inventorySlot.Quality)
        {
            case ItemQuality.Bad:
                CustomEvents.Currency.OnAddCurrency?.Invoke(_inventorySlot.Item.SellValueBad * stackTwo);
                break;
            case ItemQuality.Normal:
                CustomEvents.Currency.OnAddCurrency?.Invoke(_inventorySlot.Item.SellValueNormal * stackTwo);
                break;
            case ItemQuality.Good:
                CustomEvents.Currency.OnAddCurrency?.Invoke(_inventorySlot.Item.SellValueGood * stackTwo);
                break;
            case ItemQuality.Perfect:
                CustomEvents.Currency.OnAddCurrency?.Invoke(_inventorySlot.Item.SellValuePerfect * stackTwo);
                break;
        }
        if (stackOne > 0)
        {
            _inventorySlot.DecreaseAmount(stackTwo);
            CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI?.Invoke();
            stackOne = 0;
            stackTwo = 1;
        }
        else
        {
            CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot);
            stackOne = 0;
            stackTwo = 1;
        }
        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionPlayer?.Invoke(_inventorySlot, false, _shopID);
    }

    public void OnCloseSplit()
    {
        splitMenu.SetActive(false);
        isSplitting = false;
    }


    public void OnInspect()
    {
        selectionBox.SetActive(false);
        CustomEvents.ShopSystem.SpecialShop.OnOpenInspectionPlayer?.Invoke(_inventorySlot, true, _shopID);
        if (_inventorySlot.Item.ItemType == ItemTypes.crop)
        {
            CustomEvents.InventorySystem.OnShowCropPediaButtons?.Invoke((SCR_CropItems)_inventorySlot.Item, 1);
        }
        else
        {
            CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
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
