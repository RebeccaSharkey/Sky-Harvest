using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SCR_CraftMachineSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Inventory Slot Data")]
    [SerializeField] private InventorySlot _inventorySlot;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;

    [Header("Implementation Data")]
    [SerializeField] private GameObject toolTip;
    private GameObject _toolTip = null;

    private int machineID = 0;

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
        Destroy(_toolTip);
        _toolTip = null;
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(_inventorySlot.Item, _inventorySlot.Amount, _inventorySlot.Quality);
        CustomEvents.CraftMachine.OnRemoveItemStackWithSlot(_inventorySlot, machineID);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Vector3 mousePos = data.position;
        mousePos.x += 110f;
        mousePos.y -= 75f;
        _toolTip = Instantiate(toolTip, mousePos, Quaternion.identity, GetComponentInParent<Canvas>().gameObject.transform);
        if(_inventorySlot.Item.IgnoreQuality)
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
        Destroy(_toolTip);
    }
}
