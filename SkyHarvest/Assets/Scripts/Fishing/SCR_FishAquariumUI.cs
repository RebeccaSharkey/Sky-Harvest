using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_FishAquariumUI : MonoBehaviour, iSaveable
{
    [Header("UI Data")]
    [SerializeField] private GameObject ui;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private GameObject rightOption;
    [SerializeField] private GameObject leftOption;
    private int currentPage;

    [Header("Fish Data")]
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private Image fishIcon;
    [SerializeField] private TextMeshProUGUI descFishName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI lore;

    private Dictionary<SO_Fish, int> fishList = new Dictionary<SO_Fish, int>();

    private void NewGame()
    {
        foreach (SCR_Items fish in itemManager.allFishItems)
        {
            if (!fishList.ContainsKey((SO_Fish)fish))
            {
                fishList.Add((SO_Fish)fish, 0);
            }
        }
    }

    private void ToggleUI(bool newBool)
    {
        if(newBool)
        {
            ui.SetActive(true);
            Open();
        }
        else
        {
            ui.SetActive(false);
            CustomEvents.TimeCycle.OnUnpause?.Invoke();
        }
    }

    public void Close()
    {
        ToggleUI(false);
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
    }

    public void Open()
    {
        currentPage = 0;
        OnForward();
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
    }

    public void OnForward()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        currentPage++;
        TurnPage();
        if (currentPage >= itemManager.allFishItems.Count)
        {
            rightOption.SetActive(false);
        }
        else
        {
            rightOption.SetActive(true);
        }

        if (currentPage == 1)
        {
            leftOption.SetActive(false);
        }
        else
        {
            leftOption.SetActive(true);
        }
    }

    public void OnBackwards()
    {
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Menu Button");
        currentPage--;
        TurnPage();
        if (currentPage >= itemManager.allFishItems.Count)
        {
            rightOption.SetActive(false);
        }
        else
        {
            rightOption.SetActive(true);
        }

        if (currentPage == 2)
        {
            leftOption.SetActive(false);
        }
        else
        {
            leftOption.SetActive(true);
        }
    }

    private void TurnPage()
    {
        SO_Fish currentItem = (SO_Fish)itemManager.allFishItems[currentPage - 1];
        if (fishList[currentItem] > 0)
        {
            fishName.text = currentItem.ItemName;
            fishIcon.sprite = currentItem.ItemSprite;
            fishIcon.color = Color.white;
            descFishName.text = currentItem.ItemName;
            amount.text = "Amount Caught: " + fishList[currentItem];
            lore.text = "Lore:\n" + currentItem.Lore;
        }
        else
        {
            fishName.text = "???";
            fishIcon.sprite = currentItem.ItemSprite;
            fishIcon.color = Color.black;
            descFishName.text = "???";
            amount.text = "Amount Caught: " + fishList[currentItem];
            lore.text = "Lore:\n???";
        }
    }

    private void IncrimentAmount(SO_Fish fish)
    {
        fishList[fish]++;
    }

    private void OnEnable()
    {
        CustomEvents.Fishing.OnIncrimentAmountCaught += IncrimentAmount;
        CustomEvents.Fishing.OnToggleAquarium += ToggleUI;
        CustomEvents.Tutorial.OnStartTutorial += NewGame;
    }
    private void OnDisable()
    {
        CustomEvents.Fishing.OnIncrimentAmountCaught -= IncrimentAmount;
        CustomEvents.Fishing.OnToggleAquarium -= ToggleUI;
        CustomEvents.Tutorial.OnStartTutorial -= NewGame;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        foreach(KeyValuePair<SO_Fish, int> fishItem in fishList)
        {
            data.Add($"{fishItem.Key.name}.{fishItem.Value.ToString()}");
        }
        return data;
    }

    public void LoadData(SerializableList _data)
    {
        fishList.Clear();

        if (_data.Count <= 0)
        {
            Debug.LogWarning("Fishes not loaded properly! Try and start a new game for a proper save of Fish");
        }

        foreach(string item in _data)
        {
            if (string.IsNullOrEmpty(item)) return;

            string[] kayPairInfo = item.Split('.');

            SO_Fish fishItem = Resources.Load<SO_Fish>($"Items/Fish/{kayPairInfo[0].ToString()}");

            if(fishItem != null)
            {
                fishList.Add(fishItem, int.Parse(kayPairInfo[1]));
            }
        }
    }
}
