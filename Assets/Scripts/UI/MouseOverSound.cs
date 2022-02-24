using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AK.Wwise.Event MouseSFX;

    private bool mouse_over = false;

    private void Update()
    {
        if (mouse_over)
        {
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseSFX.Post(gameObject);
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }
}
