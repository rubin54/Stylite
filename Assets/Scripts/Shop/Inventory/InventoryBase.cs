using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    [SerializeField]
    protected List<GrabbableSocket> sockets;

    protected void AddGrabbableToEmptySocket(List<GrabbableSocket> sockets, Grabbable grabbable)
    {
        foreach (var socket in sockets)
        {
            if (socket.IsEmpty())
            {
                socket.AddGrabbable(grabbable);
                socket.OnGrabbableStoppedMoving(grabbable);
                return;
            }
        }
    }
}
