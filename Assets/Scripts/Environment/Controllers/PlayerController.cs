using Cells;
using Grid;
using Grid.GridStates;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class PlayerController : Controller
{
    private SpellCastTab spellCastTab;

    private UnitInfoTab unitInfoTab;

    private SpellTemplate currentSpell;

    private RestingInput restingInput;

    private Dictionary<int, InputRequirement> currentInputRequirements;

    private int currentInputStep = 0;

    private bool currentlyAwaitingInput = false;

    protected override void OnSetup(CellGrid cellGrid, SpellCastTab spellCastTab, UnitInfoTab unitInfoTab, RestingInput restingInput)
    {
        this.spellCastTab = spellCastTab;
        this.unitInfoTab = unitInfoTab;
        this.restingInput = restingInput;
    }

    public override void StartAction()
    {
        unitInfoTab.SetNewPlayer(Unit);
        restingInput.OnPlayerTurnStart();
        restingInput.AttemptedResting += Rest;

        Unit.ResetActionPoints();

        spellCastTab.OnUnitSelection(Unit);
        spellCastTab.SelectedSpell += OnSelectedSpell;

        cellGrid.CellGridState = new CellGridStateUnitSelected(cellGrid, Unit);
    }

    public override void Act()
    {
        if(Unit.ActionPoints > 0)
        {
            if (Unit.Spells[0])
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) OnSelectedSpell(Unit.Spells[0]);
            }

            if (Unit.Spells[1])
            {
                if (Input.GetKeyDown(KeyCode.Alpha2)) OnSelectedSpell(Unit.Spells[1]);
            }

            if (Unit.Spells[2])
            {
                if (Input.GetKeyDown(KeyCode.Alpha3)) OnSelectedSpell(Unit.Spells[2]);
            }

            if (currentInputRequirements != null)
            {
                if (currentInputStep != int.MaxValue)
                {
                    if (!currentlyAwaitingInput)
                    {
                        currentlyAwaitingInput = true;

                        StartInput();
                    }
                }
                else
                {
                    float apBeforeCast = Unit.ActionPoints;
                    currentSpell.Act(Unit, currentInputRequirements);
                    if(Unit.ActionPoints < apBeforeCast)
                    {
                        spellCastTab.DeactivateSpell(currentSpell);
                    }

                    currentInputRequirements = null;
                    currentInputStep = int.MinValue;
                    currentlyAwaitingInput = false;
                    cellGrid.CellGridState = new CellGridStateUnitSelected(cellGrid, Unit);
                }
            }
        }
        else
        {
            FinishedTurn?.Invoke(this);
        }
    }

    public void Rest()
    {
        Unit.Heal(Mathf.RoundToInt(Unit.ActionPoints));
        Unit.ActionPoints = 0;

        cellGrid.CellGridState = new CellGridStateBlockInput(cellGrid);
    }

    public override void OnFinishedAction(Controller controller)
    {
        //Unit.OnTurnEnd();

        //Unit.UnitClicked -= cellGrid.OnUnitClicked;
        spellCastTab.OnUnitDeselection(Unit);
        restingInput.AttemptedResting -= Rest;
        restingInput.OnPlayerTurnEnd();
        spellCastTab.SelectedSpell -= OnSelectedSpell;
        Unit.ActionPoints = -1;
    }

    public void OnSelectedSpell(SpellTemplate spell)
    {
        int apCost = spell.ApCost;
        if (spell.Type == Unit.UnitType.Universal) apCost -= 1;
        if (Unit.Type != spell.Type) apCost++;

        if (apCost > Unit.ActionPoints) return;

        currentSpell = spell;
        currentInputRequirements = currentSpell.GetInputRequirements();
        currentInputStep = int.MinValue;
        currentlyAwaitingInput = false;
        SetFollowingLowestInputStep();
    }

    public void DeselectSpell()
    {
        currentSpell = null;
        currentInputRequirements = null;
        currentInputStep = int.MinValue;
        currentlyAwaitingInput = false;
        cellGrid.ClickedCell -= OnInput;
        cellGrid.CellGridState = new CellGridStateUnitSelected(cellGrid, Unit);
    }

    #region InputSingleTarget
    private void StartInput()
    {
        if (currentInputRequirements != null && currentInputRequirements.ContainsKey(currentInputStep))
        {
            List<Hexagon> hexagons = currentInputRequirements[currentInputStep].GetAllOptions((Hexagon)Unit.Cell);

            cellGrid.CellGridState = new CellGridStateSpellSelected(cellGrid, hexagons, Unit, currentSpell);
        }

        cellGrid.ClickedCell += OnInput;
    }

    public void OnInput(Cell cell)
    {
        if (currentInputRequirements == null || !currentInputRequirements.ContainsKey(currentInputStep))
        {
            return;
        }

        Hexagon hexagon = (Hexagon)cell;
        if(currentInputRequirements[currentInputStep].IsValid(hexagon, Unit))
        {
            currentInputRequirements[currentInputStep].Hexagon = hexagon;
            StopInput();
        }
        else
        {
            DeselectSpell();
        }
    }

    private void StopInput()
    {
        cellGrid.ClickedCell -= OnInput;
        FinishInputStep();
    }
    #endregion InputSingleTarget

    private void FinishInputStep()
    {
        currentlyAwaitingInput = false;
        SetFollowingLowestInputStep();
    }

    public bool IsFinalInputStep(int inputStep)
    {
        int highestInputStep = int.MinValue;

        foreach (var requirement in currentInputRequirements)
        {
            if(highestInputStep < requirement.Key) highestInputStep = requirement.Key;
        }

        return highestInputStep == inputStep;
    }

    public void SetFollowingLowestInputStep()
    {
        currentInputStep = GetFollowingLowestInputStep(currentInputStep, currentInputRequirements);
    }

    public override bool IsPlayer() => true;
}
