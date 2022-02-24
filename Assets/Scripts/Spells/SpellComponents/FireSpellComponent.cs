using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpellComponent : SpellComponent
{
    [SerializeField]
    private int neededCharges = 2;

    private int currentCharges = 1;

    private int currentIndex = 0;

    protected override void CastInternal(Dictionary<int, InputRequirement> input,ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        if(currentCharges >= neededCharges)
        {
            foreach (var hexagon in currentSpellEffects.TargetedHexs)
            {
                if (!hexagon.isFlamable) continue;
                LightHexagonOnFireConsequence lightConsequence = new LightHexagonOnFireConsequence();
                lightConsequence.Caster = currentSpellEffects.Caster;
                lightConsequence.Component = this;
                lightConsequence.Hexagon = hexagon;
                consequences.Add(lightConsequence);
            }
        }
        else
        {
            ChargeFireConsequence chargeConsequence = new ChargeFireConsequence();
            chargeConsequence.Component = this;
            chargeConsequence.Amount = 1;
            chargeConsequence.Index = currentIndex;
            consequences.Add(chargeConsequence);
        }
    }

    public void IncreaseCharges(int index, int amount)
    {
        if(currentIndex == index)
        {
            currentCharges += amount;
            currentIndex++;
        }
    }

    public void ResetCharges()
    {
        currentCharges = 1;
    }

    protected override SpellComponentType GetType() => SpellComponentType.PostDamage;
}
