using Cells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsequenceType
{
    Invalid = -1,

    MovementConsequence,
    DamageTargetConsequence,
    MoveConsequence,
    ChargeFireConsequence,
    PlaceFireConsequence,
    ExtinguishFireConsequence,
    SwitchCasterWithFirstTargetConsequence,

    Count
}

public abstract class Consequences
{
    public ConsequenceType Type
    {
        get => GetConsequenceType();
    }

    public List<Hexagon> Act()
    {
        List < Hexagon > hexagons = new List<Hexagon>();
        Act(hexagons);
        return hexagons;
    }

    protected abstract void Act(List<Hexagon> hexagons);


    protected abstract ConsequenceType GetConsequenceType();

    public abstract float CalculateImportance();
}
