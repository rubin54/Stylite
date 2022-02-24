using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTileWithoutUnitRequirement : SpellComponent
{
    public int MinRange = 1;
    public int MaxRange = 1;

    public bool HasUnit = true;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        SingleTargetAndContainsUnitRequirement Input = (SingleTargetAndContainsUnitRequirement)input[index];
        currentSpellEffects.TargetedHexs.Add(Input.Hexagon);
    }

    protected override Dictionary<int, InputRequirement> GetInputRequirements(Dictionary<int, InputRequirement> inputRequirements, int index)
    {
        inputRequirements.Add(index, new SingleTargetAndContainsUnitRequirement(MinRange, MaxRange, HasUnit));
        return inputRequirements;
    }

    protected override SpellComponentType GetType() => SpellComponentType.PreRegion;
}
