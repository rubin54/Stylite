using Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullSpellComponent : SpellComponent
{
    [SerializeField]
    public int Force = 1;

    protected override SpellComponentType GetType() => SpellComponentType.Damage;

    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        foreach (var target in currentSpellEffects.TargetedHexs)
        {
            if (target.CurrentUnit)
            {
                MoveTargetConsequence pushTargetConsequence = new MoveTargetConsequence();

                pushTargetConsequence.Caster = currentSpellEffects.Caster;
                pushTargetConsequence.Target = target.CurrentUnit;
                if(currentSpellEffects.UseSpellAnchorForDirection)
                {
                    pushTargetConsequence.Direction = currentSpellEffects.SpellAnchor.CubeCoord - target.CubeCoord;
                }
                else
                {
                    pushTargetConsequence.Direction = ((Hexagon)currentSpellEffects.Caster.Cell).CubeCoord - target.CubeCoord;
                }
                pushTargetConsequence.Force = Force;

                consequences.Add(pushTargetConsequence);
            }

            if(target.OnFire && Force >= 2)
            {
                ExtinguishFireConsequence consequence = new ExtinguishFireConsequence();
                consequence.Hexagon = target;
                consequences.Add(consequence);
            }
        }
    }
}
