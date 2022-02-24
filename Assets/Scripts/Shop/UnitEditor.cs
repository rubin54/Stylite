using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class UnitEditor : UnitOverview
{
    public void Destroy()
    {
        foreach (var socket in spellSockets)
        {
            socket.Destroy();
        }

        foreach (var socket in modifierSockets)
        {
            socket.Destroy();
        }

        Destroy(gameObject);
    }

    public override void OnAddedSpell(SpellTemplate spell, int index)
    {
        Unit.SetSpell(spell, index);

        if(currentModifiers.ContainsKey(index))
        {
            if (currentModifiers[index].Item2 == null) return;

            Unit.AddModifier(currentModifiers[index].Item2, index);
            currentModifiers[index] = (true, currentModifiers[index].Item2);
        }
        else
        {
            currentModifiers.Add(index, (false, null));
        }
    }

    public override void OnRemovedSpell(int index)
    {
        if (currentModifiers.ContainsKey(index))
        {
            if (currentModifiers[index].Item2 != null)
            {
                Unit.RemoveModifier(currentModifiers[index].Item2, index);
                currentModifiers[index] = (false, currentModifiers[index].Item2);
            }
        }

        Unit.RemoveSpell(index);
    }

    public override void OnAddedModifier(ModifierTemplate modifier, int index)
    {
        if (currentModifiers.ContainsKey(index))
        {
            Unit.AddModifier(modifier, index);
            currentModifiers[index] = (true, modifier);
        }
        else
        {
            currentModifiers.Add(index, (false, modifier));
        }
    }

    public override void OnRemovedModifier(ModifierTemplate modifier, int index)
    {
        if(modifier)
        {
            if (currentModifiers.ContainsKey(index))
            {
                if (currentModifiers[index].Item1)
                {
                    Unit.RemoveModifier(modifier, index);
                }
                currentModifiers.Remove(index);
            }
        }
    }


    public void OnUnitDeath()
    {

    }
}
