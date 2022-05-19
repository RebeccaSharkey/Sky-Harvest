using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that all tents use in order to buy and spawn them in
/// </summary>
public class TentBuying : MonoBehaviour, iSaveable
{
    [SerializeField] private int tentCost;
    [SerializeField] private GameObject tentToSpawn;
    [SerializeField] private GameObject sign;
    
    [SerializeField] private Vector3 posToSpawn;
    
    [SerializeField] private int tentID;

    private bool isBought = false;

    /// <summary>
    /// Checks the currency to the cost of the tent to buy it
    /// Spawns in the tent if the player can buy it and destroys itself
    /// </summary>
    private void BuyTent(int _ID)
    {
        if (tentID == _ID)
        {
            if (CustomEvents.Currency.OnCheckCurrency?.Invoke(tentCost) == true)
            {
                CustomEvents.Currency.OnRemoveCurrency?.Invoke(tentCost);
                isBought = true;
                InitialiseTent();
            }
            else
            {
                Debug.Log("Not Enough Money");
            }
        }
    }

    private void InitialiseTent()
    {
        Instantiate(tentToSpawn, posToSpawn, Quaternion.Euler(-90f, 0f, 0f));
        sign.SetActive(false);
    }

    public void SetUpUI()
    {
        CustomEvents.Tent.OnSetupUI?.Invoke(tentCost, tentID);
        CustomEvents.Tent.OnToggleUI?.Invoke(true);
    }

    private void OnEnable()
    {
        CustomEvents.Tent.OnBuyTent += BuyTent;
    }

    private void OnDisable()
    {
        CustomEvents.Tent.OnBuyTent -= BuyTent;
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();

        data.Add(isBought.ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        isBought = bool.Parse(_data[0]);

        if (isBought) InitialiseTent();
    }
}
