using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerTest : MonoBehaviour,IPointerEnterHandler  
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("进入");
    }
}
