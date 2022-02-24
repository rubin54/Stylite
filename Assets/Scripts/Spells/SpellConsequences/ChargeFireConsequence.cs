using Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireConsequence : Consequences
{
    public FireSpellComponent Component;
    public int Index;
    public int Amount;

    public override float CalculateImportance()
    {
        return 2;
    }

    protected override void Act(List<Hexagon> hexagons)
    {
        Component.IncreaseCharges(Index, Amount);
    }

    protected override ConsequenceType GetConsequenceType() => ConsequenceType.ChargeFireConsequence;
}
