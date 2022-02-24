using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class CalculateConsequenceImportance
{
    public static float Calculate(Unit unit,List<Consequences> consequences)
    {
        float importance = 0;

        foreach(Consequences consequence in consequences)
        {
            importance += consequence.CalculateImportance();
        }

        return importance;
    }
}
