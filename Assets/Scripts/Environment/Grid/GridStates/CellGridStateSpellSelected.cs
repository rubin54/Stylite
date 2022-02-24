using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid.GridStates;
using Grid;
using Cells;
using Units;
using System;

public class CellGridStateSpellSelected : CellGridState
{
    private Unit caster;

    private List<Hexagon> markedHexagons;

    private List<Hexagon> currentSpellConsequenceMarkings;

    private SpellTemplate currentSpell;

    public CellGridStateSpellSelected(CellGrid cellGrid, List<Hexagon> markedHexagons, Unit unit, SpellTemplate spell) : base(cellGrid)
    {
        _cellGrid = cellGrid;
        this.markedHexagons = markedHexagons;
        caster = unit;
        currentSpell = spell;
    }

    public override void OnStateEnter()
    {
        foreach (var hexagon in markedHexagons)
        {
            currentSpellConsequenceMarkings = new List<Hexagon>();

            hexagon.MarkAsHighlightedSpells();
            //hexagon.MarkAsHighlighted();
        }
    }

    public override void OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, caster);
        }
    }

    public override void OnStateExit()
    {
        foreach (var hexagon in markedHexagons)
        {
            hexagon.UnMark();
        }
    }

    public override void OnCellDeselected(Cell cell)
    {
        foreach (var hexagon in currentSpellConsequenceMarkings.ToArray())
        {
            if (markedHexagons.Contains(hexagon))
            {
                hexagon.MarkAsHighlighted();
            }
            else
            {
                hexagon.UnMark();
            }

            currentSpellConsequenceMarkings.Remove(hexagon);
        }


        if(markedHexagons.Contains((Hexagon)cell))
        {
            cell.MarkAsHighlighted();
        }
        else
        {
            cell.UnMark();
        }
    }

    public override void OnCellSelected(Cell cell)
    {
        Dictionary<int, InputRequirement> requirements = currentSpell.GetInputRequirements();

        if (requirements.Count == 1)
        {
            bool valid = true;
            foreach (var requirement in requirements)
            {
                if(!requirement.Value.IsValid((Hexagon)cell, caster))
                {
                    valid = false;
                }

                requirement.Value.Hexagon = (Hexagon)cell;
            }

            if(valid)
            {
                SpellInformation information = new SpellInformation(currentSpell, caster);

                currentSpell.Cast(caster, information, requirements);

                foreach (var hexagon in information.TargetedHexs)
                {
                    currentSpellConsequenceMarkings.Add(hexagon);
                    hexagon.MarkAsPath();
                }
            }
        }

        if(markedHexagons.Contains((Hexagon)cell))
        {
            cell.MarkAsPath();
            return;
        }

        cell.MarkAsHighlighted();
    }

    public override void OnUnitClicked(Unit unit)
    {
        if (caster == unit && !markedHexagons.Contains((Hexagon)caster.Cell))
        {
            _cellGrid.CellGridState = new CellGridStateUnitSelected(_cellGrid, unit);
            return;
        }

        _cellGrid.OnCellClicked(unit.Cell, new EventArgs());
    }

    public override void OnUnitSelected(Unit unit)
    {
        OnCellSelected(unit.Cell);
    }

    public override void OnUnitDeselected(Unit unit)
    {
        OnCellDeselected(unit.Cell);
    }
}
