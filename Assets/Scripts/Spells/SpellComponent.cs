using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellComponentType
{
    Invalid = -1,

    PostDamage,
    Damage,
    PreDamage,
    PostPostRegion,
    PostRegion,
    Region,
    PreRegion,
    Setup,

    Count
}

public abstract class SpellComponent : MonoBehaviour
{
    [SerializeField]
    public SpellComponent Next;

    public SpellComponentType Type
    {
        get => GetType();
    }

    protected abstract SpellComponentType GetType();

    public void Cast(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index)
    {
        CastInternal(input, ref currentSpellEffects, consequences, index);
        if (Next)
        {
            Next.Cast(input, ref currentSpellEffects, consequences, index +1);
        }
    }
    protected abstract void CastInternal(Dictionary<int, InputRequirement> input, ref SpellInformation currentSpellEffects, List<Consequences> consequences, int index);

    #region [Linking]

    public SpellComponent LinkComponent(SpellComponent spellComponent)
    {
        if ((int)Type < (int)spellComponent.Type)
        {
            spellComponent.Next = this;
            return spellComponent;
        }

        if (Next)
        {
            SpellComponent returnedComponent = Next.LinkComponent(spellComponent);
            if (returnedComponent) Next = returnedComponent;
        }
        else Next = spellComponent;

        return null;
    }

    public void UnlinkComponent(SpellComponent spellComponent)
    {
        if(Next == spellComponent)
        {
            Next = Next.Next;
        }

        if(Next)
        {
            Next.UnlinkComponent(spellComponent);
        }
    }

    #endregion [Linking]

    #region [Input]
    public Dictionary<int, InputRequirement> GetInputRequirements()
    {
        Dictionary<int, InputRequirement> inputRequirements = new Dictionary<int, InputRequirement>();
        return GetInputRequirementsInternal(inputRequirements, 0);
    }

    protected virtual Dictionary<int, InputRequirement> GetInputRequirements(Dictionary<int, InputRequirement> inputRequirements, int index)
    {
        return inputRequirements;
    }

    protected Dictionary<int, InputRequirement> GetInputRequirementsInternal(Dictionary<int, InputRequirement> inputRequirements, int index)
    {
        inputRequirements = GetInputRequirements(inputRequirements, index);
        if (Next) inputRequirements = Next.GetInputRequirementsInternal(inputRequirements, index+1);
        return inputRequirements;
    }
    #endregion [Input]
}
