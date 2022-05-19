using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    crop,
    seed,
    seedPacket,
    fertilizer,
    treeSeeds,
    produce,
    fish,
    unique,
    animalProduce
};


public class SCR_Items : ScriptableObject
{
    [SerializeField] private string _itemName;
    [HideInInspector] public string ItemName { get => _itemName; set => _itemName = value; }

    [Header("Inventory Info:")]
    [SerializeField] private ItemTypes _itemType;
    [HideInInspector] public ItemTypes ItemType { get => _itemType; set => _itemType = value; }

    [SerializeField] private GameObject _itemModel;
    [HideInInspector] public GameObject ItemModel { get => _itemModel; set => _itemModel = value; }

    [SerializeField] private Sprite _sprite;
    [HideInInspector] public Sprite ItemSprite { get => _sprite; set => _sprite = value; }

    [SerializeField] private bool _stackable;
    [HideInInspector] public bool Stackable { get => _stackable; set => _stackable = value; }

    [SerializeField] private int _stackAmount = 999;
    [HideInInspector] public int StackAmount { get => _stackAmount; set => _stackAmount = value; }

    [SerializeField] private int _sellvalueBad;
    [HideInInspector] public int SellValueBad { get => _sellvalueBad; set => _sellvalueBad = value; }
    [SerializeField] private int _sellvalueNormal;
    [HideInInspector] public int SellValueNormal { get => _sellvalueNormal; set => _sellvalueNormal = value; }
    [SerializeField] private int _sellvalueGood;
    [HideInInspector] public int SellValueGood { get => _sellvalueGood; set => _sellvalueGood = value; }
    [SerializeField] private int _sellvaluePerfect;
    [HideInInspector] public int SellValuePerfect { get => _sellvaluePerfect; set => _sellvaluePerfect = value; }

    [SerializeField] private int _buyValueBad;
    [HideInInspector] public int BuyValueBad { get => _buyValueBad; set => _buyValueBad = value; }
    [SerializeField] private int _buyValueNormal;
    [HideInInspector] public int BuyValueNormal { get => _buyValueNormal; set => _buyValueNormal = value; }
    [SerializeField] private int _buyValueGood;
    [HideInInspector] public int BuyValueGood { get => _buyValueGood; set => _buyValueGood = value; }
    [SerializeField] private int _buyValuePerfect;
    [HideInInspector] public int BuyValuePerfect { get => _buyValuePerfect; set => _buyValuePerfect = value; }

    [SerializeField] private bool _ignoreQuality;
    [HideInInspector] public bool IgnoreQuality { get => _ignoreQuality; set => _ignoreQuality = value; }

}
