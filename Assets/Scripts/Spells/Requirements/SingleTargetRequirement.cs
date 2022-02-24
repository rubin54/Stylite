using Cells;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SingleTargetRequirement : InputRequirement
{
    public SingleTargetRequirement(int minRange, int maxRange)
    {
        MinRange = minRange;
        MaxRange = maxRange;
    }

    public override InputRequirement Duplicate()
    {
        SingleTargetRequirement requirement = new SingleTargetRequirement(MinRange, MaxRange);
        requirement.Hexagon = Hexagon;

        return requirement;
    }

    public override List<Hexagon> GetAllOptions(Hexagon hexagon)
    {
        return GetEveryHexagonInAnRadius(hexagon.CubeCoord, MinRange, MaxRange);
        
    }

    public override bool IsValid(Hexagon hexagon, Unit unit)
    {
        return unit.Cell.GetDistance(hexagon) >= MinRange && unit.Cell.GetDistance(hexagon) <= MaxRange;
    }

    protected override InputType GetInputType() => InputType.SingleTargetRequirement;
}
