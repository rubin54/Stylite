using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //This class doesnt make any sense and i dont know why it even exists.
    //This class should be like 100 lines longer, just add needed funtions in the last 3 days.

    [SerializeField]
    private SpellInventory spellInventory;

    [SerializeField]
    private UnitInventory unitInventory;

    [SerializeField]
    private ModifierInventory modifierInventory;

    public UnitInventory UnitInventory
    {
        get => unitInventory;
    }
    public SpellInventory SpellInventory
    {
        get => spellInventory;
    }

    public void AddSpell(GameObject gameObject)
    {
        spellInventory.AddSpell(gameObject.GetComponent<SpellTemplate>());
    }

    public void AddUnit(GameObject gameObject)
    {
        unitInventory.AddUnit(gameObject.GetComponent<UnitTemplate>());
    }

    public void AddModifier(GameObject gameObject)
    {
        modifierInventory.AddModifier(gameObject.GetComponent<ModifierTemplate>());
    }
}
