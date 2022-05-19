using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Objects which holds data that every animal will use
/// </summary>
public enum AnimalType { Cow, Chicken, Pig, Sheep, Horse };

[CreateAssetMenu(fileName = "Animals", menuName = "AI/Animal")]

public class AnimalSO : ScriptableObject
{
    private string[] defaultNames = new string[]
    {
        "Bella", "Poppy", "Daisy", "Rosie", "Willow", "Max", "Charlie", "Bear", "Lee", "Ralph"
    };
    public string animalName;

    public string SetRandomName()
    {
        animalName = defaultNames[Random.Range(0, defaultNames.Length)];
        return animalName;
    }

    public int minProduceReturned = 0;
    public int maxProduceReturned = 1;

    public int buyValue;

    public int happinessValue = 10;

    public AnimalType animalType;

    public SCR_Items itemToFeed;
    
    [Header("Produce Harvesting")]
    public SCR_Items[] produce;
}
