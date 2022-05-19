using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_BagMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SCR_BagSlotUI itemSlotScript;

    public void OnPointerEnter(PointerEventData data)
    {
        itemSlotScript.SetIsHovering(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        itemSlotScript.SetIsHovering(false);
        gameObject.SetActive(false);
    }
}
