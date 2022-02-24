using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRearHexesSpellComponent : SpellComponent
{
    [SerializeField]
    int Length = 1;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        List<Vector3> disectedDirections = DisectDirection(currentSpellEffects.SpellAnchor.CubeCoord - ((Hexagon)currentSpellEffects.Caster.Cell).CubeCoord);
        Vector3 finalPosition = currentSpellEffects.SpellAnchor.CubeCoord;

        for (int i = 0; i < Length; i++)
        {
            Vector3 calculatedPosition = finalPosition + disectedDirections[i % disectedDirections.Count];

            finalPosition = calculatedPosition;
            Hexagon calculatedHexagon = CellGrid.Instance.GetHexagon(calculatedPosition);

            if (calculatedHexagon == null) continue;
            if(!currentSpellEffects.TargetedHexs.Contains(calculatedHexagon))
            {
                currentSpellEffects.TargetedHexs.Add(calculatedHexagon);
            }
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostPostRegion;


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
