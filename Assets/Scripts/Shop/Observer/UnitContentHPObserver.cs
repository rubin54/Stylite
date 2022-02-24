using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitContentHPObserver : ShopOfferObserver
{
    protected override void Observe()
    {
        UnitTemplate unit = shopOffer.Content.GetComponent<UnitTemplate>();
        if (unit)
        {
            text.text = unit.Health.ToString() + "HP";
        }
    }
}
