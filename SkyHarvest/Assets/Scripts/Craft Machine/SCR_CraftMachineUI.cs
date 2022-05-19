using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_CraftMachineUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private List<GameObject> objectsToHide;
    [SerializeField] private GameObject craftMenu;

    [Header("Craft Machine Variables")]
    [SerializeField] private List<GameObject> craftingSlots;
    [SerializeField] private GameObject blankSlot;
    [SerializeField] private GameObject craftingSlot;
    private Inventory craftMachineInventory;

    [Header("Player Data")]
    [SerializeField] private GameObject uiInventorySlot;
    [SerializeField] private Transform playerContainer;
    private Inventory playerInventory;
    [SerializeField] private TextMeshProUGUI sizePlayer;

    [Header("Inspection Data")]
    [SerializeField] private GameObject contentBox;
    [SerializeField] private GameObject inspectionBox;
    private bool isInspectionOpen = false;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI qaulity;

    private int machineID = 0;

    private void Awake()
    {
        craftMachineInventory = new Inventory();
        playerInventory = new Inventory();
    }

    private void OpenUI(bool newBool)
    {
        craftMenu.SetActive(newBool);
        UpdatePlayerInventory();
        UpdateCraftMachine(); 
        foreach (GameObject objecty in objectsToHide)
        {
            objecty.SetActive(false);
        }
    }

    public void OnExit()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        if(isInspectionOpen)
        {
            OnCloseInspection();
        }
        if (craftMachineInventory.GetInventory().Count > 0)
        {
            foreach (InventorySlot itemSlot in craftMachineInventory.GetInventory())
            {
                if(craftMachineInventory.GetInventory().Count == 3)
                {
                    if (itemSlot != craftMachineInventory.GetInventory()[2])
                    {
                        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(itemSlot.Item, itemSlot.Amount, itemSlot.Quality);
                    }
                }
                else
                {
                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(itemSlot.Item, itemSlot.Amount, itemSlot.Quality);
                }
            }
        }
        craftMachineInventory.GetInventory().Clear();
        UpdateCraftMachine();

        foreach (Transform item in playerContainer)
        {
            item.GetComponent<SCR_PlayerCraftMachineSlot>().OnCloseSplit();
        }

        craftMenu.SetActive(false);
        CustomEvents.TimeCycle.OnUnpause?.Invoke(); 
        foreach (GameObject objecty in objectsToHide)
        {
            objecty.SetActive(true);
        }
    }

    //Craft Machine Stuff
    private void SetCraftMachineInventory(Inventory newInventory, int id)
    {
        craftMachineInventory = newInventory;
        machineID = id;
    }

    private void UpdateCraftMachine()
    {
        foreach (GameObject slot in craftingSlots)
        {
            foreach (Transform item in slot.transform)
            {
                Destroy(item.gameObject);
            }
        }

        if (craftMachineInventory.GetInventory().Count != 0)
        {
            for (int i = 0; i < craftMachineInventory.GetInventory().Count; i++)
            {
                GameObject currentSlot = Instantiate(craftingSlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<SCR_CraftMachineSlot>().SetInventorySlot(craftMachineInventory.GetInventory()[i], machineID);
                currentSlot.GetComponent<RectTransform>().SetParent(craftingSlots[i].transform);
                currentSlot.GetComponent<RectTransform>().position = craftingSlots[i].GetComponent<RectTransform>().position;
            }
        }

        if (craftMachineInventory.GetInventory().Count != 3)
        {
            for (int i = craftMachineInventory.GetInventory().Count; i < 3; i++)
            {
                GameObject currentSlot = Instantiate(blankSlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<RectTransform>().SetParent(craftingSlots[i].transform);
                currentSlot.GetComponent<RectTransform>().position = craftingSlots[i].GetComponent<RectTransform>().position;
            }
        }
    }

    //Player Inventory Stuff
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

        if (playerInventory != null)
        {
            foreach (InventorySlot slot in playerInventory.GetInventory())
            {
                GameObject currentSlot = Instantiate(uiInventorySlot, transform.position, Quaternion.identity, gameObject.transform);
                currentSlot.GetComponent<SCR_PlayerCraftMachineSlot>().SetInventorySlot(slot, machineID);

                currentSlot.GetComponent<RectTransform>().SetParent(playerContainer.gameObject.GetComponent<RectTransform>());
                currentSlot.GetComponent<RectTransform>().localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
        }
        int total = 30;
        total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        sizePlayer.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }

    public void OnOpenInspection(InventorySlot inventorySlot, bool openInspection, int craftingID)
    {
        if (craftingID == machineID)
        {
            if (!isInspectionOpen && openInspection)
            {
                contentBox.GetComponent<RectTransform>().offsetMax -= new Vector2(350, 0);
                GetComponentInChildren<GridLayoutGroup>().constraintCount = 5;
                inspectionBox.SetActive(true);
                isInspectionOpen = true;
            }
            icon.GetComponent<Image>().sprite = inventorySlot.Item.ItemSprite;
            itemName.text = "Name: " + inventorySlot.Name;
            amount.text = "Amount in slot: " + inventorySlot.Amount.ToString();
            qaulity.text = "Quality: " + inventorySlot.Quality.ToString();
        }
    }

    public void OnCloseInspection()
    {
        contentBox.GetComponent<RectTransform>().offsetMax -= new Vector2(-350, 0);
        GetComponentInChildren<GridLayoutGroup>().constraintCount = 7;
        inspectionBox.SetActive(false);
        isInspectionOpen = false;
    }


    private void OnUpdateInventorySize()
    {
        int total = (int)CustomEvents.InventorySystem.PlayerInventory.OnGetInventorySize?.Invoke();
        sizePlayer.text = string.Format("Size: {0}/{1}", playerInventory.GetInventory().Count, total);
    }


    void OnEnable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData += SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI += UpdatePlayerInventory;

        CustomEvents.CraftMachine.OnSetInventory += SetCraftMachineInventory;
        CustomEvents.CraftMachine.OnUpdateUI += UpdateCraftMachine;
        CustomEvents.CraftMachine.OnToggleUI += OpenUI;

        CustomEvents.CraftMachine.OnOpenInspection += OnOpenInspection;

        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize += OnUpdateInventorySize;
    }

    void OnDisable()
    {
        CustomEvents.InventorySystem.PlayerInventory.OnGetInventoryData -= SetInventory;
        CustomEvents.InventorySystem.PlayerInventory.OnUpdateUI -= UpdatePlayerInventory;

        CustomEvents.CraftMachine.OnSetInventory -= SetCraftMachineInventory;
        CustomEvents.CraftMachine.OnUpdateUI -= UpdateCraftMachine;
        CustomEvents.CraftMachine.OnToggleUI -= OpenUI;

        CustomEvents.CraftMachine.OnOpenInspection -= OnOpenInspection;

        CustomEvents.InventorySystem.PlayerInventory.OnUpdateInventorySize -= OnUpdateInventorySize;
    }
}
