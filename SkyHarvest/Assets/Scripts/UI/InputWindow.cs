using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textDisplay;

    private void OnToggleUI(bool state, string _titleText, string _inputText)
    {
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(false);
        CustomEvents.Scripts.OnDisablePlayer?.Invoke(false);
        gameObject.SetActive(state);
        inputText.text = _inputText;
        titleText.text = _titleText;
    }

    public void OnConfirmInputBtn()
    {
        CustomEvents.UI.OnConfirmInput?.Invoke();
    }

    private void OnConfirmInput()
    {
        textDisplay.text = inputText.text;
        CustomEvents.UI.OnToggleInputWindowUI?.Invoke(false, null, null);
        CustomEvents.Scripts.OnDisableCameraMovement?.Invoke(true);
        CustomEvents.Scripts.OnDisablePlayer?.Invoke(true);
    }

    private void OnEnable()
    {
        CustomEvents.UI.OnToggleInputWindowUI += OnToggleUI;
        CustomEvents.UI.OnConfirmInput += OnConfirmInput;
    }

    private void OnDisable()
    {
        CustomEvents.UI.OnToggleInputWindowUI -= OnToggleUI;
        CustomEvents.UI.OnConfirmInput -= OnConfirmInput;
    }
}
