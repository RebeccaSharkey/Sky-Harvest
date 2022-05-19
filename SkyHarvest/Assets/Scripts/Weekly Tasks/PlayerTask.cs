using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerTask 
{
    public string name;

    private SCR_Items item;
    public SCR_Items Item { get => item; }

    private int amount;
    public int Amount { get => amount; }

    private int reward;
    public int Reward { get => reward; }

    private bool requireQuality;
    public bool RequireQuality { get => requireQuality; }

    private ItemQuality quality;
    public ItemQuality Quality { get => quality; }

    public PlayerTask()
    {
        item = null;

        amount = 0;

        reward = 0;

        requireQuality = true;

        quality = ItemQuality.Normal;

        name = string.Format("null task");
    }

    public PlayerTask(SCR_Items _item, int _amount, int _reward, bool _requireQuality, ItemQuality _itemQuality)
    {
        item = _item;

        amount = _amount;

        reward = _reward;

        requireQuality = _requireQuality;

        quality = _itemQuality;

        if (requireQuality)
        {
            name = string.Format("Get {0} {1} {2}(s)\nReward: {3} frozen raindrops.", amount.ToString(), quality.ToString(), item.name, reward.ToString());
        }
        else
        {
            name = string.Format("Get {0} {1}(s)\nReward: {2} frozen raindrops.", amount.ToString(), item.name, reward.ToString());
        }
    }

    public void LoadItems(List<SCR_Items> availableCrops)
    {
        ItemManager itemManager = Resources.Load<ItemManager>("Items/Item Manager");
        if (itemManager == null)
        {
            Debug.LogWarning("Item Manager not loaded from resources");
        }
        else
        {
            int probability = (int)Random.Range(0f, 5f);
            switch (probability)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    item = availableCrops[(int)Random.Range(0f, availableCrops.Count)];
                    break;
                case 4:
                case 5:
                    requireQuality = false;
                    item = itemManager.allFishItems[(int)Random.Range(0f, itemManager.allFishItems.Count)];
                    break;
            }
        }

        amount = (int)Random.Range(5f, 20f);

        quality = (ItemQuality)Random.Range(0, 4);

        switch(quality)
        {
            case ItemQuality.Bad:
                reward = (item.SellValueBad * amount) + (int)((item.SellValueBad * amount) * 0.5f);
                break;
            case ItemQuality.Normal:
                reward = (item.SellValueNormal * amount) + (int)((item.SellValueNormal * amount) * 0.5f);
                break;
            case ItemQuality.Good:
                reward = (item.SellValueGood * amount) + (int)((item.SellValueGood * amount) * 0.5f);
                break;
            case ItemQuality.Perfect:
                reward = (item.SellValuePerfect * amount) + (int)((item.SellValuePerfect * amount) * 0.5f);
                break;
        }

        if(requireQuality)
        {
            name = string.Format("Get {0} {1} {2}(s)\nReward: {3} frozen raindrops.", amount.ToString(), quality.ToString(), item.name, reward.ToString());
        }
        else
        {
            name = string.Format("Get {0} {1}(s)\nReward: {2} frozen raindrops.", amount.ToString(), item.name, reward.ToString());
        }
    }
}
