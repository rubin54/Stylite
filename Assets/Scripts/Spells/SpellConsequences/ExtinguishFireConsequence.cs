using Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFireConsequence : Consequences
{
    public Hexagon Hexagon;

    public override float CalculateImportance()
    {
        if (!Hexagon) return 0;

        if (Hexagon.OnFire) return 2;

        return -0.02f;
    }

    protected override void Act(List<Hexagon> hexagons)
    {
        if (!Hexagon) return;
        Hexagon.OnFire = false;
    }

    protected override ConsequenceType GetConsequenceType() => ConsequenceType.ExtinguishFireConsequence;
}
