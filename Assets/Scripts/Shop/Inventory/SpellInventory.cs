using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellInventory : InventoryBase
{
    private List<SpellTemplate> spells = new List<SpellTemplate>();

    protected List<GrabbableSocket> modiferSockets;

    public void AddSpell(SpellTemplate spell)
    {
        AddGrabbableToEmptySocket(sockets, spell);
        spells.Add(spell);
    }
}
