using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Inventory Slot Data")]
    [SerializeField] private InventorySlot _inventorySlot;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;

    [Header("Implementation Data")]
    [SerializeField] private GameObject selectionBox;
    [SerializeField] private GameObject bagPrefab;
    [SerializeField] private GameObject plantOptions;
    [SerializeField] private GameObject toolTip;
    private GameObject _toolTip = null;
    private GameObject panel = null;

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

    private bool isPlanting = false;
    public bool IsPlanting { get => isPlanting; set => isPlanting = value; }
    private bool isFertilizing = false;
    public bool IsFertilizing { get => isFertilizing; set => isFertilizing = value; }

    private bool isPlantingTree = false;
    public bool IsPlantingTree{ get => isPlantingTree; set => isPlantingTree = value; }

    private PlotBehaviour _plot;
    public PlotBehaviour Plot { get => _plot; set => _plot = value; }

    private FruitTreePlotBehaviour _treePlot;
    public FruitTreePlotBehaviour TreePlot{ get => _treePlot; set => _treePlot = value; }

    private bool isHovering = false;
    public void SetIsHovering(bool value) { isHovering = value; }

    private void Awake()
    {
        _inventorySlot = new InventorySlot();
        UpdateInventorySlot();
        CustomEvents.InventorySystem.PlayerInventory.DropItem += DropStack;
    }

    private void Start()
    {
        panel = GetComponentInParent<ScrollRect>().gameObject;
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
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!isPlanting && !isFertilizing && !isPlantingTree)
        {
            if (!moving)
            {
                OnMovingStateChange(true);
                movingOverlay.SetActive(true);
                _movingSlot = Instantiate(movingSlot, gameObject.transform.position, Quaternion.identity, GetComponentInParent<Canvas>().gameObject.transform);
                _movingSlot.GetComponent<SCR_MoveableSlot>().SetValues(_inventorySlot, "Silo");
                CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection?.Invoke(_inventorySlot, false);
                if (_inventorySlot.Item.ItemType == ItemTypes.crop)
                {
                    CustomEvents.InventorySystem.OnShowCropPediaButton?.Invoke((SCR_CropItems)_inventorySlot.Item);
                }
                else
                {
                    CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
                }
                CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(true);
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
                    CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
                }
            }
        }
        else
        {
            plantOptions.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
            plantOptions.SetActive(!selectionBox.activeSelf);
            plantOptions.GetComponent<RectTransform>().SetAsLastSibling();
        }        
    }

    public void OnClick()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            if (!isPlanting && !isFertilizing && !isPlantingTree)
            {
                if (!isSplitting)
                {
                    selectionBox.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
                    selectionBox.SetActive(!selectionBox.activeSelf);
                    selectionBox.GetComponent<RectTransform>().SetAsLastSibling();
                    //selectionBox.GetComponent<RectTransform>().position = gameObject.transform.position + new Vector3(250f, 0f, 0f);
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
            else
            {
                plantOptions.transform.SetParent(GetComponentInParent<Canvas>().gameObject.transform);
                plantOptions.SetActive(!selectionBox.activeSelf);
                plantOptions.GetComponent<RectTransform>().SetAsLastSibling();
            }
            CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection?.Invoke(_inventorySlot, false);
            if (_inventorySlot.Item.ItemType == ItemTypes.crop)
            {
                CustomEvents.InventorySystem.OnShowCropPediaButton?.Invoke((SCR_CropItems)_inventorySlot.Item);
            }
            else
            {
                CustomEvents.InventorySystem.OnDisableCroppediaButton?.Invoke();
            }
        }
        else
        {
            CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection?.Invoke(_inventorySlot, false);
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

    public void OnSplitStackOption()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            if(_inventorySlot.Amount > 1)
            {
                selectionBox.SetActive(false);
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
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            stackOne = _inventorySlot.Amount - (int)splitSlider.value;
            stackTwo = (int)splitSlider.value;
            stackOneText.text = string.Format("Stack One: \n {0}", stackOne);
            stackTwoText.text = string.Format("Stack Two: \n {0}", stackTwo);
        }
    }

    public void OnSplitStack()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
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
            CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection?.Invoke(_inventorySlot, false);
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
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            DropStack(_inventorySlot);
        }
    }

    public void OnUse()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            Destroy(_toolTip);
            _toolTip = null;
            CustomEvents.TimeCycle.OnUnpause?.Invoke();
            if (isPlanting)
            {
                iExecutable plantTask = new PlantTask(_plot, _inventorySlot.Item.ItemName, _inventorySlot);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(plantTask);

                CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(_inventorySlot.Item, 1, _inventorySlot.Quality);
                CustomEvents.InventorySystem.PlayerInventory.OnResetUIInventory?.Invoke();
            }
            else if (isFertilizing)
            {
                iExecutable fertiliseTask = new FertiliseTask(_plot, _inventorySlot.Item.ItemName, _inventorySlot);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(fertiliseTask);

                CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(_inventorySlot.Item, 1, _inventorySlot.Quality);
                CustomEvents.InventorySystem.PlayerInventory.OnResetUIInventory?.Invoke();
            }
            else if (isPlantingTree)
            {
                iExecutable plantTreeTask = new PlantTreeTask(_treePlot, _inventorySlot.Item.ItemName, _inventorySlot);
                CustomEvents.TaskSystem.OnAddNewTask?.Invoke(plantTreeTask);
                
                CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(_inventorySlot.Item, 1, _inventorySlot.Quality);
                CustomEvents.InventorySystem.PlayerInventory.OnResetUIInventory?.Invoke();
            }
        }
    }

    public void OnBuyItem()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (_inventorySlot.Item != null)
        {
            Destroy(_toolTip);
            _toolTip = null;
            CustomEvents.ShopSystem.OnBuyItem?.Invoke(_inventorySlot.Item);
            //CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(_inventorySlot.Item, _inventorySlot.Amount);
        }
    }

    public void OnOpenSeedPacket()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
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

    public void OnCloseSplit()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            splitMenu.SetActive(false);
            isSplitting = false;
        }
    }

    public void OnInspect()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        if (!moving)
        {
            selectionBox.SetActive(false);
            CustomEvents.InventorySystem.PlayerInventory.OnOpenInspection?.Invoke(_inventorySlot, true);
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
        if(!moving)
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
            plantOptions.SetActive(false);
        }

    }

    public void StopHolding()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
    }

    public void MergeAll()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlotsToAllOfType?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
    }

    public void MergeSlots()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMergeSlots?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
    }

    public void MoveSlotHere()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMoveSlots?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
    }

    public void SwapSlots()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Button");
        moveOptions.SetActive(false);
        mergeOptions.SetActive(false);
        CustomEvents.InventorySystem.PlayerInventory.OnSwapInventorySlots?.Invoke(movingInvSlot, _inventorySlot);
        OnMovingStateChange(false);
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged?.Invoke(false);
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
            gameObject.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
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
            gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    public void OnPointerMove(PointerEventData data)
    {
        if(!moving)
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

    private void OnDestroy()
    {
        Destroy(selectionBox);
        Destroy(plantOptions);
        Destroy(_toolTip);
        Destroy(splitMenu);
        Destroy(_movingSlot);
        Destroy(mergeOptions);
        Destroy(moveOptions);
    }

    private IEnumerator CheckExitTime()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        if (!isHovering)
        {
            selectionBox.SetActive(false);
            plantOptions.SetActive(false);
            mergeOptions.SetActive(false);
            moveOptions.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged += OnMovingStateChange;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnMovingChanged -= OnMovingStateChange;
    }

}
