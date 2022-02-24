using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;

public class PushSpellComponent : SpellComponent
{
    [SerializeField]
    public int Force = 1;

    protected override SpellComponentType GetType() => SpellComponentType.Damage;


    protected override void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        foreach (var target in currentSpellEffects.TargetedHexs)
        {
            if(target.CurrentUnit && target != currentSpellEffects.Caster.Cell)
            {
                MoveTargetConsequence pushTargetConsequence = new MoveTargetConsequence();

                pushTargetConsequence.Caster = currentSpellEffects.Caster;
                pushTargetConsequence.Target = target.CurrentUnit;

                if (currentSpellEffects.UseSpellAnchorForDirection)
                {
                    pushTargetConsequence.Direction = target.CubeCoord - currentSpellEffects.SpellAnchor.CubeCoord;
                }
                else
                {
                    pushTargetConsequence.Direction = target.CubeCoord - ((Hexagon)currentSpellEffects.Caster.Cell).CubeCoord;
                }
                pushTargetConsequence.Force = Force;

                consequences.Add(pushTargetConsequence);
            }

            if (target.OnFire && Force >= 2)
            {
                ExtinguishFireConsequence consequence = new ExtinguishFireConsequence();
                consequence.Hexagon = target;
                consequences.Add(consequence);
            }
        }


    }
}
