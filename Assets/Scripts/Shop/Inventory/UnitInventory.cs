using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : InventoryBase
{
    public Action ReachedLimit;
    public Action Opened;
    public Action Reopened;
    public Action<List<UnitTemplate>> ChangedUnits;

    [SerializeField]
    public int Limit = 5;

    private int lastUnitInventoryContentQuantity = 0;
    private List<UnitTemplate> units = new List<UnitTemplate>();

    public List<UnitTemplate> Units
    {
        get => units;
    }

    private void Start()
    {
        foreach (var socket in sockets)
        {
            if(socket.grabbable)
            {
                AddUnitToParty((UnitTemplate)socket.grabbable);
            }
        }

        ChangedUnits += CheckForLimitation;
    }

    public void AddUnit(UnitTemplate unit)
    {
        AddGrabbableToEmptySocket(sockets, unit);
        AddUnitToParty(unit);
    }

    public void AddUnitToParty(UnitTemplate unit)
    {
        UnitDeposit.Instance.Party.Add(unit);
        units.Add(unit);
        unit.Died += RemoveUnit;
        ChangedUnits?.Invoke(units);
    }

    public void RemoveUnit(UnitTemplate unit)
    {
        UnitDeposit.Instance.Party.Remove(unit);
        units.Remove(unit);
        unit.Destroy();
        ChangedUnits?.Invoke(units);
    }

    private void CheckForLimitation(List<UnitTemplate> units)
    {
        if(IsFull())
        {
            ReachedLimit?.Invoke();
        }

        if (!IsFull())
        {
            Opened?.Invoke();
        }

        if (lastUnitInventoryContentQuantity == Limit && units.Count < Limit)
        {
            Reopened?.Invoke();
        }

        lastUnitInventoryContentQuantity = units.Count;

        if (units.Count > Limit)
        {
            Debug.LogError("SOMETHING WENT AWFULLY WRING");
        }
    }

    public bool IsFull()
    {
        return units.Count == Limit;
    }
}
