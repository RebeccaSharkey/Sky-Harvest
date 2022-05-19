using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base script for all animal behaviours allowing for basic interaction with the player
/// </summary>
public class AnimalBehaviour : MonoBehaviour
{
    [SerializeField] private AnimalSO animalData;
    public AnimalSO AnimalData { get => animalData; set => animalData = value; }
    [SerializeField] private GameObject animalDataSign;
    [SerializeField] private GameObject horseSpeedSign;
    [SerializeField] private Vector3 offset;

    [SerializeField] private bool bIsFed = false;
    [SerializeField] private bool bIsHarvestable = false;
    [SerializeField] private bool bCanBePet = true;
    private int produceAmount = 0;

    private string animalName;

    private int _buyValue;
    public int BuyValue { get => animalData.buyValue; set => _buyValue = value; }

    private int happinessLevel = 0;
    private int currentHappinessLevel;

    private AnimalType type;

    [SerializeField] private int petHappinessAmount;

    [SerializeField] private int billboardID;
    
    [SerializeField] private Vector3 particleOffset;
    private GameObject petParticles;

    private GameObject player;
    private float speedIncreaser = 0.5f;

    private AnimalBillboard billboardScript;

    private void Start()
    {
        GameObject harvestSignGO = Instantiate(animalDataSign, transform.position + offset, Quaternion.identity);
        billboardScript = harvestSignGO.GetComponent<AnimalBillboard>();
        type = animalData.animalType;
        harvestSignGO.transform.parent = gameObject.transform;
        CustomEvents.AnimalPens.OnSetNewID?.Invoke();
        billboardID = billboardScript.GetNewID();
        animalName = animalData.SetRandomName();
        billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
        happinessLevel = animalData.happinessValue;
        currentHappinessLevel = happinessLevel;
        petParticles = Resources.Load<GameObject>("Prefabs/Particle Effects/PetFX");
        player = GameObject.FindGameObjectWithTag("Player");
        if (type == AnimalType.Horse)
        {
            GameObject sign = Instantiate(horseSpeedSign, transform.position + offset, Quaternion.identity);
            sign.transform.parent = gameObject.transform;
            PlayerSpeedSetter(true);
            harvestSignGO.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the states of the animal when a new day starts
    /// Checks if the animal has been fed and updates if resource collection is allowed via bools
    /// Checks happiness meter
    /// </summary>
    private void OnNewDay()
    {
        if(bIsFed)
        {
            if (happinessLevel > 0)
            {
                bIsHarvestable = true;
                bIsFed = false;
                switch (type)
                {
                    case AnimalType.Chicken:
                    case AnimalType.Cow:
                    case AnimalType.Pig:
                    case AnimalType.Sheep:
                        produceAmount = animalData.maxProduceReturned;
                        break;
                }
                billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
            }
            else
            {
                Debug.Log("Animal is too unhappy!");
                bIsHarvestable = false;
                produceAmount = animalData.minProduceReturned;
                billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
            }
        }
        else
        {
            bIsHarvestable = false;
            produceAmount = animalData.minProduceReturned;
            billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
        }

        if (!bCanBePet)
        {
            bCanBePet = true;
        }
        else
        {
            currentHappinessLevel -= petHappinessAmount;
            if (currentHappinessLevel < 0)
            {
                currentHappinessLevel = 0;
            }
        }

    }

    /// <summary>
    /// Function to feed the animal for harvest production
    /// Loops through the player's inventory to check for a compatible item to feed the animal
    /// Debugs if there are no items for testing only
    /// Debugs if the player has already fed the animal and tries to feed the animal again
    /// </summary>
    public void Feed()
    {
        if(!bIsFed)
        {
            Inventory playerInventory = CustomEvents.InventorySystem.PlayerInventory.OnGetInventory?.Invoke();
            if(playerInventory != null)
            {
                foreach (InventorySlot slot in playerInventory.GetInventory())
                {
                    if (slot.Item == animalData.itemToFeed)
                    {
                        SCR_Items itemToFeed = slot.Item;
                        CustomEvents.InventorySystem.PlayerInventory.OnRemoveFromItemStack?.Invoke(itemToFeed, 1, slot.Quality);
                        bIsFed = true;
                        billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
                        break;
                    }
                }
                if(!bIsFed)
                {
                    Debug.Log("No compatible items found to feed");
                }
            }
            else
            {
                Debug.LogWarning("No Player Inventory Found");
            }
        }
        else
        {
            Debug.Log("Animal has already been fed");
        }

        switch (type)
        {
            case AnimalType.Chicken:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Chicken Sound");
                break;
            case AnimalType.Pig:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Pig Sound");
                break;
            case AnimalType.Sheep:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Goat Sound");
                break;
        }
    }

    /// <summary>
    /// Function to allow for harvesting the animal if they are ready
    /// Loops through all harvest items in the animal data to then place in the inventory
    /// Loops through all child objects and destorys them to delete the harvest sign from the animal
    /// </summary>
    public void Harvest()
    {
        if(!bIsHarvestable)
        {
            Debug.Log("Animal Is Not Ready To Be Harvested");
        }
        else
        {
            foreach(SCR_Items _item in animalData.produce)
            {
                switch (_item.ItemType)
                {
                    case ItemTypes.produce:
                        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(_item, produceAmount, ItemQuality.Normal);
                        break;
                    case ItemTypes.crop:
                        CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(_item, produceAmount, ItemQuality.Normal);
                        break;
                    default:
                        Debug.LogWarning("Unhandled item type in harvest");
                        break;
                }
            }

            bIsHarvestable = false;
            billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
        }
        switch (type)
        {
            case AnimalType.Chicken:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Chicken Sound");
                break;
            case AnimalType.Pig:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Pig Sound");
                break;
            case AnimalType.Sheep:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Goat Sound");
                break;
        }
    }

    public void Pet()
    {
        if (!bCanBePet)
        {
            Debug.Log("Already been pet today");
        }
        else
        {
            currentHappinessLevel += petHappinessAmount;
            if (currentHappinessLevel > happinessLevel)
            {
                currentHappinessLevel = happinessLevel;
            }

            GameObject petFX = Instantiate(petParticles, transform.position + particleOffset, Quaternion.identity);
            Destroy(petFX, 1f);
            bCanBePet = false;
            CustomEvents.Achievements.OnAddToTotalAnimalsPet?.Invoke(1);
            billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
        }
        switch (type)
        {
            case AnimalType.Chicken:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Chicken Sound");
                break;
            case AnimalType.Pig:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Pig Sound");
                break;
            case AnimalType.Sheep:
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Goat Sound");
                break;
        }
    }

    public void Rename(string _name)
    {
        animalName = _name;
        CustomEvents.AnimalPens.OnToggleRenamer?.Invoke(true);
        billboardScript.SetBillboard(animalName, bIsFed.ToString(), bIsHarvestable.ToString(), animalData.happinessValue, billboardID);
        CustomEvents.AnimalPens.OnGetAnimalScript?.Invoke(GetComponent<AnimalBehaviour>());
    }

    private void PlayerSpeedSetter(bool state)
    {
        if (state)
        {
            player.GetComponent<NavMeshAgent>().speed += speedIncreaser;
            player.GetComponent<NavMeshAgent>().angularSpeed += speedIncreaser;
        }
        else
        {
            player.GetComponent<NavMeshAgent>().speed -= speedIncreaser;
            player.GetComponent<NavMeshAgent>().angularSpeed -= speedIncreaser;
        }
    }

    public bool GetIsFed()
    {
        return bIsFed;
    }

    public void SetIsFed(bool _state)
    {
        bIsFed = _state;
    }

    public bool GetIsHarvestable()
    {
        return bIsHarvestable;
    }

    public void SetIsHarvestable(bool _state)
    {
        bIsHarvestable = _state;
    }

    public bool GetCanBePet()
    {
        return bCanBePet;
    }

    public void SetCanBePet(bool _state)
    {
        bCanBePet = _state;
    }

    public int GetHappinessLevel()
    {
        return currentHappinessLevel;
    }

    public void SetHappinessLevel(int _amount)
    {
        currentHappinessLevel = _amount;
    }

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnNewDaySetup += OnNewDay;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnNewDaySetup -= OnNewDay;
    }
}
