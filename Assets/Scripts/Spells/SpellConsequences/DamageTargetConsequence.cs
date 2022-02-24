using Cells;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class DamageTargetConsequence : Consequences
{
    public Unit Caster;
    public Unit Unit;

    public int Damage;

    protected override ConsequenceType GetConsequenceType() => ConsequenceType.DamageTargetConsequence;

    public DamageTargetConsequence(Unit caster, Unit unit, int damage)
    {
        Caster = caster;
        Unit = unit;
        Damage = damage;
    }

    public override float CalculateImportance()
    {
        float importance = 0;

        float factor = Caster == Unit ? -8 : 6;
        if (Caster.Teammates.Contains(Unit) && Unit != Caster) factor *= -1;

        importance = Damage * factor;

        return importance;
    }

    protected override void Act(List<Hexagon> hexagons)
    {
        if(Damage >= 0)
        {
            Unit.ReceiveDamage(Damage);
        }
        else if (Damage < 0)
        {
            Unit.Heal(Damage * -1);
        }
    }
}
