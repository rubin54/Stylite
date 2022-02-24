using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHexagonsAsTargetAroundTheAnchorSpellComponent : SpellComponent
{
    [SerializeField]
    public int startRadius = 1;

    [SerializeField]
    public int endRadius = 1;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        List<Hexagon> newHexagons = GetEveryHexagonInAnRadius(currentSpellEffects.SpellAnchor.CubeCoord, startRadius, endRadius);
        foreach (var hexagon in newHexagons)
        {
            if(!currentSpellEffects.TargetedHexs.Contains(hexagon))
            {
                currentSpellEffects.TargetedHexs.Add(hexagon);
            }
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostRegion;

    //Codeduplication *~*
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

}
