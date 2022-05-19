using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_SiloMenuBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SCR_SiloSlotUI itemSlotScript = null;
    [SerializeField] private SCR_PlayerSIloSlotUI playerItemSlotScript = null;

    public void OnPointerEnter(PointerEventData data)
    {
        if(itemSlotScript != null)
        {
            itemSlotScript.SetIsHovering(true);
        }
        else
        {
            playerItemSlotScript.SetIsHovering(true);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (itemSlotScript != null)
        {
            itemSlotScript.SetIsHovering(false);
        }
        else
        {
            playerItemSlotScript.SetIsHovering(false);
        }
        gameObject.SetActive(false);
    }
}
