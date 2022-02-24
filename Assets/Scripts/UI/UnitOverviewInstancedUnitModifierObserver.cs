using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitOverviewInstancedUnitModifierObserver : MonoBehaviour
{
    [SerializeField]
    public UnitOverview editor;

    private ModifierTemplate[] modifiers = new ModifierTemplate[3];
    public List<ModifierSocket> modifierSockets;

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
        for (int i = 0; i < editor.InstancedUnit.Modifiers.Length - 1; i++)
        {
            if (editor.InstancedUnit.Modifiers != null)
            {
                if (modifierSockets.Count > i && editor.InstancedUnit.Modifiers[i] != null)
                {
                    ModifierTemplate spell = editor.InstancedUnit.Modifiers[i].Duplicate();
                    modifierSockets[i].AddGrabbable(spell);
                    spell.OnStoppedMoving();
                    modifiers[i] = spell;
                }
            }
        }
    }
}
