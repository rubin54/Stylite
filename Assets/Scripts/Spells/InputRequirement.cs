using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;


public enum InputType
{
    Invalid = -1,

    SingleTargetRequirement,
    SingleTargetAndEmptyRequirement,
    SingleTargetAndContainsUnitRequirement,
    SingleTargetLinearRequirement,
 
    Count
}

public abstract class InputRequirement
{
    public int MinRange = 3;
    public int MaxRange = 8;

    public Hexagon Hexagon;

    public InputType Type => GetInputType();

    protected abstract InputType GetInputType();

    public abstract InputRequirement Duplicate();

    public abstract bool IsValid(Hexagon hexagon, Unit unit);

    public abstract List<Hexagon> GetAllOptions(Hexagon hexagon);


    public List<Hexagon> GetEveryHexagonInAnRadius(Vector3 position, int minRadius, int maxRadius)
    {
        List<Hexagon> retVal = new List<Hexagon>();

        Vector3[] directions = Hexagon._directions;


        for (int i = minRadius; i <= maxRadius; i++)
        {
            for (int sideCount = 0; sideCount < 6; sideCount++)
            {
                for (int innerSideCount = 0; innerSideCount <= i; innerSideCount++)
                {
                    Hexagon cell = CellGrid.Instance.GetHexagon(new Vector3(
                        directions[sideCount].x * i + directions[(sideCount + 2) % 6].x * innerSideCount + position.x,
                        directions[sideCount].y * i + directions[(sideCount + 2) % 6].y * innerSideCount + position.y,
                        directions[sideCount].z * i + directions[(sideCount + 2) % 6].z * innerSideCount + position.z
                        ));

                    if (cell)
                    {
                        retVal.Add(cell);
                    }
                }
            }
        }

        return retVal;
    }

    public static Dictionary<int, InputRequirement> DuplicateAll(Dictionary<int, InputRequirement> requirements)
    {
        Dictionary<int, InputRequirement> retVal = new Dictionary<int, InputRequirement>();

        foreach (var requirement in requirements)
        {
            retVal.Add(requirement.Key, requirement.Value);
        }

        return retVal;
    }
}
