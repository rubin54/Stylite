using Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SpellTemplate : Grabbable
{
    [SerializeField]
    private int apCost = 3;

    [SerializeField]
    public Unit.UnitType Type;

    public int ApCost
    {
        get => apCost;
    }

    [SerializeField]
    public bool UseSpellAnchorForDirection = true;

    [SerializeField]
    public GameObject Effect;

    public AK.Wwise.Event SoundSFX;
    

    [SerializeField]
    public string Name;

    [SerializeField]
    public SpellComponent Next;

    public UnitTemplate Owner;

    public Dictionary<int, InputRequirement> GetInputRequirements()
    {
        if (Next)
        {
            return Next.GetInputRequirements();
        }
        return null;
    }

    public void Act(Unit caster, Dictionary<int, InputRequirement> inputRequirements)
    {
        int endCost = apCost;
        if (Type == Unit.UnitType.Universal) endCost -= 1;
        if(caster.Type != Type) endCost++;

        if(!caster.ReduceActionPoints(endCost)) return;

        SpellInformation spellInformation = new SpellInformation(this, caster);

        List<Consequences> consequences = Cast(caster, spellInformation, inputRequirements);

        foreach (var consequence in consequences)
        {
            consequence.Act();

            foreach (var hexagon in spellInformation.EffectPositions)
            {
                if (Effect && EffectSocket.Instance)
                {
                    EffectSocket.Instance.CreateEffect(Effect, hexagon.gameObject.transform.position);
                    SoundSFX.Post(gameObject);
                }
            }
        }
    }

    public List<Consequences> Cast(Unit caster, Dictionary<int, InputRequirement> inputRequirements)
    {
        return Cast(caster, new SpellInformation(this,caster), inputRequirements);
    }

    public List<Consequences> Cast(Unit caster, SpellInformation spellInformation, Dictionary<int, InputRequirement> inputRequirements)
    {
        List<Consequences> consequences = new List<Consequences>();
        Next?.Cast(inputRequirements, ref spellInformation, consequences, 0);
        return consequences;
    }

    public SpellTemplate Duplicate()
    {
        GameObject duplicate = Instantiate(gameObject);
        SpellTemplate spellTemplate = duplicate.GetComponent<SpellTemplate>();
        spellTemplate.Effect = Effect;
        spellTemplate.Name = Name;
        //this could be an big yikes, but i hope it works.
        spellTemplate.Next = Next;

        return spellTemplate;
    }

    public bool Contains(ModifierTemplate modifier)
    {
        SpellComponent currentComponent = Next;

        while(Next != null)
        {
            if (Next = modifier.SpellComponent) return true;
            Next = Next.Next;
        }

        return false;
    }

    public void LinkComponent(SpellComponent spellComponent)
    {
        if (Next)
        {
            SpellComponent returnedComponent = Next.LinkComponent(spellComponent);
            if (returnedComponent) Next = returnedComponent;
        }
        else Next = spellComponent;
    }

    public void UnlinkComponent(SpellComponent spellComponent)
    {
        if(Next == spellComponent)
        {
            Next = Next.Next;
            return;
        }

        if (Next)
        {
            Next.UnlinkComponent(spellComponent);
        }
        else Debug.LogError("Unlinked nonexisting Spell Component in SpellComponent/UnlinkComponent");
    }

    public override Content GetContent()
    {
        string apCost = ApCost.ToString();

        if(Owner)
        {
            if (Owner.Type != Type && Type != Unit.UnitType.Universal)
            {
                apCost += " (+1)";
            }
        }

        Content content = new Content(Name, "Type: " + UnitTemplate.GetUnitTypeAsString(Type) + "\nAp Cost: " + apCost + '\n' + bodyDescription);
        return content;
    }


}
