using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopOfferObserver : MonoBehaviour
{
    [SerializeField]
    protected ShopOffer shopOffer;

    [SerializeField]
    protected TextMeshProUGUI text;

    private void Start()
    {
        if(shopOffer && text)
        {
            Observe();
        }
    }

    protected virtual void Observe()
    {

    }
}
