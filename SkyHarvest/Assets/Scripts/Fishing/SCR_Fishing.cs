using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SCR_Fishing : MonoBehaviour
{
    [SerializeField] private bool canFish;

    [Header("Fish Types")]
    [SerializeField] private List<SO_FishObject> anytimeCatch;
    [SerializeField] private List<SO_FishObject> amOnly;
    [SerializeField] private List<SO_FishObject> pmOnly;
    private List<SO_FishObject> amFishPool;
    private List<SO_FishObject> pmFishPool;
    private float amFishCumulativeProbMAX;
    private float pmFishCumulativeProbMAX;

    [Header("Implementation Variables")]
    [SerializeField] private int waterID;
    [HideInInspector] public int WaterID { get => waterID; }
    private SO_FishObject currentFish = null;
    private int hour = 0;

    public void Start()
    {
        amFishPool = new List<SO_FishObject>();
        pmFishPool = new List<SO_FishObject>();
        amFishCumulativeProbMAX = 0.0f;
        pmFishCumulativeProbMAX = 0.0f;
        foreach (SO_FishObject fish in anytimeCatch)
        {
            amFishPool.Add(fish);
            pmFishPool.Add(fish);
            amFishCumulativeProbMAX += fish.Probability;
            pmFishCumulativeProbMAX += fish.Probability;
        }
        foreach (SO_FishObject fish in amOnly)
        {
            amFishPool.Add(fish);
            amFishCumulativeProbMAX += fish.Probability;
        }
        foreach (SO_FishObject fish in pmOnly)
        {
            pmFishPool.Add(fish);
            pmFishCumulativeProbMAX += fish.Probability;
        }
    }

    private void OnOpen(int thisID)
    {
        if(waterID == thisID)
        {
            //Send over this fishing id
            if(canFish)
            {
                CustomEvents.Fishing.OnSetupFishingUI?.Invoke(waterID);
                CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
                CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Fishing");
            }
        }
    }

    private void UpdateHour(int _newHour)
    {
        hour = _newHour;
    }

    private void Fish(int thisID)
    {
        if(waterID == thisID)
        {
            List<SO_FishObject> fishes = new List<SO_FishObject>();


            //-------------------------------------------------OLD CODE FOR TRYING TO CHOOSE RANDOM FISH DEPENDING ON PROBABILITY
            /*foreach(SO_FishObject fish in anytimeCatch)
            {
                //Add more of the higher probability fish to the list
                int probability = (int)(fish.Probability * 100);
                for(int i = 0; i <= probability; i++)
                {
                    fishes.Add(fish);
                }
            }

            if(hour <= 12)
            {
                foreach (SO_FishObject fish in amOnly)
                {
                    //Add more of the higher probability fish to the list
                    int probability = (int)(fish.Probability * 100);
                    for (int i = 0; i <= probability; i++)
                    {
                        fishes.Add(fish);
                    }
                }
            }
            else
            {
                foreach (SO_FishObject fish in pmOnly)
                {
                    //Add more of the higher probability fish to the list
                    int probability = (int)(fish.Probability * 100);
                    for (int i = 0; i <= probability; i++)
                    {
                        fishes.Add(fish);
                    }
                }
            }*/

            //New code for random fish selection based of probabilty :)
            if(hour <= 12)
            {
                float randomNumber = Random.Range(0f, amFishCumulativeProbMAX);
                float amFishCumulativeProb = 0f;
                foreach(SO_FishObject fish in amFishPool)
                {
                    amFishCumulativeProb += fish.Probability;
                    if(randomNumber <= amFishCumulativeProb)
                    {
                        currentFish = fish;
                        break;
                    }
                }
            }
            else
            {
                float randomNumber = Random.Range(0f, pmFishCumulativeProbMAX);
                float pmFishCumulativeProb = 0f;
                foreach (SO_FishObject fish in pmFishPool)
                {
                    pmFishCumulativeProb += fish.Probability;
                    if (randomNumber <= pmFishCumulativeProb)
                    {
                        currentFish = fish;
                        break;
                    }
                }
            }

            CustomEvents.Fishing.OnSetFish?.Invoke(currentFish);

            //Use this fishes details and play out minigame based on fishes difficulty to catch.
            CustomEvents.Fishing.OnPlayMiniGame?.Invoke();
        }
    }

    private void FishCaught(int thisID)
    {
        if (waterID == thisID)
        {
            //Place fish into players inventory
            CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(currentFish.Item, 1, (ItemQuality)Random.Range(0, 4));
            CustomEvents.Fishing.OnIncrimentAmountCaught?.Invoke((SO_Fish)currentFish.Item);
        }
    }

    private void OnEnable()
    {
        CustomEvents.Fishing.OnOpenFishingUI += OnOpen;
        CustomEvents.Fishing.OnFish += Fish;
        CustomEvents.Fishing.OnFishCaught += FishCaught;
        CustomEvents.UI.OnTimeChangedHours += UpdateHour;
    }

    private void OnDisable()
    {
        CustomEvents.Fishing.OnOpenFishingUI -= OnOpen;
        CustomEvents.Fishing.OnFish -= Fish;
        CustomEvents.Fishing.OnFishCaught -= FishCaught;
        CustomEvents.UI.OnTimeChangedHours -= UpdateHour;
    }
}
