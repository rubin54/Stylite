using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEditorHub : MonoBehaviour
{
    [SerializeField]
    private UnitInventory unitInventory;

    [SerializeField]
    private GameObject unitEditorPrefab;

    [SerializeField]
    private SocketCreator modifierCreator;

    [SerializeField]
    private SocketCreator spellCreator;

    [SerializeField]
    private Transform socket;

    [SerializeField]
    public List<SpellTemplate> initSpells1;

    [SerializeField]
    public List<ModifierTemplate> initModifiers1;

    [SerializeField]
    public List<SpellTemplate> initSpells2;

    [SerializeField]
    public List<ModifierTemplate> initModifiers2;

    [SerializeField]
    public List<SpellTemplate> initSpells3;

    [SerializeField]
    public List<ModifierTemplate> initModifiers3;

    private List<UnitEditor> units = new List<UnitEditor>();

    int currentUnit = 0;



    private void Awake()
    {
        unitInventory.ChangedUnits += OnChangedUnits;
    }

    public void OnChangedUnits(List<UnitTemplate> unitTemplates)
    {
        //Check if Units still exist
        foreach (var template in units.ToArray())
        {
            if(!unitTemplates.Contains(template.Unit))
            {
                units.Remove(template);
                template.Destroy();
            }
        }

        //Create new Units
        foreach (var unitTemplate in unitTemplates)
        {
            bool contains = false;
            foreach (var unit in units.ToArray())
            {
                if (unitTemplate == unit.Unit) contains = true;
            }

            if(!contains)
            {
                GameObject createdObject = Instantiate(unitEditorPrefab, socket);

                UnitEditor editor = createdObject.GetComponent<UnitEditor>();
                if(editor)
                {
                    units.Add(editor);
                    spellCreator.CreatedCreator?.Invoke(editor.SpellSocketCreator);
                    modifierCreator.CreatedCreator?.Invoke(editor.ModifierSocketCreator);
                    editor.Setup(unitTemplate, null);
                }
                else
                {
                    Debug.LogError("Created UnitEditorPrefab doesnt contain a UnitEditor Script");
                    Destroy(createdObject);
                }
            }
        }

        if(currentUnit == 0)
        {
            UnitEditor unit = units[0];
            {
                if (initSpells1.Count > 0)
                {
                    foreach (var spell in initSpells1)
                    {
                        unit.AddSpell(spell);
                    }

                    foreach (var modifier in initModifiers1)
                    {
                        unit.AddModifier(modifier);
                    }

                    initSpells1.RemoveAt(0);
                }
            }
        }
        else if
        (currentUnit == 1)
        {
            UnitEditor unit = units[1];
            {
                if (initSpells2.Count > 0)
                {
                    foreach (var spell in initSpells2)
                    {
                        unit.AddSpell(spell);
                    }

                    foreach (var modifier in initModifiers2)
                    {
                        unit.AddModifier(modifier);
                    }

                    initSpells2.RemoveAt(0);
                }
            }
        }
        else if
        (currentUnit == 2)
        {
            UnitEditor unit = units[2];
            {
                if (initSpells3.Count > 0)
                {
                    foreach (var spell in initSpells3)
                    {
                        unit.AddSpell(spell);
                    }

                    foreach (var modifier in initModifiers3)
                    {
                        unit.AddModifier(modifier);
                    }

                    initSpells3.RemoveAt(0);
                }
            }
        }

        currentUnit++;
    }

}
