using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_BagSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Inventory Slot Data")]
    [SerializeField] private InventorySlot _inventorySlot;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;

    [Header("Implementation Data")]
    [SerializeField] private GameObject selectionBox;
    private int _bagID;
    public int BagID { get => _bagID; set => _bagID = value; }
    [SerializeField] private GameObject toolTip;
    private GameObject _toolTip = null;

    [SerializeField] private GameObject mergeOptions;
    [SerializeField] private GameObject moveOptions;
    [SerializeField] private GameObject movingSlot;
    [SerializeField] private GameObject movingOverlay;
    private GameObject _movingSlot;
    private bool moving = false;
    private InventorySlot movingInvSlot;

    [Header("Split Data")]
    [SerializeField] private GameObject splitButton;
    [SerializeField] private GameObject splitMenu;
    [SerializeField] private Slider splitSlider;
    [SerializeField] private TextMeshProUGUI stackOneText;
    [SerializeField] private TextMeshProUGUI stackTwoText;
    private bool isSplitting = false;
    private int stackOne;
    private int stackTwo;

    [Header("Open Seeds Data")]
    [SerializeField] private GameObject openButton;

    private bool isHovering = false;
    public void SetIsHovering(bool value) { isHovering = value; }

    private void Awake()
    {
        _inventorySlot = new InventorySlot();
        UpdateInventorySlot();
    }

    public void SetInventorySlot(InventorySlot inventorySlot)
    {
        _inventorySlot = inventorySlot;
        UpdateInventorySlot();
    }

    public void UpdateInventorySlot()
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

    public void DeleteSlot()
    {
        Destroy(gameObject);
    }

    public void OnMove()
    {
        if (!moving && !isSplitting)
        {
            OnMovingStateChange(true);
            movingOverlay.SetActive(true);
            _movingSlot = Instantiate(movingSlot, gameObject.transform.position, Quaternion.identity, GetComponentInParent<Canvas>().gameObject.transform);
            _movingSlot.GetComponent<SCR_MoveableSlot>().SetValues(_inventorySlot, "Bag");
            CustomEvents.InventorySystem.BagInventory.OnOpenInspection?.Invoke(_inventorySlot, false, _bagID);
            if (_inventorySlot.Item.ItemType == ItemTypes.crop)
            {
                CustomEvents.InventorySystem.OnShowCropPediaButton?.Invoke((SCR_CropItems)_inventorySlot.Item);
            }
            else
            {
                CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
            }
            CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(true);
        }
        else
        {
            movingInvSlot = (InventorySlot)CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventorySlot?.Invoke();
            if (_inventorySlot != movingInvSlot && movingInvSlot != null)
            {
                if ((movingInvSlot.Item == _inventorySlot.Item) && (movingInvSlot.Quality == _inventorySlot.Quality))
                {
                    mergeOptions.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
                    mergeOptions.SetActive(!mergeOptions.activeSelf);
                    mergeOptions.GetComponent<RectTransform>().SetAsLastSibling();
                }
                else
                {
                    moveOptions.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
                    moveOptions.SetActive(!mergeOptions.activeSelf);
                    moveOptions.GetComponent<RectTransform>().SetAsLastSibling();
                }
            }
            else
            {
                OnMovingStateChange(false);
                CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
            }
        }

    }

    public void OnClick()
    {
        if(!moving)
        {
            if (!isSplitting)
            {
                selectionBox.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
                selectionBox.SetActive(!selectionBox.activeSelf);
                selectionBox.GetComponent<RectTransform>().SetAsLastSibling();
                if (_inventorySlot.Item.ItemType != ItemTypes.seedPacket)
                {
                    splitButton.SetActive(true);
                    openButton.SetActive(false);
                }
                else
                {

                    splitButton.SetActive(false);
                    openButton.SetActive(true);
                }
            }
            CustomEvents.InventorySystem.BagInventory.OnOpenInspection?.Invoke(_inventorySlot, false, BagID);
            if (_inventorySlot.Item.ItemType == ItemTypes.crop)
            {
                CustomEvents.InventorySystem.OnShowCropPediaButton?.Invoke((SCR_CropItems)_inventorySlot.Item);
            }
            else
            {
                CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
            }
        }
    }

    public void OnTake()
    {
        if(!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            CustomEvents.InventorySystem.BagInventory.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot, _bagID);
            CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(_inventorySlot.Item, _inventorySlot.Amount, _inventorySlot.Quality);
        }
    }

    public void OnSplitStackOption()
    {
        if(!moving)
        {
            if (_inventorySlot.Amount > 1)
            {
                isSplitting = true;
                splitMenu.SetActive(!splitMenu.activeSelf);
                splitMenu.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
                splitMenu.GetComponent<RectTransform>().SetAsLastSibling();
                splitMenu.GetComponent<RectTransform>().position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                splitSlider.maxValue = _inventorySlot.Amount - 1;
                splitSlider.minValue = 1;
                splitSlider.value = 1;
                stackOne = _inventorySlot.Amount - (int)splitSlider.value;
                stackTwo = (int)splitSlider.value;
                stackOneText.text = string.Format("Stack One: \n {0}", stackOne);
                stackTwoText.text = string.Format("Stack Two: \n {0}", stackTwo);
            }
        }
    }

    public void OnChangeSplitSlider()
    {
        if(!moving)
        {
            stackOne = _inventorySlot.Amount - (int)splitSlider.value;
            stackTwo = (int)splitSlider.value;
            stackOneText.text = string.Format("Stack One: \n {0}", stackOne);
            stackTwoText.text = string.Format("Stack Two: \n {0}", stackTwo);
        }

    }

    public void OnSplitStack()
    {
        if(!moving)
        {
            SplitStack();
            isSplitting = false;
        }
    }

    private void SplitStack()
    {
        if(!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            _inventorySlot.DecreaseAmount(stackTwo);
            CustomEvents.InventorySystem.BagInventory.OnSplit?.Invoke(_inventorySlot.Item, stackTwo, _inventorySlot.Quality, _bagID);
            CustomEvents.InventorySystem.BagInventory.OnOpenInspection?.Invoke(_inventorySlot, false, BagID);
        }
    }

    public void OnOpenSeedPacket()
    {
        if(!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            SCR_SeedPacketItems seedPacket = (SCR_SeedPacketItems)_inventorySlot.Item;
            foreach (SCR_Items item in seedPacket.GetContents())
            {
                CustomEvents.InventorySystem.BagInventory.OnAddNewItemStack(item, 1, ItemQuality.Normal, _bagID);
            }
            CustomEvents.InventorySystem.BagInventory.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot, _bagID);
        }
    }

    public void OnCloseSplit()
    {
        if(!moving)
        {
            splitMenu.SetActive(false);
            isSplitting = false;
        }
    }

    public void OnInspect()
    {
        if(!moving)
        {
            selectionBox.SetActive(false);
            CustomEvents.InventorySystem.BagInventory.OnOpenInspection?.Invoke(_inventorySlot, true, BagID);
            if (_inventorySlot.Item.ItemType == ItemTypes.crop)
            {
                CustomEvents.InventorySystem.OnShowCropPediaButton?.Invoke((SCR_CropItems)_inventorySlot.Item);
            }
            else
            {
                CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
            }
        }
    }


    public void OnMovingStateChange(bool newState)
    {
        moving = newState;
        if (!moving)
        {
            gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
            if (_movingSlot != null)
            {
                Destroy(_movingSlot);
                _movingSlot = null;
                movingOverlay.SetActive(false);
            }
        }
        else
        {
            Destroy(_toolTip);
            selectionBox.SetActive(false);
            splitMenu.SetActive(false);
        }

    }

    public void StopHolding()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
    }

    public void MergeAll()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.BagInventory.OnMergeSlotsToAllOfType?.Invoke(movingInvSlot, _inventorySlot, _bagID);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
    }

    public void MergeSlots()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.BagInventory.OnMergeSlots?.Invoke(movingInvSlot, _inventorySlot, _bagID);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
    }

    public void MoveSlotHere()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.BagInventory.OnMoveSlots?.Invoke(movingInvSlot, _inventorySlot, _bagID);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
    }

    public void SwapSlots()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.BagInventory.OnSwapInventorySlots?.Invoke(movingInvSlot, _inventorySlot, _bagID);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged?.Invoke(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(!moving)
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
        else
        {
            gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(!moving)
        {
            StartCoroutine(CheckExitTime());
            Destroy(_toolTip);
            _toolTip = null;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    public void OnPointerMove(PointerEventData data)
    {
        if(!moving)
        {
            if (_toolTip != null)
            {
                Vector3 mousePos = data.position;
                mousePos.x += 110f;
                mousePos.y -= 75f;
                _toolTip.transform.position = mousePos;
            }
        }   
    }

    private void OnDestroy()
    {
        Destroy(selectionBox);
        Destroy(_toolTip);
        Destroy(splitMenu);
        Destroy(_movingSlot);
    }

    private IEnumerator CheckExitTime()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        if (!isHovering)
        {
            selectionBox.SetActive(false);
            mergeOptions.SetActive(false);
            moveOptions.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged += OnMovingStateChange;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.BagInventory.OnMovingChanged -= OnMovingStateChange;
    }
}
