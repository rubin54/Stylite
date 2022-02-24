using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierTemplate : Grabbable
{
    [SerializeField]
    public string Name = "Basic";

    [SerializeField]
    public SpellComponent SpellComponent;

    public ModifierTemplate Duplicate()
    {
        GameObject duplicate = Instantiate(gameObject);
        ModifierTemplate retVal = duplicate.GetComponent<ModifierTemplate>();
        retVal.Name = Name;
        retVal.SpellComponent = SpellComponent;
        return retVal;
    }

    public override Content GetContent()
    {
        return new Content(Name, bodyDescription);
    }
}
