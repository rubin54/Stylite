using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEffectPositionAtSpellAnchorSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        currentSpellEffects.EffectPositions.Add(currentSpellEffects.SpellAnchor);
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostPostRegion;
}
