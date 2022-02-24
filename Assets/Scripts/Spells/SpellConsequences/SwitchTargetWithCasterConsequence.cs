using Cells;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SwitchTargetWithCasterConsequence : Consequences
{
    public Unit Caster;
    public Unit Target;

    public override float CalculateImportance()
    {
        //please calculate if the Target is near pits, and if oneself removes themselfs from Pits, thaanks =///=

        return 3;
    }

    protected override void Act(List<Hexagon> hexagons)
    {
        Cell casterCell = Caster.Cell;

        Caster.Cell = Target.Cell;
        Caster.Cell.CurrentUnit = Caster;
        Caster.transform.position = Caster.Cell.transform.position;

        Target.Cell = casterCell;
        Target.Cell.CurrentUnit = Target;
        Target.transform.position = Target.Cell.transform.position;
    }

    protected override ConsequenceType GetConsequenceType() => ConsequenceType.SwitchCasterWithFirstTargetConsequence; 
}
