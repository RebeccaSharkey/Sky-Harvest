using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalBillboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI animalName;
    [SerializeField] private TextMeshProUGUI feedStatus;
    [SerializeField] private TextMeshProUGUI harvestStatus;

    [SerializeField] private Slider happinessSlider;
    [SerializeField] private Image sliderHandle;
    [SerializeField] private Sprite veryHappyLogo, happyLogo, neutralLogo, sadLogo, verySadLogo;
    private int idCount = 0;
    [SerializeField] private int ID = 0;

    public Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0);
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            transform.LookAt(transform.position + target.forward);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    public void SetBillboard(string _name, string _feedStatus, string _harvestStatus, int _happinessValue, int _id)
    {
        if (ID == _id)
        {
            animalName.text = _name;
            SetHappinessLogo(_happinessValue);
            happinessSlider.value = _happinessValue;
            switch (_feedStatus)
            {
                case "True":
                    _feedStatus = "Fed";
                    feedStatus.color = Color.green;
                    break;
                case "False":
                    _feedStatus = "Not Fed";
                    feedStatus.color = Color.red;
                    break;
            }

            switch (_harvestStatus)
            {
                case "True":
                    _harvestStatus = "Can Harvest";
                    harvestStatus.color = Color.green;
                    break;
                case "False":
                    _harvestStatus = "Cannot Harvest";
                    harvestStatus.color = Color.red;
                    break;
            }
            feedStatus.text = _feedStatus;
            harvestStatus.text = _harvestStatus;
        }
    }

    public void SetNewID()
    {
        idCount++;
        ID = idCount;
    }

    public int GetNewID()
    {
        return idCount;
    }

    public void SetHappinessLogo(int _amount)
    {
        if (_amount >= 0 && _amount <= 20)
        {
            sliderHandle.sprite = verySadLogo;
        }

        if (_amount >= 21 && _amount <= 40)
        {
            sliderHandle.sprite = sadLogo;
        }

        if (_amount >= 41 && _amount <= 60)
        {
            sliderHandle.sprite = neutralLogo;
        }

        if (_amount >= 61 && _amount <= 80)
        {
            sliderHandle.sprite = happyLogo;
        }

        if (_amount >= 81 && _amount <= happinessSlider.maxValue)
        {
            sliderHandle.sprite = veryHappyLogo;
        }
    }
}
