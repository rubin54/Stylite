using Cells;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SingleTargetAndContainsUnitRequirement : InputRequirement
{
    public bool ContainsUnit = false;

    public SingleTargetAndContainsUnitRequirement(int minRange, int maxRange, bool containsUnit)
    {
        MinRange = minRange;
        MaxRange = maxRange;
        ContainsUnit = containsUnit;
    }

    public override InputRequirement Duplicate()
    {
        SingleTargetAndContainsUnitRequirement requirement = new SingleTargetAndContainsUnitRequirement(MinRange, MaxRange, ContainsUnit);
        requirement.Hexagon = Hexagon;

        return requirement;
    }

    public override List<Hexagon> GetAllOptions(Hexagon hexagon)
    {
        List<Hexagon> hexagons = GetEveryHexagonInAnRadius(hexagon.CubeCoord, MinRange, MaxRange);

        foreach (var radiusHexagon in hexagons.ToArray())
        {
            if ((radiusHexagon.CurrentUnit == null) != ContainsUnit) hexagons.Remove(hexagon);
        }

        return hexagons;
    }

    public override bool IsValid(Hexagon hexagon, Unit unit)
    {
        return unit.Cell.GetDistance(hexagon) >= MinRange && unit.Cell.GetDistance(hexagon) <= MaxRange && ((hexagon.CurrentUnit != null) == ContainsUnit);
    }

    protected override InputType GetInputType() => InputType.SingleTargetAndContainsUnitRequirement;
}
