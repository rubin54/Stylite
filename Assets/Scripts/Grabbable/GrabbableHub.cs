using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableHub : MonoBehaviour
{
    [SerializeField]
    private List<SocketCreator> initSocketCreator;

    [SerializeField]
    private List<GrabbableSocket> initSockets;

    [SerializeField]
    private bool blockMovement = false;

    #region mouse curser
    [SerializeField]
    private Texture2D[] curs;

    public Vector2 spot = Vector2.zero;

    public CursorMode curMode = CursorMode.Auto;
    #endregion
    protected List<SocketCreator> socketCreators;

    protected List<GrabbableSocket> sockets;

    protected Grabbable grabbable;
    protected GrabbableSocket currentSocket;

    protected ContentVisualizer currentVisualizer;
    protected GrabbableSocket currentVisualizingSocket;

    void Awake()
    {
        sockets = new List<GrabbableSocket>();

        foreach(GrabbableSocket socket in initSockets)
        {
            AddSocketToList(socket);
        }

        socketCreators = new List<SocketCreator>();

        foreach (SocketCreator socketCreator in initSocketCreator)
        {
            AddSocketCreatorToList(socketCreator);
        }

        OnStart();
    }

    protected virtual void OnStart() { }

    private void Update()
    {
        if(grabbable)
        {
            if(Input.GetMouseButton(0))
            {
                //TODO: cursor
                //Cursor.SetCursor(curs[1], spot, curMode);
                grabbable.transform.position = Input.mousePosition;
            }
            else
            {
                //TODO: mouse cursor
                //Cursor.SetCursor(curs[0], spot, curMode);
                GrabbableSocket overlappingSocket = currentSocket;
                foreach (GrabbableSocket socket in sockets)
                {
                    if(socket.PositionIsInsideHitbox(Input.mousePosition))
                    {
                        overlappingSocket = socket;
                        break;
                    }
                }

                currentSocket.MoveContent(overlappingSocket);

                grabbable = null;
                currentSocket = null;
            }
        }
    }

    public void AddSocketCreatorToList(SocketCreator socketCreator)
    {
        socketCreators.Add(socketCreator);
        socketCreator.CreatedCreator += AddSocketCreatorToList;
        socketCreator.CreatedSocket += AddSocketToList;
    }

    public void RemoveSocketCreatorFromList(SocketCreator socketCreator)
    {
        socketCreators.Remove(socketCreator);
        socketCreator.DestroyedSocketCreator -= AddSocketCreatorToList;
        socketCreator.DestroyedSocket -= AddSocketToList;
    }

    public void AddSocketToList(GrabbableSocket grabbableSocket)
    {
        sockets.Add(grabbableSocket);
        grabbableSocket.ClickedGrabbable += OnClickedGrabbable;
        grabbableSocket.startedVisualizing += OnStartedVisualization;
        grabbableSocket.Destroyed += RemoveSocketFromList;
    }

    public void RemoveSocketFromList(GrabbableSocket grabbableSocket)
    {
        sockets.Remove(grabbableSocket);
        grabbableSocket.ClickedGrabbable -= OnClickedGrabbable;
        grabbableSocket.startedVisualizing -= OnStartedVisualization;
        grabbableSocket.Destroyed -= RemoveSocketFromList;
    }

    public void OnClickedGrabbable(GrabbableSocket socket, Grabbable grabbable)
    {
        if (blockMovement) return;

        this.grabbable = grabbable;
        this.currentSocket = socket;
        grabbable.gameObject.transform.SetParent(transform);
    }

    public void OnStartedVisualization(ContentVisualizer visualizer, GrabbableSocket socket)
    {
        if (currentVisualizer) currentVisualizingSocket.StopVisualizingContent(socket.grabbable);

        visualizer.gameObject.transform.SetParent(transform);
        float socketHeight = socket.GetComponent<RectTransform>().rect.width;
        float visualizerHeight = visualizer.GetComponent<RectTransform>().rect.width;

        RectTransform visualizerRect = visualizer.GetComponent<RectTransform>();

        float calculatedHeight = 160 + 30 * visualizer.Body.text.Split('\n').Length;
        visualizer.GetComponent<RectTransform>().sizeDelta = new Vector2(visualizerRect.rect.width, calculatedHeight);


        Vector3 wantedPosition = socket.GetComponent<RectTransform>().position;

        wantedPosition.x -= visualizerHeight / 2 + socketHeight / 2;


        if (wantedPosition.y + visualizerRect.rect.height / 2 > Screen.height)
        {
            wantedPosition.y = Screen.height - visualizerRect.rect.height / 2;
        }
        else if(wantedPosition.y - visualizerRect.rect.height / 2 < 0)
        {
            wantedPosition.y = visualizerRect.rect.height / 2;
        }

        visualizer.transform.position = wantedPosition;

        currentVisualizer = visualizer;
        currentVisualizingSocket = socket;
    }
}
