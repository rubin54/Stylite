using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFireSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        foreach (var hexagon in currentSpellEffects.TargetedHexs)
        {
            hexagon.OnFire = false;
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostRegion;
}
