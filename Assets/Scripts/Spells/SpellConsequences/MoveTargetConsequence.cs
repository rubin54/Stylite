using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class MoveTargetConsequence : Consequences
{
    public readonly int DamagePerForce = 2;

    public Unit Caster;
    public Unit Target;
    public Vector3 Direction;

    public int Force;


    protected override ConsequenceType GetConsequenceType() => ConsequenceType.MoveConsequence;

    public override float CalculateImportance()
    {
        float importance = 0;

        float factor = Caster == Target ? -5 : 10;
        if (Caster.Teammates.Contains(Target) && Target != Caster) factor *= -1;


        List<Vector3> directionsPerForce = DisectDirection(Direction);
        Vector3 finalPosition = ((Hexagon)Target.Cell).CubeCoord;

        int remainingForce = Force;

        for (int i = 0; i < Force; i++)
        {
            Vector3 calculatedPosition = finalPosition + directionsPerForce[i % directionsPerForce.Count];

            Hexagon calculatedHexagon = CellGrid.Instance.GetHexagon(calculatedPosition);

            if (calculatedHexagon == null) break;

            if (calculatedHexagon.IsTaken)
            {
                if (calculatedHexagon.CurrentUnit && calculatedHexagon.CurrentUnit != Caster)
                {
                    bool isCurrentUnitFriendly = Caster.Teammates.Contains(calculatedHexagon.CurrentUnit);
                    importance += remainingForce * DamagePerForce * 3 * (isCurrentUnitFriendly ? -1 : 1) ;
                }
                break;
            }

            importance += calculatedHexagon.DamageByForcedTraversal * 3; 

            finalPosition = calculatedPosition;
            remainingForce--;
        }

        Hexagon finalHexagon = CellGrid.Instance.GetHexagon(finalPosition);

        importance += remainingForce * DamagePerForce;

        if (finalHexagon)
        {
            if(finalHexagon.MovementCost > 5)
            {
                importance = int.MaxValue;
            }
        }


        importance = Force * factor;

        return importance;
    }

    protected override void Act(List<Hexagon> hexagons)
    {

        List<Cell> path = new List<Cell>();
        List<Vector3> directionsPerForce = DisectDirection(Direction);
        Vector3 finalPosition = ((Hexagon)Target.Cell).CubeCoord;

        int remainingForce = Force;

        for (int i = 0; i < Force; i++)
        {
            Vector3 calculatedPosition = finalPosition + directionsPerForce[i % directionsPerForce.Count];

            Hexagon calculatedHexagon = CellGrid.Instance.GetHexagon(calculatedPosition);

            if (calculatedHexagon == null) break;

            if(calculatedHexagon.IsTaken)
            {
                if(calculatedHexagon.CurrentUnit)
                {
                    
                    calculatedHexagon.CurrentUnit.ReceiveDamage(remainingForce * DamagePerForce);
                }

                break;
            }

            if(calculatedHexagon.DamageByForcedTraversal != 0) Target.ReceiveDamage(calculatedHexagon.DamageByForcedTraversal);

            path.Add(calculatedHexagon);

            finalPosition = calculatedPosition;
            remainingForce--;
        }

        Hexagon finalHexagon = CellGrid.Instance.GetHexagon(finalPosition);
        if (remainingForce * DamagePerForce != 0) Target.ReceiveDamage(remainingForce * DamagePerForce);

        if(finalHexagon)
        {
            path.Reverse();
            Target.ForceMovement(finalHexagon, path);
        }
    }


    //TODO: MIXING DIRECTIONS INTO AN MORE NATURAL ORDER

    public List<Vector3> DisectDirection(Vector3 direction)
    {
        List<Vector3> directions = new List<Vector3>();


        Dictionary<Vector3, int> howOftenDoesSomethingFit = new Dictionary<Vector3, int>();


        foreach (var simpleDirection in Hexagon._directions)
        {
            bool compatible = direction.x * simpleDirection.x >= 0 &&
                                direction.y * simpleDirection.y >= 0 &&
                                direction.z * simpleDirection.z >= 0;

            if (!compatible) continue;

            int lowestAbsoluteNumber = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                if (simpleDirection[i].Equals(0)) continue;

                if(lowestAbsoluteNumber > (Direction[i] / simpleDirection[i]))
                {
                    lowestAbsoluteNumber = Mathf.RoundToInt(Direction[i] / simpleDirection[i]);
                }
            }

            howOftenDoesSomethingFit.Add(simpleDirection, lowestAbsoluteNumber);
        }


        foreach (var directionSize in howOftenDoesSomethingFit)
        {
            for (int i = 0; i < directionSize.Value; i++)
            {
                directions.Add(directionSize.Key);
            }
        }


        if(directions.Count == 0)
        {
            directions.Add(new Vector3(0, 0, 0));
        }

        return directions;
    }
}
