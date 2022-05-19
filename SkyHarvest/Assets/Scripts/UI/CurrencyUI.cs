using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private void OnEnable()
    {
        CustomEvents.UI.OnMoneyChanged += UpdateCurrencyUI;
    }

    private void OnDisable()
    {
        CustomEvents.UI.OnMoneyChanged -= UpdateCurrencyUI;
    }

    private void UpdateCurrencyUI(int _newAmount)
    {
        currencyText.text = $"Currency: {_newAmount}";
    }
}
