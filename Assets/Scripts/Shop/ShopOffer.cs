using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContentType
{
    Invalid = -1,
    Spell,
    Unit,
    Count
}

public class ShopOffer : MonoBehaviour
{
    public Action<ShopOffer> SuccessfullPurchase;

    [SerializeField]
    public ContentType Type = ContentType.Invalid;

    [SerializeField]
    public GameObject Content;

    [SerializeField]
    Purchase purchase;

    public void Setup(Resource resource)
    {
        purchase.SetResource(resource);
        purchase.SuccessfulPurchase += OnSuccessfullPurchase;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void OnSuccessfullPurchase()
    {
        SuccessfullPurchase?.Invoke(this);
    }

    public void Activate()
    {
        purchase.BlockedTransaction = false;
    }

    public void Deactivate()
    {
        purchase.BlockedTransaction = true;
    }
}
