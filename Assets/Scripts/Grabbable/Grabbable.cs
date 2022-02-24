using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Content
{
    public string Header;
    public string Body;

    public Content(string header, string body)
    {
        Header = header;
        Body = body;
    }
}

public class Grabbable : MonoBehaviour
{
    public Action<Grabbable> ClickedGrabbable;
    public Action<Grabbable> StoppedMoving;
    public Action<Grabbable> QueuedSwitchingSockets;
    public Action<Grabbable> Destroyed;

    public Action<Grabbable> Entered;
    public Action<Grabbable> Exited;

    [SerializeField]
    private InstantClickSelectable selectable;

    [SerializeField]
    protected string headerDescription = "";

    [SerializeField]
    protected string bodyDescription = "";

    private void Start()
    {
        if(selectable)
        {
            selectable.Clicked += OnClicked;
            selectable.Entered += OnEntered;
            selectable.Exited += OnExited;
        }
    }

    virtual public bool IsEndless()
    {
        return false;
    }

    public void OnClicked() => ClickedGrabbable?.Invoke(this);

    public void OnEntered() => Entered?.Invoke(this);
    public void OnExited() => Exited?.Invoke(this);

    public void OnStoppedMoving() => StoppedMoving?.Invoke(this);

    public void Activate()
    {
        selectable.interactable = true;
    }

    public void Deactivate()
    {
        selectable.interactable = false;
    }

    public bool IsInteractable()
    {
        return selectable.interactable;
    }

    public virtual Content GetContent()
    {
        return new Content(headerDescription, bodyDescription);
    }
}
