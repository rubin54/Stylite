using Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class UnitDeposit
{
    private static UnitDeposit instance;

    public static UnitDeposit Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UnitDeposit();
            }

            return instance;
        }
    }

    public List<UnitTemplate> Party;


    private UnitDeposit()
    {
        Party = new List<UnitTemplate>();
    }

    //public void AddPartyMember(UnitTemplate unit)
    //{

    //}

    //public void RemovePartyMember(UnitTemplate unit)
    //{

    //}

    //public List<UnitTemplate> GetParty()
    //{
    //    List<Unit>
    //}
}


public class UnitCreator : MonoBehaviour
{
    public Action<Unit> CreatedUnit;

    [SerializeField]
    public List<GameObject> prefabs;

    [SerializeField]
    public List<Hexagon> SpawnHexagons; 

    public void Setup()
    {
        CreatedUnit += OnCreatedUnit;

        foreach (var unit in UnitDeposit.Instance.Party.ToArray())
        {
            ConstructUnit(unit);
        }
    }

    public void ConstructUnit(UnitTemplate template)
    {
        if (template)
        {
            GameObject prefab = GetRightPrefab(template.Type);
            if(prefab)
            {
                Unit unit = Instantiate(prefab, transform).GetComponent<Unit>();
                unit.Setup(template);
                CreatedUnit?.Invoke(unit);
            }
            else
            {
                Debug.LogError("Missing Prefab");
            }
        }
    }

    public void OnCreatedUnit(Unit unit)
    {
        foreach (var hexagon in SpawnHexagons)
        {
            if(hexagon.CurrentUnit == null)
            {
                unit.transform.position = hexagon.transform.position;
                unit.Cell = hexagon;
                hexagon.CurrentUnit = unit;
                hexagon.IsTaken = true;
                unit.Initialize();
                return;
            }
        }
    }

    public GameObject GetRightPrefab(Unit.UnitType unittype)
    {
        GameObject retVal = null;
        foreach (var prefab in prefabs)
        {
            if(prefab.GetComponent<Unit>().Type == unittype)
            {
                retVal = prefab;
            }
        }
        return retVal;
    }
}
