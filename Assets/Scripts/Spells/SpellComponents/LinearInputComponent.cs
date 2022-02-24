using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInputComponent : SpellComponent
{
    public int MinRange = 3;
    public int MaxRange = 5;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        currentSpellEffects.TargetedHexs.Add(((LinearInputRequirement)input[index]).Hexagon);
    }

    protected override Dictionary<int, InputRequirement> GetInputRequirements(Dictionary<int, InputRequirement> inputRequirements, int index)
    {
        inputRequirements.Add(index, new LinearInputRequirement(MinRange, MaxRange));
        return inputRequirements;
    }

    protected override SpellComponentType GetType() => SpellComponentType.PreRegion;
}
