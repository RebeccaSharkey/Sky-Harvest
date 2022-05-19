using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Base script for all animal pen posts that allow for buying the pens
/// Spawns an animal pen based off the animal pen field
/// </summary>
public class AnimalPenBuying : MonoBehaviour, iSaveable
{
    [SerializeField] private int penCost;
    [SerializeField] private int penID;
    [SerializeField] private GameObject animalPenToSpawn;
    [SerializeField] private GameObject animalToSpawn;

    [SerializeField] private Transform penSpawnLocation;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 rotationOffset;
    private AnimalType animalType;

    [SerializeField] private string penDescription;
    [SerializeField] private string penName;
    private GameObject spawnParticleFX;

    [SerializeField] private GameObject[] childObjects;
    public bool bHasBeenBought;

    private void Start()
    {
        spawnParticleFX = Resources.Load<GameObject>("Prefabs/Particle Effects/SmokeExplosionFX");
    }
    /// <summary>
    /// Function that spawns in an animal pen after the player has "bought" it
    /// Checks the currency against the animal pen cost in order to buy it
    /// Compares an ID in order to use the same UI for different pens
    /// </summary>
    /// <param name="_penID">Passes in an ID to allow for pens to use the same UI</param>
    private void BuyAnimalPen(int _penID)
    {
        if (penID == _penID)
        {
            if (CustomEvents.Currency.OnCheckCurrency?.Invoke(penCost) == true)
            {
                CustomEvents.Currency.OnRemoveCurrency?.Invoke(penCost);
                //Instantiate(animalPenToSpawn, penSpawnLocation.position + spawnOffset, Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z));
                animalPenToSpawn.SetActive(true);
                animalPenToSpawn.GetComponentInParent<BoxCollider>().enabled = true;
                GameObject animalGO = Instantiate(animalToSpawn, transform.position, Quaternion.identity);
                animalType = animalGO.GetComponentInChildren<AnimalBehaviour>().AnimalData.animalType;
                AnimalPen animalPen = animalPenToSpawn.GetComponentInParent<AnimalPen>(); // :))))))
                if (animalPen != null)
                {
                    animalPen.animalType = animalType;
                    animalPen.AddAnimalToList(animalGO);
                    animalPen.SetHasBoughtPen(true);
                }
                else
                {
                    Debug.Log("Animal Pen == Null");
                }
                CustomEvents.AnimalPens.OnToggleUI?.Invoke(false);
                GameObject particleFX = Instantiate(spawnParticleFX, transform.position, Quaternion.identity);
                particleFX.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                Destroy(particleFX, 1f);
                bHasBeenBought = true;
                foreach (GameObject child in childObjects)
                {
                    child.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Not Enough Money");
            }
        }
    }

    /// <summary>
    /// Sets up the UI for the pen that will be bought after comparing the ID
    /// Sets Text fields via the events system to save on having to reference other scripts
    /// </summary>
    public void OpenPenUI()
    {
        CustomEvents.AnimalPens.OnSetUI?.Invoke(penName, penDescription, penCost, penID);
        CustomEvents.AnimalPens.OnToggleUI?.Invoke(true);
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        data.Add(bHasBeenBought.ToString());
        return data;
    }

    public void LoadData(SerializableList _data)
    {
        if (_data.Count <= 0)
        {
            return;
        }

        bHasBeenBought = bool.Parse(_data[0]);
        //_data.Remove(_data[0]);
        if (bHasBeenBought)
        {
            foreach (GameObject child in childObjects)
            {
                child.SetActive(false);
            }
            animalPenToSpawn.SetActive(true);
        }

    }

    /// <summary>
    /// Subscribes and unsubscribes from the event systems depending on the objects state
    /// </summary>
    private void OnEnable()
    {
        CustomEvents.AnimalPens.OnBuyAnimalPen += BuyAnimalPen;
    }

    private void OnDisable()
    {
        CustomEvents.AnimalPens.OnBuyAnimalPen -= BuyAnimalPen;
    }
}
