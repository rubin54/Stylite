using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOverviewInstancedUnitSpellObserver : MonoBehaviour
{
    [SerializeField]
    public UnitOverview editor;

    private SpellTemplate[] spells = new SpellTemplate[3];
    public List<SpellSocket> spellSockets;

    private void OnEnable()
    {
        editor.AddedUnit += OnAddedUnit;

        if (editor.Unit)
        {
            OnAddedUnit();
        }
    }

    public void OnAddedUnit()
    {
        for (int i = 0; i < editor.InstancedUnit.Spells.Length; i++)
        {
            if (editor.InstancedUnit.Spells != null)
            {
                if (spellSockets.Count >= i && editor.InstancedUnit.Spells[i] != null)
                {
                    SpellTemplate spell = editor.InstancedUnit.Spells[i].Duplicate();
                    spellSockets[i].AddGrabbable(spell);
                    spell.OnStoppedMoving();
                    spells[i] = spell;
                }
            }
        }
    }
}
