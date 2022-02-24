using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Action<Controller> FinishedTurn;
    public Action<Controller> Died;

    [SerializeField]
    public Unit Unit;

    protected CellGrid cellGrid;

    public void Setup(CellGrid cellGrid, SpellCastTab spellTab, UnitInfoTab unitInfoTab, RestingInput restingInput)
    {
        this.cellGrid = cellGrid;
        Unit.Died += OnDeath;
        OnSetup(cellGrid, spellTab, unitInfoTab, restingInput);
        FinishedTurn += OnFinishedAction;
    }

    protected virtual void OnSetup(CellGrid cellGrid, SpellCastTab spellCastTab, UnitInfoTab unitInfoTab, RestingInput restingInput)
    {

    }

    public abstract void StartAction();

    public abstract void Act();

    public abstract void OnFinishedAction(Controller controller);


    public int GetFollowingLowestInputStep(int value, Dictionary<int, InputRequirement> requirements)
    {
        int lowestInput = int.MaxValue;

        foreach (var requirement in requirements)
        {
            if (lowestInput > requirement.Key && requirement.Key > value) lowestInput = requirement.Key;
        }

        return lowestInput;
    }

    public virtual void OnUnitDeath(Unit unit)
    {

    }

    public virtual bool IsPlayer() => false;
    public virtual bool IsEnemy() => false;

    public void OnDeath(Unit unit)
    {
        Died?.Invoke(this);
    }
}
