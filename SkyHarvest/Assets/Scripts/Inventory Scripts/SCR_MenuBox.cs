using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_MenuBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SCR_ItemSlotUI itemSlotScript;

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
