using System.Collections.Generic;
using UnityEngine;

public class PickupNotifManager : MonoBehaviour
{
    [SerializeField] private int maxConcurrentNotifs = 5;
    [SerializeField] private float notifLifetime = 3f;

    private Queue<PickupNotifData> queue;
    private int activeNotifs;

    private void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack += ItemPickup;
        CustomEvents.InventorySystem.PlayerInventory.OnAddToItemStack += ItemPickup;
        CustomEvents.UI.OnPickupNotifDestroyed += OnNotifDestroyed;
    }

    private void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack -= ItemPickup;
        CustomEvents.InventorySystem.PlayerInventory.OnAddToItemStack -= ItemPickup;
        CustomEvents.UI.OnPickupNotifDestroyed -= OnNotifDestroyed;
    }

    private void Start()
    {
        queue = new Queue<PickupNotifData>();
    }

    private void ItemPickup(SCR_Items _item, int _amount, ItemQuality _quality)
    {
        queue.Enqueue(new PickupNotifData(_item.ItemSprite, _item.name, _quality.ToString(), _amount.ToString()));
        TryCreatePopup();
    }

    private void OnNotifDestroyed()
    {
        activeNotifs--;
        TryCreatePopup();
    }

    private void TryCreatePopup()
    {
        if (activeNotifs >= maxConcurrentNotifs) return;
        if (queue.Count <= 0) return;

        GameObject newNotif = Instantiate(Resources.Load("Graphics/PickupNotif/ItemPopup", typeof(GameObject))) as GameObject;
        activeNotifs++;
        newNotif.transform.SetParent(transform);
        newNotif.GetComponent<PickupPopupSetter>().SetValues(queue.Dequeue());
        Destroy(newNotif, notifLifetime);
    }
}

public class PickupNotifData
{
    public Sprite icon;
    public string name, quality, quantity;

    public PickupNotifData(Sprite _sprite, string _name, string _quality, string _quantity)
    {
        icon = _sprite;
        name = _name;
        quality = _quality;
        quantity = _quantity;
    }
}
