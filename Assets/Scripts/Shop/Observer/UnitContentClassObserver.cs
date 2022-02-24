using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitContentClassObserver : ShopOfferObserver
{
    protected override void Observe()
    {
        UnitTemplate unit = shopOffer.Content.GetComponent<UnitTemplate>();
        if (unit)
        {
            text.text = UnitTemplate.GetClassAsString(unit);
        }
    }
}
