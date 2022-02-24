using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpellAnchorToLastHitHexSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        if (currentSpellEffects.TargetedHexs.Count != 0)
        {
            currentSpellEffects.SpellAnchor = currentSpellEffects.TargetedHexs[currentSpellEffects.TargetedHexs.Count - 1];
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostRegion;
}
