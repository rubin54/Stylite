using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Action MouseEntered;
    public Action MouseExited;
    public Action MouseClicked;

    protected virtual void OnMouseEnter()
    {
        MouseEntered?.Invoke();
    }
    protected virtual void OnMouseExit()
    {
        MouseExited?.Invoke();
    }
    void OnMouseDown()
    {
        MouseClicked?.Invoke();
    }
}
