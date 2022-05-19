using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SCR_SpecialShop : MonoBehaviour
{
    [SerializeField] private GameObject dockedShop;
    [SerializeField] private GameObject npcAssistant;

    private bool isShopDocked = false;
    public bool IsShopDocked { get => isShopDocked; set => isShopDocked = value; }

    private void Awake()
    {
        GameObject[] specialShops;
        specialShops = GameObject.FindGameObjectsWithTag("SpecialShop");
        if (specialShops.Length == 0)
        {
            gameObject.GetComponent<SCR_SpecialShopInventory>()._shopID = 1;
        }
        else
        {            
            for (int i = 0; i < specialShops.Length; i++)
            {
                if (this.gameObject == specialShops[i].GetComponentInParent<SCR_SpecialShop>().gameObject)
                {
                    if(gameObject.GetComponent<SCR_SpecialShopInventory>()._shopID == -1)
                    {
                        gameObject.GetComponent<SCR_SpecialShopInventory>()._shopID = i;
                    }
                }
            }
        }
    }

    public void Start()
    {
        SetNewDay();
    }

    private void SetNewDay()
    {
        float probability = Random.Range(0f, 1f);

        if (probability > 0.75f)
        {
            isShopDocked = true;
            npcAssistant.SetActive(true);
            dockedShop.SetActive(true);
            GetComponent<SCR_SpecialShopInventory>().ResetInventroy();
        }
        else
        {
            isShopDocked = false;
            npcAssistant.SetActive(false);
            dockedShop.SetActive(false);
        }
    }

    public void OnNotDocked()
    {
        CustomEvents.ShopSystem.SpecialShop.OnWarning?.Invoke();
    }

    private void OnEnable()
    {
        CustomEvents.TimeCycle.OnDayStart += SetNewDay;
    }

    private void OnDisable()
    {
        CustomEvents.TimeCycle.OnDayStart -= SetNewDay;
    }
}
