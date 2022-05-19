using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_PlayerCraftMachineSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Inventory Slot Data")]
    [SerializeField] private InventorySlot _inventorySlot;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;

    [Header("Implementation Data")]
    [SerializeField] private GameObject selectionBox;
    private int _siloID;
    [SerializeField] private GameObject toolTip;
    private GameObject _toolTip = null;

    [Header("Split Data")]
    [SerializeField] private GameObject splitButton;
    [SerializeField] private GameObject splitMenu;
    [SerializeField] private Slider splitSlider;
    [SerializeField] private TextMeshProUGUI stackOneText;
    [SerializeField] private TextMeshProUGUI stackTwoText;
    private bool isSplitting = false;
    private int stackOne;
    private int stackTwo;

    [Header("Merge and Move Options")]
    [SerializeField] private GameObject mergeOptions;
    [SerializeField] private GameObject moveOptions;
    [SerializeField] private GameObject movingSlot;
    [SerializeField] private GameObject movingOverlay;
    private GameObject _movingSlot;
    private bool moving = false;
    private InventorySlot movingInvSlot;

    [Header("Open Seeds Data")]
    [SerializeField] private GameObject openButton;

    private int machineID = 0;
    private bool isHovering = false;
    public void SetIsHovering(bool value) { isHovering = value; }

    private void Awake()
    {
        _inventorySlot = new InventorySlot();
        UpdateInventorySlot();
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

    public void SetInventorySlot(InventorySlot inventorySlot, int id)
    {
        _inventorySlot = inventorySlot;
        machineID = id;
        UpdateInventorySlot();
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
        }
        CustomEvents.CraftMachine.OnOpenInspection?.Invoke(_inventorySlot, false, machineID);
    }

    public void OnMove()
    {
        if (!moving)
        {
            OnMovingStateChange(true);
            movingOverlay.SetActive(true);
            _movingSlot = Instantiate(movingSlot, gameObject.transform.position, Quaternion.identity, GetComponentInParent<Canvas>().gameObject.transform);
            _movingSlot.GetComponent<SCR_MoveableSlot>().SetValues(_inventorySlot, "Player", machineID);
            CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection?.Invoke(_inventorySlot, false);
            CustomEvents.CraftMachine.OnMovingChanged?.Invoke(true);
        }
        else
        {
            movingInvSlot = (InventorySlot)CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventorySlot?.Invoke();
            if ((string)CustomEvents.InventorySystem.PlayerInventory.GetMoveablesInventoryOrigin?.Invoke() == "Player")
            {
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
                    CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
                }
            }
        }
    }

    public void OnPlace()
    {
        if(!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            if ((bool)CustomEvents.CraftMachine.OnAddNewItemStack?.Invoke(_inventorySlot.Item, _inventorySlot.Amount, machineID, _inventorySlot.Quality))
            {
                CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot(_inventorySlot);
            }
        }
    }

    public void OnSplitOptionButton()
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
            CustomEvents.InventorySystem.PlayerInventory.OnSplit?.Invoke(_inventorySlot.Item, stackTwo, _inventorySlot.Quality);
            CustomEvents.CraftMachine.OnOpenInspection?.Invoke(_inventorySlot, false, machineID);
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

    public void OnOpenSeedPacket()
    {
        if(!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            SCR_SeedPacketItems seedPacket = (SCR_SeedPacketItems)_inventorySlot.Item;
            foreach (SCR_Items item in seedPacket.GetContents())
            {
                CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack(item, 1, ItemQuality.Normal);
            }
            CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot);
        }
    }

    public void OnInspect()
    {
        if (!moving)
        {
            selectionBox.SetActive(false);
            CustomEvents.CraftMachine.OnOpenInspection?.Invoke(_inventorySlot, true, machineID);
        }
    }

    private void DropStack(InventorySlot invSlot)
    {
        if (!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            if (invSlot == _inventorySlot)
            {
                int tempValue = (int)(CustomEvents.InventorySystem.PlayerInventory.FindClosestBagID?.Invoke());
                if (tempValue == -1)
                {
                    CustomEvents.InventorySystem.PlayerInventory.CreatBag?.Invoke();
                    CustomEvents.InventorySystem.BagInventory.OnAddNewItemStack?.Invoke(_inventorySlot.Item, _inventorySlot.Amount, _inventorySlot.Quality, (int)(CustomEvents.InventorySystem.PlayerInventory.FindClosestBagID?.Invoke()));
                    CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot);
                }
                else
                {
                    CustomEvents.InventorySystem.BagInventory.OnAddNewItemStack?.Invoke(_inventorySlot.Item, _inventorySlot.Amount, _inventorySlot.Quality, tempValue);
                    CustomEvents.InventorySystem.PlayerInventory.OnRemoveItemStackWithSlot?.Invoke(_inventorySlot);
                }
            }
        }
    }

    public void OnDropStackClick()
    {
        if(!moving)
        {
            DropStack(_inventorySlot);
        }
    }

    public void OnMovingStateChange(bool newState)
    {
        moving = newState;
        if (!moving)
        {
            gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
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
        CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
    }

    public void MergeAll()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlotsToAllOfType?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
    }

    public void MergeSlots()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlots?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
    }

    public void MoveSlotHere()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMoveSlots?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
    }

    public void SwapSlots()
    {
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnSwapInventorySlots?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.CraftMachine.OnMovingChanged?.Invoke(false);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (!moving)
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
            gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        StartCoroutine(CheckExitTime());
        if (!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
    }

    public void OnPointerMove(PointerEventData data)
    {
        if (!moving)
        {
            Vector3 mousePos = data.position;
            mousePos.x += 110f;
            mousePos.y -= 75f;
            if (_toolTip != null)
            {
                _toolTip.transform.position = mousePos;
            }
        }
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

    private void OnDestroy()
    {
        Destroy(_toolTip);
        Destroy(selectionBox);
        Destroy(splitMenu);
        Destroy(_movingSlot);
        Destroy(mergeOptions);
        Destroy(moveOptions);
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
