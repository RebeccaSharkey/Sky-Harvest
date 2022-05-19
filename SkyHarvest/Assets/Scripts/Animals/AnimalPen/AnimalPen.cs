using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

/// <summary>
/// Base animal pen script all Animal Pens use
/// Spawns an animal based off of the animal type passed in
/// Can buy animals depending on the animal type enum
/// </summary>
public class AnimalPen : MonoBehaviour, iSaveable
{
    public AnimalType animalType;
    private GameObject animal;
    [SerializeField] private string animalPath;

    public int buyValue;
    public AnimalBehaviour animalScript;

    [SerializeField] private Vector3 offset;

    [SerializeField] private List<GameObject> animalList;
    
    private int currentAmount = 1;
    private int maxAmount = 5;

    [SerializeField] GameObject[] childObjects;
    private bool bSpawnedIn = false;

    private void Start()
    {
        /*if (!bSpawnedIn)
        {
            animalList = new List<GameObject>();
            bSpawnedIn = true;
        }*/
    }

    /// <summary>
    /// Spawns a certain type of animal depending on the animal type enum
    /// Grabs the correct prefab from the asset directory rather than by serialized reference
    /// </summary>
    public void SpawnAnimal()
    {
        switch (animalType)
        {
            case AnimalType.Chicken:
                BuyAnimal(animal = Resources.Load<GameObject>("Prefabs/Animals/Chicken Prefab"), animalScript = animal.GetComponentInChildren<AnimalBehaviour>(), transform.position);
                break;
            case AnimalType.Cow:
                BuyAnimal(animal = Resources.Load<GameObject>("Prefabs/Animals/Cow Prefab"), animalScript = animal.GetComponentInChildren<AnimalBehaviour>(), transform.position);
                break;
            case AnimalType.Pig:
                BuyAnimal(animal = Resources.Load<GameObject>("Prefabs/Animals/Pig Prefab"), animalScript = animal.GetComponentInChildren<AnimalBehaviour>(), transform.position);
                break;
            case AnimalType.Horse:
                BuyAnimal(animal = Resources.Load<GameObject>("Prefabs/Animals/Horse Prefab"), animalScript = animal.GetComponentInChildren<AnimalBehaviour>(), transform.position);
                break;
            case AnimalType.Sheep:
                BuyAnimal(animal = Resources.Load<GameObject>("Prefabs/Animals/Sheep Prefab"), animalScript = animal.GetComponentInChildren<AnimalBehaviour>(), transform.position);
                break;
            default:
                Debug.LogError("Unhandled Animal Type");
                break;
        }
    }

    /// <summary>
    /// Function that spawns an animal in once the player has "bought" it by checking the current currency
    /// Keeps track of the amount of animals spawned in as to not go over a limit
    /// </summary>
    /// <param name="animal"> Passes in the animal object itself that has been grabbed</param>
    /// <param name="animalScript"> Passes in the animal script attached to the animal to grab its buy value</param>
    /// <param name="pos"> Location passed in for its spawn point</param>
    private void BuyAnimal(GameObject animal, AnimalBehaviour _animalScript, Vector3 pos)
    {
        if (CustomEvents.Currency.OnCheckCurrency?.Invoke(_animalScript.BuyValue) == true)
        {
            if (currentAmount < maxAmount)
            {
                buyValue = _animalScript.BuyValue;
                CustomEvents.Currency.OnRemoveCurrency?.Invoke(_animalScript.BuyValue);
                GameObject temp = Instantiate(animal, pos + offset, Quaternion.identity);
                animalList.Add((temp));
                currentAmount++;
                CustomEvents.AnimalPens.OnSetNewID?.Invoke();
            }
            else
            {
                Debug.Log("Max capacity of animal reached");
            }
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void AddAnimalToList(GameObject _animal)
    {
        Debug.Log("Called");
        animalList.Add(_animal);
    }

    public void SetHasBoughtPen(bool _state)
    {
        bSpawnedIn = _state;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        
        //0
        data.Add(bSpawnedIn.ToString());
        //1
        data.Add(currentAmount.ToString());
        //2
        if(bSpawnedIn)
        {
            foreach (GameObject animal in animalList)
            {
                //Adds 3

                Debug.Log(animal.transform.localPosition);
                float x = animal.transform.localPosition.x;
                float y = animal.transform.localPosition.y;
                float z = animal.transform.localPosition.z;
                data.Add(x.ToString());
                data.Add(y.ToString());
                data.Add(z.ToString());

                //Adds 4
                data.Add(animal.GetComponentInChildren<AnimalBehaviour>().GetIsFed().ToString());
                data.Add(animal.GetComponentInChildren<AnimalBehaviour>().GetIsHarvestable().ToString());
                data.Add(animal.GetComponentInChildren<AnimalBehaviour>().GetCanBePet().ToString());

                data.Add(animal.GetComponentInChildren<AnimalBehaviour>().GetHappinessLevel().ToString());
            }
        }

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        if (_data.Count < 0)
        {
            return;
        }
        
        int increment = 0;
        bSpawnedIn = bool.Parse(_data[0]);
        currentAmount = int.Parse(_data[1]);
        if (bSpawnedIn)
        {
            foreach (GameObject child in childObjects)
            {
                child.SetActive(true);
            }
        }

        if(bSpawnedIn)
        {
            animal = Resources.Load<GameObject>(animalPath);
            for (int i = 0; i < currentAmount; i++)
            //foreach(GameObject animal in animalList)
            {
                //Data elements STARTS at 4
                /*Debug.Log(_data[increment + 3]);
                Debug.Log(_data[increment + 4]);
                Debug.Log(_data[increment + 5]);*/
                GameObject temp = Instantiate(animal, new Vector3(float.Parse(_data[increment + 2]), float.Parse(_data[increment + 3]), float.Parse(_data[increment + 4])),
                    Quaternion.identity);

                animalList.Add(temp);
                //1 LOOP: 7,8,9,10 (i + increment = 3)
                //2 LOOP: 11,12,13,14 (i + increment = 4)
                temp.GetComponentInChildren<AnimalBehaviour>().SetIsFed(bool.Parse(_data[increment + 5]));
                temp.GetComponentInChildren<AnimalBehaviour>().SetIsHarvestable(bool.Parse(_data[increment + 6]));
                temp.GetComponentInChildren<AnimalBehaviour>().SetCanBePet(bool.Parse(_data[increment + 7]));
                temp.GetComponentInChildren<AnimalBehaviour>().SetHappinessLevel(int.Parse(_data[increment + 8]));
                increment += 7;
            }

            Debug.Log(animalList.Count);
        }
    }
}
