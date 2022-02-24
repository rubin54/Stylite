using Cells;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class LightHexagonOnFireConsequence : Consequences
{
    public Unit Caster;
    public FireSpellComponent Component;
    public Hexagon Hexagon;

    public override float CalculateImportance()
    {
        float importance = 0;

        if (!Hexagon.OnFire) importance += 1;
        if(Hexagon.CurrentUnit)
        {
            if (Caster.Teammates.Contains(Hexagon.CurrentUnit)) importance -= 2;
            else
            {
                importance += 2;
            }
        }
        else
        {
            importance *= 0;
        }
        return importance;
    }

    protected override void Act(List<Hexagon> hexagons)
    {
        Hexagon.OnFire = true;
        Component.ResetCharges();
    }

    protected override ConsequenceType GetConsequenceType() => ConsequenceType.PlaceFireConsequence;
}
