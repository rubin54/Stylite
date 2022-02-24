using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabbableSocket : MonoBehaviour
{
    public Action<GrabbableSocket, Grabbable> ClickedGrabbable;
    public Action<ContentVisualizer, GrabbableSocket> startedVisualizing;
    public Action<GrabbableSocket> Destroyed;

    [SerializeField]
    public Transform Socket;

    public Grabbable grabbable;

    public GameObject VisualizerPrefab;
    private ContentVisualizer currentVisualizer;

    private void Start()
    {
        if(grabbable != null)
        {
            ObserveGrabbable(grabbable);
        }

        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public bool IsEmpty()
    {
        return grabbable == null;
    }

    public void AddGrabbable(Grabbable grabbable)
    {
        this.grabbable = grabbable;
        ObserveGrabbable(grabbable);
        OnAddGrabbable(grabbable);
    }

    protected virtual void OnAddGrabbable(Grabbable grabbable)
    {

    }

    public Grabbable RemoveGrabbable()
    {
        Grabbable removedGrabbable = grabbable;
        if(grabbable) LoseGrabbable(removedGrabbable);
        grabbable = null;
        OnRemovedGrabbable(removedGrabbable);
        return removedGrabbable;
    }

    protected virtual void OnRemovedGrabbable(Grabbable removedGrabbable)
    {

    }

    public virtual bool CheckIfSwitchIsPossible(GrabbableSocket other)
    {
        return true;
    }

    protected void ObserveGrabbable(Grabbable grabbable)
    {
        if(grabbable)
        {
            grabbable.ClickedGrabbable += OnClickedGrabbable;
            grabbable.StoppedMoving += OnGrabbableStoppedMoving;
            grabbable.QueuedSwitchingSockets += OnGrabbableQueuedSwitchingSockets;
            grabbable.Destroyed += OnGrabbableGotDestroyed;

            grabbable.Entered += VisualizeContent;
            grabbable.ClickedGrabbable += StopVisualizingContent;
            grabbable.Exited += StopVisualizingContent;
        }
    }

    protected void LoseGrabbable(Grabbable grabbable)
    {
        grabbable.ClickedGrabbable -= OnClickedGrabbable;
        grabbable.StoppedMoving -= OnGrabbableStoppedMoving;
        grabbable.QueuedSwitchingSockets -= OnGrabbableQueuedSwitchingSockets;
        grabbable.Destroyed -= OnGrabbableGotDestroyed;

        grabbable.Entered -= VisualizeContent;
        grabbable.ClickedGrabbable -= StopVisualizingContent;
        grabbable.Exited -= StopVisualizingContent;
    }

    public bool PositionIsInsideHitbox(Vector2 position)
    {
        RectTransform rect = GetComponent<RectTransform>();
        if(position.x > rect.position.x - (rect.rect.width/2) && position.x < rect.position.x + (rect.rect.width / 2))
        {
            if (position.y > rect.position.y - (rect.rect.height / 2) && position.y < rect.position.y + (rect.rect.height / 2))
            {
                return true;
            }
        }

        return false;
    }

    public virtual void OnGrabbableQueuedSwitchingSockets(Grabbable grabbable)
    {
    }

    public void OnGrabbableStoppedMoving(Grabbable grabbable)
    {
        grabbable.transform.SetParent(Socket);
        grabbable.transform.position = transform.position;
        grabbable.transform.localScale = Vector3.one;

        RectTransform rect = grabbable.GetComponent<RectTransform>();
        if(rect)
        {
            rect.sizeDelta = new Vector2(0, 0) ;
        }
    }

    public virtual void MoveContent(GrabbableSocket socket)
    {
        Grabbable retVal = socket.SetContent(RemoveGrabbable(), socket);
        if(retVal)
        {
            if(retVal.IsEndless())
            {
                Destroy(retVal.gameObject);
                return;
            }

            AddGrabbable(retVal);
            OnGrabbableStoppedMoving(grabbable);
        }
    }

    public virtual Grabbable SetContent(Grabbable grabbable, GrabbableSocket socket)
    {
        if (grabbable.IsEndless())
        {
            Destroy(grabbable.gameObject);
        }

        Grabbable oldGrabbable = RemoveGrabbable();
        AddGrabbable(grabbable);
        OnGrabbableStoppedMoving(grabbable);
        return oldGrabbable;
    }
    
    public virtual void OnClickedGrabbable(Grabbable grabbable) => ClickedGrabbable?.Invoke(this, grabbable);


    public virtual void VisualizeContent(Grabbable grabbable)
    {
        if(VisualizerPrefab && currentVisualizer == null)
        {
            currentVisualizer = Instantiate(VisualizerPrefab).GetComponent<ContentVisualizer>();
            currentVisualizer.transform.position = transform.position;
            currentVisualizer.Visualize(grabbable.GetContent());
            startedVisualizing?.Invoke(currentVisualizer, this);
        }
    }

    public virtual void StopVisualizingContent(Grabbable grabbable)
    {
        if(currentVisualizer)
        {
            currentVisualizer.Destroy();
            currentVisualizer = null;
        }
    }

    public virtual void OnGrabbableGotDestroyed(Grabbable grabbable)
    {
        if(this.grabbable = grabbable)
        {
            LoseGrabbable(grabbable);
            grabbable = null;
        }
    }
}
