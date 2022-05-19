using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupPopupSetter : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText, qualityText, quantityText;
    
    public void SetValues(PickupNotifData _data)
    {
        icon.sprite = _data.icon;
        nameText.text = _data.name;
        qualityText.text = _data.quality;
        quantityText.text = _data.quantity;
    }
}
