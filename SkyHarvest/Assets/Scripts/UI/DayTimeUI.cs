using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText, dayText;

    private int hour, minute;

    private void OnEnable()
    {
        CustomEvents.UI.OnTimeChangedMinutes += UpdateMinute;
        CustomEvents.UI.OnTimeChangedHours += UpdateHour;
        CustomEvents.UI.OnDayChanged += UpdateDaytext;
    }

    private void OnDisable()
    {
        CustomEvents.UI.OnTimeChangedMinutes -= UpdateMinute;
        CustomEvents.UI.OnTimeChangedHours -= UpdateHour;
        CustomEvents.UI.OnDayChanged -= UpdateDaytext;
    }

    private void UpdateMinute(int _newMinute)
    {
        minute = _newMinute;
        UpdateTimeText();
    }

    private void UpdateHour(int _newHour)
    {
        hour = _newHour;
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        timeText.text = $"{hour.ToString("D2")}:{minute.ToString("D2")}";
    }

    private void UpdateDaytext(int _day)
    {
        dayText.text = $"Day {_day}";
    }
}
