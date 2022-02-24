using Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnEveryTileComponent : SpellComponent
{
    [SerializeField]
    public int Damage = 0;

    protected override SpellComponentType GetType() => SpellComponentType.Damage;

    protected override void CastInternal(Dictionary<int, InputRequirement> input,ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        foreach (Hexagon hexagon in currentSpellEffects.TargetedHexs)
        {
            if (hexagon.CurrentUnit != null)
            {
                consequences.Add(new DamageTargetConsequence(currentSpellEffects.Caster, hexagon.CurrentUnit, Damage));
            }
        }
    }
}
