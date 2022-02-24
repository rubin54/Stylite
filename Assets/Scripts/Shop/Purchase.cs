using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    public Action SuccessfulPurchase;

    [SerializeField]
    private Resource resource;

    [SerializeField]
    private Resource cost;

    public bool BlockedTransaction = false;

    public void TryPurchasing()
    {
        if(resource.Contains(cost.Amount) && !BlockedTransaction)
        {
            resource.Subtract(cost.Amount);
            SuccessfulPurchase?.Invoke();
        }
    }

    public void SetResource(Resource resource)
    {
        this.resource = resource;
    }

}
