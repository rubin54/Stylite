using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSpellComponent : SpellComponent
{
    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        List<Hexagon> currentHexes = new List<Hexagon>();

        foreach (var targetedHex in currentSpellEffects.TargetedHexs.ToArray())
        {
            if(targetedHex.IsConductive)
            {
                currentHexes.Add(targetedHex);
            }
        }

        while(currentHexes.Count != 0)
        {
            foreach (var hexagon in currentHexes[0].GetNeighbours(CellGrid.Instance.Cells))
            {
                if(hexagon.IsConductive && !currentSpellEffects.TargetedHexs.Contains((Hexagon)hexagon))
                {
                    currentHexes.Add((Hexagon)hexagon);
                }
            }

            if(!currentSpellEffects.TargetedHexs.Contains(currentHexes[0]))
            {
                currentSpellEffects.TargetedHexs.Add(currentHexes[0]);
            }

            currentHexes.RemoveAt(0);
        }
    }

    protected override SpellComponentType GetType() => SpellComponentType.PreDamage;
}
