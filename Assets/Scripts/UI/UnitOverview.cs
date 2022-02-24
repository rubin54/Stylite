using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class UnitOverview : MonoBehaviour
{
    public Action AddedUnit;
    public Action<UnitOverview, UnitTemplate> RemovedUnit;

    [SerializeField]
    protected List<SpellSocket> spellSockets;

    [SerializeField]
    protected List<ModifierSocket> modifierSockets;

    [SerializeField]
    public SocketCreator SpellSocketCreator;

    [SerializeField]
    public SocketCreator ModifierSocketCreator;

    public UnitTemplate Unit;

    public Unit InstancedUnit;

    protected Dictionary<int, (bool, ModifierTemplate)> currentModifiers = new Dictionary<int, (bool, ModifierTemplate)>();


    public void AddSpell(SpellTemplate spell)
    {
        foreach (var socket in spellSockets)
        {
            if ((socket.grabbable == null))
            {
                socket.AddGrabbable(spell);
                spell.StoppedMoving?.Invoke(spell);
                return;
            }
        }
    }

    public void AddModifier(ModifierTemplate modifier)
    {
        foreach (var socket in modifierSockets)
        {
            if ((socket.grabbable == null))
            {
                socket.AddGrabbable(modifier);
                modifier.StoppedMoving?.Invoke(modifier);
                return;
            }
        }
    }

    public virtual void Setup(UnitTemplate unit, Unit instancedUnit)
    {
        Unit = unit;
        InstancedUnit = instancedUnit;

        foreach (SpellSocket socket in spellSockets)
        {
            SpellSocketCreator.CreatedSocket?.Invoke(socket);
            socket.AddedSpell += OnAddedSpell;
            socket.RemovedSpell += OnRemovedSpell;
        }

        foreach (ModifierSocket modifier in modifierSockets)
        {
            ModifierSocketCreator.CreatedSocket?.Invoke(modifier);
            modifier.AddedModifier += OnAddedModifier;
            modifier.RemovedModifier += OnRemovedModifier;
        }

        AddedUnit?.Invoke();
    }

    public virtual void Destroy()
    {
        foreach (SpellSocket socket in spellSockets)
        {
            SpellSocketCreator.DestroyedSocket?.Invoke(socket);
        }

        foreach (ModifierSocket modifier in modifierSockets)
        {
            ModifierSocketCreator.DestroyedSocket?.Invoke(modifier);
        }
    }

    public virtual void OnAddedSpell(SpellTemplate spell, int index)
    {

    }

    public virtual void OnRemovedSpell(int index)
    {

    }

    public virtual void OnAddedModifier(ModifierTemplate modifier, int index)
    {

    }

    public virtual void OnRemovedModifier(ModifierTemplate modifier, int index)
    {

    }
}
