using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour, iSaveable
{
    [SerializeField] private Achievement[] achievements;
    [SerializeField] private GameObject[] allAchievementUI;

    [SerializeField] private GameObject achievementUI;
    [SerializeField] private TextMeshProUGUI completionText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Image trophyLogo;

    [SerializeField] private float uiWaitTime;
    
    private int totalCropsPlanted;
    private int totalCropsFertilised;
    private int totalFishCaught;
    
    private int totalUpgradesBought;

    private int totalAnimalsPet;

    private int totalSnowyIslandBought;

    private int totalMoneyGot = 0;

    /*private void Start()
    {
        foreach (Achievement ach in achievements)
        {
            ach.bIsComplete = false;
        }
    }*/

    //Plant x amount of crops
    private void AddToTotalCropsPlanted(int _amount)
    {
        string tempName = "Plant Lover";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalCropsPlanted += _amount;
                    if (totalCropsPlanted >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }

    private int GetTotalCrops()
    {
        return totalCropsPlanted;
    }

    //Fertilise x amount of crops
    private void AddToTotalCropsFertilised(int _amount)
    {
        string tempName = "Good Yield";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalCropsFertilised += _amount;
                    if (totalCropsFertilised >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }

    private int GetTotalFertilised()
    {
        return totalCropsFertilised;
    }

    //Catch x amount of fish
    private void AddToTotalFishCaught(int _amount)
    {
        string tempName = "Master Of The Sea";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalFishCaught += _amount;
                    if (totalFishCaught >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }

    private int GetTotalFish()
    {
        return totalFishCaught;
    }

    //Buy x amount of upgrades
    private void AddToTotalUpgradesBought(int _amount)
    {
        string tempName = "Levelling Up";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalUpgradesBought += _amount;
                    if (totalUpgradesBought >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }
    
    private int GetTotalUpgrades()
    {
        return totalUpgradesBought;
    }

    //Pet animals x amount
    private void AddToTotalAnimalsPet(int _amount)
    {
        string tempName = "Animal Lover";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalAnimalsPet += _amount;
                    if (totalAnimalsPet >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }

    private int GetTotalPets()
    {
        return totalAnimalsPet;
    }
    
    private void AddToTotalSnowyIslands(int _amount)
    {
        string tempName = "Brrrrr";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalSnowyIslandBought += _amount;
                    if (totalSnowyIslandBought >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }

    private int GetSnowyIslandsBought()
    {
        return totalSnowyIslandBought;
    }

    private void AddToTotalMoneyGot(int _amount)
    {
        string tempName = "Money Hoarder";
        foreach (Achievement ach in achievements)
        {
            if (tempName == ach.achievementName)
            {
                if (!ach.bIsComplete)
                {
                    totalMoneyGot += _amount;
                    if (totalMoneyGot >= ach.numGoal)
                    {
                        CompleteAchievement(tempName);
                    }
                }
            }
        }
    }

    //Compares the name passed in with all achievements and completes the achievement
    private void CompleteAchievement(string _name)
    {
        foreach (Achievement ach in achievements)
        {
            if (_name == ach.achievementName)
            {
                ach.bIsComplete = true;
                CustomEvents.Currency.OnAddCurrency?.Invoke(ach.currencyReward);
                StartCoroutine(ShowAchievementCompletionUI(_name));
            }
        }
    }

    //Shows the achievement UI when completed
    private IEnumerator ShowAchievementCompletionUI(string _name)
    {
        SetCompletedUI(true, _name);
        achievementUI.SetActive(true);
        CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Achievement Unlocked");
        yield return new WaitForSeconds(uiWaitTime);
        SetCompletedUI(false, _name);
        achievementUI.SetActive(false);
    }

    //Sets the UI up to show data for the various achievements
    private void SetCompletedUI(bool state, string _name)
    {
        if (state)
        {
            foreach (Achievement ach in achievements)
            {
                if (_name == ach.achievementName)
                {
                    completionText.text = ach.achievementName + " Completed!";
                    rewardText.text = "Reward: " + ach.currencyReward.ToString("0");
                    trophyLogo.sprite = ach.achievementLogo;
                }
            }
        }
        else
        {
            completionText.text = "";
            rewardText.text = "";
            trophyLogo.sprite = null;
        }
    }

    public void UpdateAchievmentUI(bool _state)
    {
        if (_state)
        {
            foreach (GameObject ach in allAchievementUI)
            {
                ach.GetComponent<AchievementUI>().UpdateUI();
                CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
                CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
            }
            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(false);
        }
        else
        {
            CustomEvents.TimeCycle.OnPauseTimeOnly?.Invoke(true);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
        }
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        data.Add(totalCropsPlanted.ToString());
        data.Add(totalCropsFertilised.ToString());
        data.Add(totalFishCaught.ToString());
        data.Add(totalUpgradesBought.ToString());
        data.Add(totalSnowyIslandBought.ToString());
        data.Add(totalAnimalsPet.ToString());
        data.Add(totalMoneyGot.ToString());
        
        foreach (Achievement ach in achievements)
        {
            data.Add(ach.bIsComplete.ToString());
        }

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        totalCropsPlanted = int.Parse(_data[0]);
        totalCropsFertilised = int.Parse(_data[1]);
        totalFishCaught = int.Parse(_data[2]);
        totalUpgradesBought = int.Parse(_data[3]);
        totalSnowyIslandBought = int.Parse(_data[4]);
        totalAnimalsPet = int.Parse(_data[5]);
        totalMoneyGot = int.Parse(_data[6]);

        int increment = 7;
        for (int i = 0; i < achievements.Length; i++)
        {
            achievements[i].bIsComplete = bool.Parse(_data[increment + i]);
        }
        
    }
    private void OnEnable()
    {
        CustomEvents.Achievements.OnAddToTotalCropsPlanted += AddToTotalCropsPlanted;
        CustomEvents.Achievements.OnAddToTotalCropsFertilised += AddToTotalCropsFertilised;
        CustomEvents.Achievements.OnAddToTotalFishCaught += AddToTotalFishCaught;
        CustomEvents.Achievements.OnAddToTotalAnimalsPet += AddToTotalAnimalsPet;
        CustomEvents.Achievements.OnAddToTotalUpgradesBought += AddToTotalUpgradesBought;
        CustomEvents.Achievements.OnAddToSnowyIslandsBought += AddToTotalSnowyIslands;
        CustomEvents.Achievements.OnAddToTotalMoneyGot += AddToTotalMoneyGot;

        CustomEvents.Achievements.OnGetTotalCropsPlanted += GetTotalCrops;
        CustomEvents.Achievements.OnGetTotalCropsFertilised += GetTotalFertilised;
        CustomEvents.Achievements.OnGetTotalFishCaught += GetTotalFish;
        CustomEvents.Achievements.OnGetTotalAnimalsPet += GetTotalPets;
        CustomEvents.Achievements.OnGetTotalUpgradesBought += GetTotalUpgrades;
        CustomEvents.Achievements.OnGetSnowyIslandsBought += GetSnowyIslandsBought;
    }

    private void OnDisable()
    {
        CustomEvents.Achievements.OnAddToTotalCropsPlanted -= AddToTotalCropsPlanted;
        CustomEvents.Achievements.OnAddToTotalCropsFertilised -= AddToTotalCropsFertilised;
        CustomEvents.Achievements.OnAddToTotalFishCaught -= AddToTotalFishCaught;
        CustomEvents.Achievements.OnAddToTotalAnimalsPet -= AddToTotalAnimalsPet;
        CustomEvents.Achievements.OnAddToTotalUpgradesBought -= AddToTotalUpgradesBought;
        CustomEvents.Achievements.OnAddToSnowyIslandsBought -= AddToTotalSnowyIslands;
        CustomEvents.Achievements.OnAddToTotalMoneyGot -= AddToTotalMoneyGot;

        CustomEvents.Achievements.OnGetTotalCropsPlanted -= GetTotalCrops;
        CustomEvents.Achievements.OnGetTotalCropsFertilised -= GetTotalFertilised;
        CustomEvents.Achievements.OnGetTotalFishCaught -= GetTotalFish;
        CustomEvents.Achievements.OnGetTotalAnimalsPet -= GetTotalPets;
        CustomEvents.Achievements.OnGetTotalUpgradesBought -= GetTotalUpgrades;
        CustomEvents.Achievements.OnGetSnowyIslandsBought -= GetSnowyIslandsBought;
    }
}
