using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEffectPositionAtAllTargetedHexsSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input,ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        foreach (var hexagon in currentSpellEffects.TargetedHexs)
        {
            currentSpellEffects.EffectPositions.Add(hexagon);
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostDamage;
}
