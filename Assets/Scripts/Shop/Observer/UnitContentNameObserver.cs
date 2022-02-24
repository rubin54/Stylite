using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContentNameObserver : ShopOfferObserver
{
    protected override void Observe()
    {
        UnitTemplate unit = shopOffer.Content.GetComponent<UnitTemplate>();
        if (unit)
        {
            text.text = unit.Name;
        }
    }
}
