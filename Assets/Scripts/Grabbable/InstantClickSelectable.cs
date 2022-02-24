using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstantClickSelectable : Selectable
{
    public Action Clicked;
    public Action Entered;
    public Action Exited;


    public override void OnPointerDown(PointerEventData eventData)
    {
        if(interactable)
        {
            Clicked?.Invoke();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Entered?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Exited?.Invoke();
    }
}
