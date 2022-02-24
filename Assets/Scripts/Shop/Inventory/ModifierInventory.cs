using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierInventory : InventoryBase
{
    private List<ModifierTemplate> modifiers = new List<ModifierTemplate>();
    public void AddModifier(ModifierTemplate modifier)
    {
        AddGrabbableToEmptySocket(sockets, modifier);
        modifiers.Add(modifier);
    }
}
