using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellTemplateInPrefabNameObserver : ShopOfferObserver
{
    protected override void Observe()
    {
        SpellTemplate spell = shopOffer.Content.GetComponent<SpellTemplate>();
        if (spell)
        {
            text.text = spell.Name;
        }
    }
}
