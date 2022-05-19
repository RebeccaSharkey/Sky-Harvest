using UnityEngine;
using TMPro;

public class TentUI : MonoBehaviour
{
    [SerializeField] private GameObject tentUI;
    [SerializeField] private TextMeshProUGUI tentCost;
    private int tentID;

    public void ToggleUI(bool state)
    {
        if (!state)
        {
            tentUI.SetActive(false);
            CustomEvents.TimeCycle.OnUnpause?.Invoke();
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
            CustomEvents.TaskSystem.OnSetPlayerState?.Invoke(PlayerState.Waiting);
        }

        if (state)
        {
            tentUI.SetActive(true);
            CustomEvents.TimeCycle.OnPause?.Invoke();
            CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
            CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
        }
    }

    private void SetupUI(int _tentCost, int _ID)
    {
        tentCost.text = "Cost: " + _tentCost.ToString();
        tentID = _ID;
    }

    public void BuyTent()
    {
        ToggleUI(false);
        CustomEvents.Tent.OnBuyTent?.Invoke(tentID);
    }

    private void OnEnable()
    {
        CustomEvents.Tent.OnToggleUI += ToggleUI;
        CustomEvents.Tent.OnSetupUI += SetupUI;
    }

    private void OnDisable()
    {
        CustomEvents.Tent.OnToggleUI -= ToggleUI;
        CustomEvents.Tent.OnSetupUI -= SetupUI;
    }
}
