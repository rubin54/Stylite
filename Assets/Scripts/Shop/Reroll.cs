using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reroll : MonoBehaviour
{
    [SerializeField]
    private Shop shop;

    [SerializeField]
    private Purchase purchase;

    private void Start()
    {
        purchase.SuccessfulPurchase += OnSuccessfullPurchase;
    }

    public void OnSuccessfullPurchase()
    {
        shop.RegenerateOffers();
    }
}
