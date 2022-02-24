using Cells;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public struct SpellInformation
{
    public Unit Caster;
    public SpellTemplate Spell;
    public List<Unit> HitUnits;
    public Hexagon SpellAnchor;
    public List<Hexagon> TargetedHexs;
    public List<Hexagon> EffectPositions;
    public bool UseSpellAnchorForDirection;

   public SpellInformation(SpellTemplate spell, Unit caster)
    {
        Caster = caster;
        Spell = spell;
        SpellAnchor = (Hexagon)caster.Cell;
        HitUnits = new List<Unit>();
        TargetedHexs = new List<Hexagon>();
        EffectPositions = new List<Hexagon>();
        UseSpellAnchorForDirection = spell.UseSpellAnchorForDirection;
    }
}
