using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class SpellCastTab : MonoBehaviour
{
    public Action<SpellTemplate> SelectedSpell;

    private SpellTemplate[] spells = new SpellTemplate[3];
    public SpellTemplate[] Spells { get => spells; }

    private ModifierTemplate[] modifiers = new ModifierTemplate[3];
    public ModifierTemplate[] Modifiers { get => modifiers; }

    public List<SpellSocket> spellSockets;
    public List<ModifierSocket> modifierSockets;

    public Unit CurrentDisplayedUnit;

    private void Start()
    {
        foreach (var socket in spellSockets)
        {
            socket.ClickedGrabbable += OnSpellSelection;
        }
    }

    private void Update()
    {
        if(CurrentDisplayedUnit)
        {
            if(spellSockets[0].grabbable)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {

                }
            }

            foreach (var socket in spellSockets)
            {
                if(socket.grabbable)
                {
                    float endCost = ((SpellTemplate)socket.grabbable).ApCost;
                    if (((SpellTemplate)socket.grabbable).Type == Unit.UnitType.Universal) endCost -= 1;
                    if (CurrentDisplayedUnit.Type != ((SpellTemplate)socket.grabbable).Type) endCost++;

                    if(endCost > CurrentDisplayedUnit.ActionPoints)
                    {
                        socket.grabbable.Deactivate();
                    }
                }
            }
        }
    }

    public void OnUnitSelection(Unit unit)
    {
        for(int i = 0; i < unit.Spells.Length; i++)
        {
            if(unit.Spells != null)
            {
                if (spellSockets.Count >= i && unit.Spells.Length > i && unit.Spells[i] != null)
                {
                    SpellTemplate spell = unit.Spells[i].Duplicate();
                    spellSockets[i].AddGrabbable(spell);
                    spell.OnStoppedMoving();
                    spells[i] = unit.Spells[i];
                }
            }

            if (unit.Modifiers != null)
            {
                if (modifierSockets.Count >= i && unit.Modifiers.Length > i && unit.Modifiers[i] != null)
                {
                    ModifierTemplate modifier = unit.Modifiers[i].Duplicate();
                    modifierSockets[i].AddGrabbable(modifier);
                    modifier.OnStoppedMoving();
                    modifiers[i] = unit.Modifiers[i];
                }
            }
        }

        CurrentDisplayedUnit = unit;
    }

    public void OnUnitDeselection(Unit unit)
    {
        foreach (var socket in spellSockets)
        {
            if(socket.grabbable)
            {
                var removedGrabbable = socket.RemoveGrabbable();
                removedGrabbable.Destroyed?.Invoke(removedGrabbable);
                Destroy(removedGrabbable.gameObject);
            }
        }

        foreach (var socket in modifierSockets)
        {
            if (socket.grabbable)
            {
                var removedGrabbable = socket.RemoveGrabbable();
                removedGrabbable.Destroyed?.Invoke(removedGrabbable);
                Destroy(removedGrabbable.gameObject);
            }
        }
    }

    public void OnSpellSelection(GrabbableSocket socket, Grabbable spell)
    {
        SelectedSpell?.Invoke((SpellTemplate)spell);
    }

    public void DeactivateSpell(SpellTemplate spell)
    {
        foreach (var socket in spellSockets)
        {
            if(socket.grabbable == spell)
            {
                socket.grabbable.Deactivate();
            }
        }
    }


    public void OnAddedSpell(SpellTemplate spell, int index)
    {
    }

    public void OnRemovedSpell(int index)
    {

    }
}
