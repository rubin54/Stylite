using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class LinearInputRequirement : InputRequirement
{
    public LinearInputRequirement(int minRange, int maxRange)
    {
        MinRange = minRange;
        MaxRange = maxRange;
    }

    public override InputRequirement Duplicate()
    {
        LinearInputRequirement requirement = new LinearInputRequirement(MinRange, MaxRange);
        requirement.Hexagon = Hexagon;

        return requirement;
    }

    public override List<Hexagon> GetAllOptions(Hexagon hexagon)
    {
        List<Hexagon> hexagons = new List<Hexagon>();

        foreach (var direction in Hexagon._directions)
        {
            Vector3 currentPoint = hexagon.CubeCoord + (direction * (MinRange-1));

            for (int i = 0; i < MaxRange - MinRange +1; i++)
            {
                currentPoint += direction;

                Hexagon foundHexagon = CellGrid.Instance.GetHexagon(currentPoint);

                if(foundHexagon != null)
                {
                    hexagons.Add(foundHexagon);
                }
            }
        }

        return hexagons;
    }

    public override bool IsValid(Hexagon hexagon, Unit unit)
    {
        Vector3 direction = ((Hexagon)unit.Cell).CubeCoord - hexagon.CubeCoord;

        List<Vector3> directions = DisectDirection(direction);

        if (directions.Count == 0) return true;

        Vector3 normalDirection = directions[0];

        foreach (var splitDirection in directions)
        {
            if (!splitDirection.Equals(normalDirection)) return false;
        }

        return unit.Cell.GetDistance(hexagon) >= MinRange && unit.Cell.GetDistance(hexagon) <= MaxRange;
    }

    protected override InputType GetInputType() => InputType.SingleTargetLinearRequirement;

    public List<Vector3> DisectDirection(Vector3 direction)
    {
        List<Vector3> directions = new List<Vector3>();


        Dictionary<Vector3, int> howOftenDoesSomethingFit = new Dictionary<Vector3, int>();


        foreach (var simpleDirection in Hexagon._directions)
        {
            bool compatible = direction.x * simpleDirection.x >= 0 &&
                                direction.y * simpleDirection.y >= 0 &&
                                direction.z * simpleDirection.z >= 0;

            if (!compatible) continue;

            int lowestAbsoluteNumber = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                if (simpleDirection[i].Equals(0)) continue;

                if (lowestAbsoluteNumber > (direction[i] / simpleDirection[i]))
                {
                    lowestAbsoluteNumber = Mathf.RoundToInt(direction[i] / simpleDirection[i]);
                }
            }

            howOftenDoesSomethingFit.Add(simpleDirection, lowestAbsoluteNumber);
        }


        foreach (var directionSize in howOftenDoesSomethingFit)
        {
            for (int i = 0; i < directionSize.Value; i++)
            {
                directions.Add(directionSize.Key);
            }
        }


        if (directions.Count == 0)
        {
            directions.Add(new Vector3(0, 0, 0));
        }

        return directions;
    }

}
