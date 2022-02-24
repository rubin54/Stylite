using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetEmptyHexagonInputSpellComponent : SpellComponent
{
    public int MinRange = 3;
    public int MaxRange = 5;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        currentSpellEffects.TargetedHexs.Add(((SingleTargetRequirement)input[index]).Hexagon);
    }

    protected override Dictionary<int, InputRequirement> GetInputRequirements(Dictionary<int, InputRequirement> inputRequirements, int index)
    {
        inputRequirements.Add(index, new SingleTargetAndEmptyRequirement(MinRange, MaxRange));
        return inputRequirements;
    }

    protected override SpellComponentType GetType() => SpellComponentType.PreRegion;
}
