using Cells;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SingleTargetAndEmptyRequirement : InputRequirement
{

    public SingleTargetAndEmptyRequirement(int minRange, int maxRange)
    {
        MinRange = minRange;
        MaxRange = maxRange;
    }

    public override InputRequirement Duplicate()
    {
        SingleTargetAndEmptyRequirement requirement = new SingleTargetAndEmptyRequirement(MinRange, MaxRange);
        requirement.Hexagon = Hexagon;

        return requirement;
    }

    public override List<Hexagon> GetAllOptions(Hexagon hexagon)
    {
        List<Hexagon> hexagons = GetEveryHexagonInAnRadius(hexagon.CubeCoord, MinRange, MaxRange);

        foreach (var radiusHexagon in hexagons.ToArray())
        {
            if (radiusHexagon.IsTaken) hexagons.Remove(hexagon);
        }

        return hexagons;
    }

    public override bool IsValid(Hexagon hexagon, Unit unit)
    {
        return unit.Cell.GetDistance(hexagon) >= MinRange && unit.Cell.GetDistance(hexagon) <= MaxRange && !hexagon.IsTaken;
    }

    protected override InputType GetInputType() => InputType.SingleTargetAndEmptyRequirement;
}
