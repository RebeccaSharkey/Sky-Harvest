using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Base scripts which all island posts use to buy different islands
/// Allows all post to use the same Island UI
/// </summary>
public class IslandBuy : MonoBehaviour, iSaveable
{
    [SerializeField] private int islandCost;
    [SerializeField] private GameObject islandToSpawn;
    [SerializeField] private GameObject teleportTotem;
    [SerializeField] private FrozenTears currency;
    [SerializeField] private int islandID;

    [SerializeField] private string islandDescription;
    [SerializeField] private string islandName;

    private GameObject spawnFX;
    [SerializeField] private float fxScaler;
    [SerializeField] private Vector3 fxOffsetPos;

    private bool bIsWaiting = false;
    [SerializeField] private float islandSpawnDelay;
    [SerializeField] private float islandAnimDelay;

    private Animator islandAnim;
    [SerializeField] private string animName;

    [SerializeField] private List<SCR_Items> itemsToAddToShops;
    [SerializeField] private List<SCR_Items> itemsToGiveToPlayer;

    [SerializeField] private GameObject childSignPost;
    private bool bIslandBought = false;

    private void Start()
    {
        spawnFX = Resources.Load<GameObject>("Prefabs/Particle Effects/SmokeExplosionFX");
        islandAnim = islandToSpawn.GetComponent<Animator>();
        islandToSpawn.SetActive(bIslandBought);
    }

    private void StartBuyIsland(int _ID)
    {
        if (_ID == 3)
        {
            CustomEvents.Achievements.OnAddToSnowyIslandsBought?.Invoke(1);
        }
        StartCoroutine(BuyIsland(_ID));
    }

    /// <summary>
    /// Can buy an island by checking the player currency against the cost of the island
    /// Compares an island ID to allow for use of the one UI
    /// Destroys itself once the island has been bought and spawns the correct island in
    /// Adds a position that the player can then teleport to
    /// </summary>
    /// <param name="_islandId">Unique ID to use the one UI for all islands</param>
    private IEnumerator BuyIsland(int _islandId)
    {
        if (islandID == _islandId)
        {
            if (currency.amount >= islandCost)
            {
                bIslandBought = true;
                Vector3 tempLocation = transform.position;
                currency.RemoveAmount(islandCost);
                CustomEvents.Camera.OnLerpToNewPos?.Invoke(islandToSpawn.transform.position);
                bIsWaiting = true;

                while(bIsWaiting)
                {
                    yield return null;
                    if(CustomEvents.Camera.OnFinishedLerping?.Invoke() == true)
                    {
                        bIsWaiting = false;
                        break;
                    }
                }

                yield return new WaitForSeconds(islandSpawnDelay);

                spawnFX.transform.localScale *= fxScaler;
                GameObject fxGO = Instantiate(spawnFX, islandToSpawn.transform.position + fxOffsetPos, Quaternion.identity);
                Destroy(fxGO, 1f);
                islandToSpawn.SetActive(true);
                islandAnim.SetBool(animName, true);
                CustomEvents.Audio.OnPlaySoundEffect?.Invoke("Spinning Islands");
                yield return new WaitForSeconds(islandAnimDelay);
                teleportTotem.GetComponent<TeleportObject>().SetCanShake(true);
                CustomEvents.IslandSystem.Teleportation.OnAddTeleportPos?.Invoke(teleportTotem.transform.position, islandName);

                yield return new WaitForSeconds(0.5f);

                foreach(SCR_Items item in itemsToAddToShops)
                {
                    if(item.ItemType == ItemTypes.crop)
                    {
                        CustomEvents.AvailableItems.OnAddToWeeklyTasks?.Invoke(item);
                    }
                    else
                    {
                        CustomEvents.AvailableItems.OnAddToShops?.Invoke(item);
                    }
                }
                foreach(SCR_Items item in itemsToGiveToPlayer)
                {
                    CustomEvents.InventorySystem.PlayerInventory.OnAddNewItemStack?.Invoke(item, 1, ItemQuality.Normal);
                }

                yield return new WaitForSeconds(2.5f);
                CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
                CustomEvents.PlayerControls.OnSnapCamToPlayer?.Invoke(tempLocation);

                childSignPost.SetActive(false);
            }
            else
            {
                Debug.Log("Not Enough Money");
            }

        }
    }

    /// <summary>
    /// Opens the UI and passes in the information of the island spawning in
    /// </summary>
    public void OpenUI()
    {
        CustomEvents.IslandSystem.OnSetUI?.Invoke(islandName, islandDescription, islandCost, islandID);
        CustomEvents.IslandSystem.OnToggleUI?.Invoke(true);
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        
        data.Add(bIslandBought.ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        bIslandBought = bool.Parse(_data[0]);
        if (bIslandBought)
        {
            islandToSpawn.SetActive(true);
            childSignPost.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CustomEvents.IslandSystem.OnBuyIsland += StartBuyIsland;
        
    }

    private void OnDisable()
    {
        CustomEvents.IslandSystem.OnBuyIsland -= StartBuyIsland;
    }
}
