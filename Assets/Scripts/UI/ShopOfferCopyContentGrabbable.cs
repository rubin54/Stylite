using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOfferCopyContentGrabbable : MonoBehaviour
{
    [SerializeField]
    public GrabbableSocket socket;

    [SerializeField]
    public ShopOffer ShopOffer;

    private void Start()
    {
        Grabbable grabbable = Instantiate(ShopOffer.Content).GetComponent<Grabbable>();
        socket.AddGrabbable(grabbable);
        grabbable.OnStoppedMoving();
    }
}
