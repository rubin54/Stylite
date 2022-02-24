using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSingleTileSpellComponent : SpellComponent
{
    [SerializeField]
    private int minRange = 0;

    [SerializeField]
    private int maxRange = 5;

    protected override SpellComponentType GetType() => SpellComponentType.Region;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        currentSpellEffects.TargetedHexs.Add(((SingleTargetRequirement)input[index]).Hexagon);
    }

    protected override Dictionary<int, InputRequirement> GetInputRequirements(Dictionary<int, InputRequirement> inputRequirements, int index)
    {
        inputRequirements.Add(index, new SingleTargetRequirement(minRange, maxRange));

        return inputRequirements;
    }

}
