using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SwitchCasterWithFirstTargetSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        Unit firstTarget = null;

        foreach (var hexagon in currentSpellEffects.TargetedHexs)
        {
            if(hexagon.CurrentUnit)
            {
                firstTarget = hexagon.CurrentUnit;
                break;
            }
        }

        if(firstTarget)
        {
            SwitchTargetWithCasterConsequence consequence = new SwitchTargetWithCasterConsequence();
            consequence.Caster = currentSpellEffects.Caster;
            consequence.Target = firstTarget;
            consequences.Add(consequence);
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostDamage;
}
