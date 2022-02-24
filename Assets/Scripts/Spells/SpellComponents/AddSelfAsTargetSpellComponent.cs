using Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSelfAsTargetSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        currentSpellEffects.TargetedHexs.Add((Hexagon)currentSpellEffects.Caster.Cell);
    }

    protected override SpellComponentType GetType() => SpellComponentType.Region;
}
