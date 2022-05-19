using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core game manager
/// </summary>
public class GameManager : MonoBehaviour, iSaveable
{
    [SerializeField] private FrozenTears currencySO;

    [Header("Debug")]
    [SerializeField] private bool isSavingActive = false;
    [SerializeField] private bool isStartingFromFarm = false;

    [SerializeField] private ItemManager itemManager;
    private Dictionary<SCR_Items, int> completionItems = new Dictionary<SCR_Items, int>();

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnDayEnd += NewDaySetup;
        CustomEvents.Tutorial.OnStartTutorial += SetNewGame;
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack += OnAddNewItem;
        CustomEvents.InventorySystem.PlayerInventory.OnAddToItemStack += OnAddNewItem;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnDayEnd -= NewDaySetup;
        CustomEvents.Tutorial.OnStartTutorial -= SetNewGame;
        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack -= OnAddNewItem;
        CustomEvents.InventorySystem.PlayerInventory.OnAddToItemStack -= OnAddNewItem;
    }

    private void Awake()
    {
        SaveLoadManager.instance.Load();
    }

    private void Start()
    {
        GameLoaded();
        if(isStartingFromFarm)
        {
            SetNewGame();
        }
    }

    /// <summary>
    /// Starts new day when game is loaded
    /// </summary>
    private void GameLoaded()
    {
        CustomEvents.UI.OnRandomFadeIn?.Invoke();
        CustomEvents.TimeCycle.OnDayStart?.Invoke();
        CustomEvents.TimeCycle.OnUnpause?.Invoke();
    }

    private void NewDaySetup()
    {
        StartCoroutine(NewDaySetupRoutine());
    }

    /// <summary>
    /// Handles setup for new days
    /// </summary>
    /// <returns>Ienumerator</returns>
    private IEnumerator NewDaySetupRoutine()
    {
        CustomEvents.TimeCycle.OnNewDaySetup?.Invoke();

        yield return null;

        if (isSavingActive)
        {
            Debug.Log("Saving...");
            //save game
            SaveLoadManager.instance.Save();
        }

        CustomEvents.TimeCycle.OnReadyForDayStart?.Invoke();

    }

    private void SetNewGame()
    {
        completionItems = new Dictionary<SCR_Items, int>();
        foreach (SCR_Items item in itemManager.allCropItems)
        {
            if (!completionItems.ContainsKey(item))
            {
                completionItems.Add(item, 0);
            }
        }
        foreach (SCR_Items item in itemManager.allFishItems)
        {
            if (!completionItems.ContainsKey(item))
            {
                completionItems.Add(item, 0);
            }
        }
    }

    private void OnAddNewItem(SCR_Items newitem, int amount, ItemQuality quality)
    {
        if (completionItems.ContainsKey(newitem))
        {
            completionItems[newitem] += amount;
            if (!completionItems.ContainsValue(0))
            {
                CustomEvents.SceneManagement.OnLoadNewScene?.Invoke("EndCutscene");
            }
        }
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        foreach (KeyValuePair<SCR_Items, int> item in completionItems)
        {
            data.Add($"{item.Key.name}#{item.Value.ToString()}");
        }
        return data;
    }

    public void LoadData(SerializableList _data)
    {
        completionItems.Clear();

        if (_data.Count <= 0)
        {
            Debug.LogWarning("Completion items not loaded properly! Try and start a new game for a proper save of Fish");
        }

        foreach (string item in _data)
        {
            if (string.IsNullOrEmpty(item)) return;

            string[] kayPairInfo = item.Split('#');

            SCR_Items thisItem = Resources.Load<SCR_Items>($"Items/Fish/{kayPairInfo[0].ToString()}");
            if (thisItem == null) thisItem = Resources.Load<SCR_Items>($"Items/Produce/{kayPairInfo[0].ToString()}");
            if (thisItem == null)
            {
                Debug.LogError($"Null SO object for {kayPairInfo[0]}");
                continue;
            }
            completionItems.Add(thisItem, int.Parse(kayPairInfo[1]));
        }
    }
}
