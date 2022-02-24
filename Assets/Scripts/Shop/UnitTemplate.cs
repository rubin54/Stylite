using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;
using System;

public class UnitTemplate : Grabbable
{
    public Action<UnitTemplate> Died;

    [SerializeField]
    public string Name = "Krispin";

    [SerializeField]
    public static int MaxSpellCount = 3;

    [SerializeField]
    public Unit.UnitType Type;

    [SerializeField]
    public int Health = 20;

    [SerializeField]
    public int FieldOfView = 5;

    [SerializeField]
    public int BaseInitiative = 0;

    [SerializeField]
    public int ActionPoints = 7; 

    private SpellTemplate[] spells = new SpellTemplate[MaxSpellCount];
    private ModifierTemplate[] modifiers = new ModifierTemplate[MaxSpellCount];

    public SpellTemplate[] Spells
    {
        get => spells;
    }

    public ModifierTemplate[] Modifiers
    {
        get => modifiers;
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
        if(gameObject)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpell(SpellTemplate spell, int index)
    {
        spells[index] = spell;

        if(spell)
        {
            spell.Owner = this;
        }

        CountNonNullSpell();
    }

    public void RemoveSpell(int index)
    {
        if(spells != null)
        {
            if (spells[index])
            {
                spells[index].Owner = null;
            }

            spells[index] = null;
        }


        CountNonNullSpell();
    }

    public void AddModifier(ModifierTemplate modifier, int index)
    {
        if(spells[index])
        {
            spells[index].LinkComponent(modifier.SpellComponent);
            modifiers[index] = modifier;
        }
    }

    public void RemoveModifier(ModifierTemplate modifier, int index)
    {
        if(spells[index])
        {
            spells[index].UnlinkComponent(modifier.SpellComponent);
            modifiers[index] = null;
        }
    }

    private void CountNonNullSpell()
    {
        int spellCount = 0;


        if(spells != null)
        {
            foreach (var spell in spells)
            {
                if (spell != null) spellCount++;
            }
        }
    }

    static public string GetUnitTypeAsString(Unit.UnitType unittype)
    {
        string name = "";
        switch (unittype)
        {
            case Unit.UnitType.Melee:
                name = "Melee";
                break;
            case Unit.UnitType.Range:
                name = "Ranged";
                break;
            case Unit.UnitType.Support:
                name = "Support";
                break;
            case Unit.UnitType.Universal:
                name = "Universal";
                break;
            default:
                name = "Somebody forgot to choose a class...";
                break;
        }

        return name;
    }

    static public string GetClassAsString(UnitTemplate unit)
    {
        return GetUnitTypeAsString(unit.Type);
    }
}
