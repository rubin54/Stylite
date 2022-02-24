using Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class AiController : Controller
{
    public List<Hexagon> visibleTiles;

    public List<Unit> Enemies;

    public List<Unit> VisibleEnemies;

    public List<Unit> Teammates;

    int actionCounter = 0;


    Dictionary<SpellTemplate, bool> usedSpells = new Dictionary<SpellTemplate, bool>();

    public void AISetup(List<Unit> enemies)
    {
        Enemies = enemies;
        VisibleEnemies = enemies;

        foreach (var spell in Unit.Spells)
        {
            if(spell)
            {
                usedSpells.Add(spell, false);
            }
        }
    }

    public override void OnUnitDeath(Unit unit)
    {
        if (Enemies.Contains(unit))
        {
            Enemies.Remove(unit);
        }

        if (VisibleEnemies.Contains(unit))
        {
            VisibleEnemies.Remove(unit);
        }

        if (Teammates.Contains(unit))
        {
            Teammates.Remove(unit);
        }
    }

    public override void StartAction()
    {
        foreach (var spell in Unit.Spells)
        {
            if(spell)
            {
                usedSpells[spell] = false;
            }
        }

        Unit.ResetActionPoints();
        //Unit.OnTurnStart();
        actionCounter = 0;
    }

    public override void Act()
    {
        if (Unit.IsMoving) return;

        if (Unit.ActionPoints <= 0 || actionCounter > Unit.TotalActionPoints || Enemies.Count == 0)
        {
            FinishedTurn?.Invoke(this);
            return;
        }


        //Tuple ist hier unnötig komplex, Erstellung einer Klasse die das vererben dieser ermöglicht.
        //Spell instance die das Dictionary und die Spelltemplate in eine Klasse vereinen.
        (float, (SpellTemplate, Dictionary<int, InputRequirement>)) bestSpellOption = GetMostValuedOption(CalculateAllSpellOptions());

        (float, Hexagon) bestMovementOption = GetMostValuedMovementOption();

        (float, int) restingOption = CalculateRestingOption();

        if (bestMovementOption.Item1 > bestSpellOption.Item1 && bestMovementOption.Item1 > restingOption.Item1)
        {
            Hexagon hexagon = bestMovementOption.Item2;
            if (hexagon)
            {
                Unit.Move(hexagon, Unit.FindPath(cellGrid.Cells, hexagon));
            }
        }
        else if (bestSpellOption.Item1 > restingOption.Item1)
        {
            usedSpells[bestSpellOption.Item2.Item1] = true;
            bestSpellOption.Item2.Item1.Act(Unit, bestSpellOption.Item2.Item2);
        }
        else
        {
           // Unit.Heal(restingOption.Item2);
        }


        actionCounter++;
    }

    public override void OnFinishedAction(Controller controller)
    {
        Debug.Log("FINISHEDACTIOn");
        Unit.ActionPoints = -1;
        //Unit.OnTurnEnd();
    }

    public List<(float, (SpellTemplate, Dictionary<int, InputRequirement>))> CalculateAllSpellOptions()
    {
        List<(float, (SpellTemplate, Dictionary<int, InputRequirement>))> allOptions = new List<(float, (SpellTemplate, Dictionary<int, InputRequirement>))>();
        foreach (SpellTemplate spell in Unit.Spells)
        {
            if(spell != null && !usedSpells[spell])
            {
                if (spell.ApCost + (Unit.Type == spell.Type ? 0 : 1) - (spell.Type == Unit.UnitType.Universal ? 1 : 0) >= Unit.ActionPoints) continue;

                List<Dictionary<int, InputRequirement>> inputOptions = GetEveryPossibleSequence(spell.GetInputRequirements());

                foreach (var inputOption in inputOptions)
                {
                    List<Consequences> consequences = spell.Cast(Unit, inputOption);

                    float calculatedValue = 0;
                    foreach (var consequence in consequences)
                    {
                        calculatedValue += consequence.CalculateImportance();
                    }

                    //failsave if neutral
                    if (calculatedValue == 0) calculatedValue = -10;

                    allOptions.Add((calculatedValue, (spell, inputOption)));
                }
            }
        }

        return allOptions;
    }


    private (float, (SpellTemplate, Dictionary<int, InputRequirement>)) GetMostValuedOption(List<(float, (SpellTemplate, Dictionary<int, InputRequirement>))> allOptions)
    {
        (SpellTemplate, Dictionary<int, InputRequirement>) bestOption = (null, null);
        float highestImportance = int.MinValue;

        foreach(var option in allOptions)
        {
            if(option.Item1 > highestImportance)
            {
                highestImportance = option.Item1;
                bestOption = (option.Item2.Item1, InputRequirement.DuplicateAll(option.Item2.Item2));
            }
        }
        return (highestImportance, bestOption);
    }

    private List<Dictionary<int, InputRequirement>> GetEveryPossibleSequence(Dictionary<int, InputRequirement> inputRequirements)
    {
        List<Dictionary<int, InputRequirement>> inputSequences = new List<Dictionary<int, InputRequirement>>();

        int currentKey = GetLowestKey(inputRequirements, int.MinValue);
        while(currentKey != int.MaxValue)
        {
            List<InputRequirement> possibleOptions = GetEveryInputOption(inputRequirements[currentKey]);

            if(inputSequences.Count == 0)
            {
                foreach (var option in possibleOptions)
                {
                    Dictionary<int, InputRequirement> newSequence = new Dictionary<int, InputRequirement>();
                    newSequence.Add(currentKey, option);

                    inputSequences.Add(newSequence);
                }
            }
            else
            {
            }

            currentKey = GetFollowingLowestInputStep(currentKey, inputRequirements);
        }
        
        return inputSequences;
    }

    private List<InputRequirement> GetEveryInputOption(InputRequirement inputRequirements)
    {
        List<InputRequirement> inputOptions = new List<InputRequirement>();

        foreach (var hexagon in inputRequirements.GetAllOptions((Hexagon)Unit.Cell))
        {
            InputRequirement newOption = inputRequirements.Duplicate();
            newOption.Hexagon = hexagon;
            inputOptions.Add(newOption);
        }

        return inputOptions;
    }

    private int GetLowestKey(Dictionary<int, InputRequirement> inputRequirements, int minKey)
    {
        int lowestKey = int.MaxValue;

        foreach(var inputRequirement in inputRequirements)
        {
            if(inputRequirement.Key < lowestKey && inputRequirement.Key >= minKey)
            {
                lowestKey = inputRequirement.Key;
            }
        }

        return lowestKey;
    }


    //Pure Pain

    public (float, Hexagon) GetMostValuedMovementOption()
    {
        Vector3 position = ((Hexagon)Unit.Cell).CubeCoord;

        List<Hexagon> allHexagons = GetEveryHexagonInAnRadius(position, 1, 8);

        Dictionary<float, Hexagon> evaluatedOptions = EvaluateHexagons(allHexagons);

        (float, Hexagon) bestOption = (int.MinValue, null);

        foreach (var option in evaluatedOptions)
        {
            if(option.Key > bestOption.Item1)
            {
                bestOption.Item1 = option.Key;
                bestOption.Item2 = option.Value;
            }
        }

        return bestOption;
    }


    public Dictionary<float, Hexagon> EvaluateHexagons(List<Hexagon> hexagons)
    {
        Dictionary<float, Hexagon> evaluatedOptions = new Dictionary<float, Hexagon>();

        foreach (Hexagon hexagon in hexagons)
        {
            int importance = EvaluateHexagon(hexagon);
            if(!evaluatedOptions.ContainsKey(importance))
            {
                evaluatedOptions.Add(importance, hexagon);
            }
        }

        return evaluatedOptions;
    }

    public int GetMaxSpellDistance()
    {
        int maxSpellDistance = int.MinValue;

        foreach (var spell in Unit.Spells)
        {
            if (!spell) continue;

            Dictionary<int, InputRequirement> requirements = spell.GetInputRequirements();

            foreach (var requirement in requirements.Values)
            {
                InputRequirement targetRequirement = (InputRequirement)requirement;

                if (maxSpellDistance < targetRequirement.MaxRange)
                {
                    maxSpellDistance = targetRequirement.MaxRange;
                }
            }
        }
        return maxSpellDistance;
    }

    public Dictionary<Unit, int> GetDistancesFromUnits(List<Unit> units, Cell hexagon)
    {
        Dictionary<Unit, int> distances = new Dictionary<Unit, int>();

        foreach (var enemy in Enemies)
        {
            distances.Add(enemy, hexagon.GetDistance(enemy.Cell));
        }

        return distances;
    }

    public int EvaluateHexagon(Hexagon hexagon)
    {
        int importance = 0;


        Dictionary<Unit, int> currentDistances = GetDistancesFromUnits(Enemies, Unit.Cell);

        Unit nearestUnit = null;
        int distance = int.MaxValue / 2;
        foreach (var distancePair in currentDistances)
        {
            if(distancePair.Value < distance)
            {
                nearestUnit = distancePair.Key;
                distance = distancePair.Value;
            }
        }

        Dictionary<Unit, int> laterDistances = GetDistancesFromUnits(Enemies, hexagon);

        int preferredDistance = GetMaxSpellDistance();

        importance += Mathf.Abs(currentDistances[nearestUnit] - preferredDistance) <= Mathf.Abs(laterDistances[nearestUnit] - preferredDistance) ? -10 : +10 - Mathf.Abs(laterDistances[nearestUnit] - preferredDistance);

        if (importance == 0)
        {
            importance = -20;
        }

        foreach (var neighbouringHexagons in hexagon.GetNeighbours(cellGrid.Cells))
        {
            if(neighbouringHexagons.MovementCost == 99)
            {
                importance -= 1;
            }
        }

        List<Cell> path = Unit.FindPath(cellGrid.Cells, hexagon);

        foreach (var cell in path)
        {
            if (cell.OnFire) importance -= 2;
        }

        if (Unit.GetPathCost(path) > Unit.ActionPoints)
        {
            importance = int.MinValue;
        }

        if (hexagon.IsTaken)
        {
            importance = int.MinValue;
        }

        return importance;
    }



    public Vector3 GetUnitPosition(Unit unit)
    {
        return ((Hexagon)Unit.Cell).CubeCoord;
    }



    //Calculate Resting Option

    public (float, int) CalculateRestingOption()
    {
        float importance = Unit.ActionPoints;
        Dictionary<Unit, int> currentDistances = GetDistancesFromUnits(Enemies, Unit.Cell);

        Unit nearestUnit = null; 
        int distance = int.MaxValue / 2;
        foreach (var distancePair in currentDistances)
        {
            if (distancePair.Value < distance)
            {
                nearestUnit = distancePair.Key;
                distance = distancePair.Value;
            }
        }

        if (GetMaxSpellDistance() > distance)
        {
            if(nearestUnit.HitPoints < Unit.HitPoints)
            {
                importance += 3;
            }
        }
        else
        {
            importance -= 10;
        }

        return (importance, Mathf.RoundToInt(Unit.ActionPoints));
    }

    public List<Hexagon> GetEveryHexagonInAnRadius(Vector3 position, int minRadius, int maxRadius)
    {
        List<Hexagon> retVal = new List<Hexagon>();

        Vector3[] directions = Hexagon._directions;


        for (int i = minRadius; i <= maxRadius; i++)
        {
            for (int sideCount = 0; sideCount < 6; sideCount++)
            {
                for (int innerSideCount = 0; innerSideCount <= i; innerSideCount++)
                {
                    Hexagon cell = cellGrid.GetHexagon(new Vector3(
                        directions[sideCount].x * i + directions[(sideCount + 2) % 6].x * innerSideCount + position.x,
                        directions[sideCount].y * i + directions[(sideCount + 2) % 6].y * innerSideCount + position.y,
                        directions[sideCount].z * i + directions[(sideCount + 2) % 6].z * innerSideCount + position.z
                        ));

                    if (cell)
                    {
                        retVal.Add(cell);
                    }
                }
            }
        }

        return retVal;
    }

    public override bool IsEnemy() => true;
}
