using UnityEngine;

/// <summary>
/// Scriptable Object holding the data and functions for the currency
/// </summary>
[CreateAssetMenu(fileName = "Currency", menuName = "SkyHarvest/ScriptableObjects/Frozen Tear")]
public class FrozenTears : ScriptableObject
{
    
    private int _amount;
    public int amount
    {
        get
        {
            return _amount;
        }
        private set
        {
            _amount = value;
            CustomEvents.UI.OnMoneyChanged?.Invoke(amount);
        }
    }

    private void Awake()
    {
        amount = 0;
    }
    
    public void AddAmount(int _amount)
    {
        if(amount + _amount >= 1000000000)
        {
            amount = 1000000000;
        }
        else
        {
            amount += _amount;
        }
    }

    public bool CheckAmount(int _amount)
    {
        if(amount >= _amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveAmount(int _amount)
    {
        if (amount + _amount <= 0)
        {
            amount = 0;
        }
        else
        {
            amount -= _amount;
        }
    }

    public void SetAmount(int _amount)
    {
        amount = _amount;
    }
}