using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement")]
[System.Serializable]
public class Achievement : ScriptableObject
{
    public string achievementName;
    public string achievementDescription;
    public int numGoal;
    
    public int currencyReward;
    //public GameObject trophyObject;
    public Sprite achievementLogo;
    public Sprite lockedLogo;
    public bool bIsComplete = false;
}
