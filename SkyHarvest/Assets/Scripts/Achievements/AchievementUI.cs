using System;
using System.Collections;
using System.Collections.Generic;
//using MiscUtil.Collections.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private Achievement ach;
    [SerializeField] private Image _icon;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _reward;
    [SerializeField] private TextMeshProUGUI _status;
    [SerializeField] private TextMeshProUGUI _amount;

    private int currentAmount;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _name.text = ach.achievementName;
        _description.text = ach.achievementDescription;
        _reward.text = ach.currencyReward.ToString();
        if (ach.bIsComplete)
        {
            _status.text = "Status: Complete";
            _icon.sprite = ach.achievementLogo;
        }
        else
        {
            _status.text = "Status: In-Progress";
            _icon.sprite = ach.lockedLogo;
        }
        
        switch (ach.achievementName)
        {
            case "Plant Lover":
                currentAmount = (int)CustomEvents.Achievements.OnGetTotalCropsPlanted?.Invoke();
                Debug.Log("Called");
                break;
            case "Good Yield":
                currentAmount = (int) CustomEvents.Achievements.OnGetTotalCropsFertilised?.Invoke();
                break;
            case "Animal Lover":
                currentAmount = (int) CustomEvents.Achievements.OnGetTotalAnimalsPet?.Invoke();
                break;
            case "Master Of The Sea":
                currentAmount = (int) CustomEvents.Achievements.OnGetTotalFishCaught?.Invoke();
                break;
            case "Levelling Up":
                currentAmount = (int) CustomEvents.Achievements.OnGetTotalUpgradesBought?.Invoke();
                break;
            case "Brrrrr":
                currentAmount = (int) CustomEvents.Achievements.OnGetSnowyIslandsBought?.Invoke();
                break;
        }
        _amount.text = currentAmount.ToString() + " / " + ach.numGoal;
    }
}
