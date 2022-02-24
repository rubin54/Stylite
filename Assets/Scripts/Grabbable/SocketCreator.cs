using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketCreator : MonoBehaviour
{
    public Action<SocketCreator> CreatedCreator;
    public Action<SocketCreator> DestroyedSocketCreator;
    public Action<GrabbableSocket> CreatedSocket; 
    public Action<GrabbableSocket> DestroyedSocket;
}
