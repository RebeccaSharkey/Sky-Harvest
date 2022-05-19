using UnityEngine;

/// <summary>
/// Monobehaviour in scene so currency scriptable object can respond to events
/// </summary>
public class CurrencyManager : MonoBehaviour, iSaveable
{
    [SerializeField] private FrozenTears frozenRaindrops;
    [SerializeField] private int amount;

    private void OnEnable()
    {
        CustomEvents.Currency.OnAddCurrency += AddRaindrops;
        CustomEvents.Currency.OnRemoveCurrency += RemoveRaindrops;
        CustomEvents.Currency.OnGetCurrency += GetRaindropsAmount;
        CustomEvents.Currency.OnCheckCurrency += CheckCurrency;
    }

    private void OnDisable()
    {
        CustomEvents.Currency.OnAddCurrency -= AddRaindrops;
        CustomEvents.Currency.OnRemoveCurrency -= RemoveRaindrops;
        CustomEvents.Currency.OnGetCurrency -= GetRaindropsAmount;
        CustomEvents.Currency.OnCheckCurrency -= CheckCurrency;
    }

    private void Start()
    {
        frozenRaindrops.SetAmount(amount);
        UpdateAmount();
    }

    private void AddRaindrops(int amount)
    {
        frozenRaindrops.AddAmount(amount);
        UpdateAmount();

        if (frozenRaindrops.amount >= 100000)
        {
            CustomEvents.Achievements.OnAddToTotalMoneyGot?.Invoke(1);
        }
    }

    private void RemoveRaindrops(int amount)
    {
        frozenRaindrops.RemoveAmount(amount);
        UpdateAmount();
    }

    private bool CheckCurrency(int amount)
    {
        return frozenRaindrops.CheckAmount(amount);
    }

    private int GetRaindropsAmount()
    {
        return frozenRaindrops.amount;
    }

    private void UpdateAmount()
    {
        amount = frozenRaindrops.amount;
        CustomEvents.UI.OnMoneyChanged?.Invoke(amount);
    }

    public SerializableList SaveData()
    {
        SerializableList data = new SerializableList();
        data.Add(GetRaindropsAmount().ToString());

        return data;
    }

    public void LoadData(SerializableList _data)
    {
        frozenRaindrops.SetAmount(int.Parse(_data[0]));
        UpdateAmount();
    }
}
