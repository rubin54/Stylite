using Cells;
using Grid;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class AiSpawnerWithRandomizedContent : MonoBehaviour
{
    [SerializeField]
    private List<Hexagon> spawnHexagons;

    [SerializeField]
    private List<UnitTemplate> unitTemplates;

    [SerializeField]
    private List<GameObject> unitPrefabs;

    [SerializeField]
    private List<SpellTemplate> spellTemplates;

    [SerializeField]
    private List<ModifierTemplate> modifierTemplates;
    
    [SerializeField]
    private float modifierChance;

    [SerializeField]
    private Transform socket;

    public List<AiController> Spawn()
    {
        List<AiController> aiControllers = new List<AiController>();

        foreach (var hexagon in spawnHexagons)
        {
            UnitTemplate unitTemplate = GetRandomClass();


            GameObject unitObject = GetUnit(unitTemplate.Type);

            unitTemplate.gameObject.transform.SetParent(unitObject.transform);
            for (int i = 0; i < unitTemplate.Spells.Length; i++)
            {
                SpellTemplate spell = GetRandomSpell();
                spell.gameObject.transform.SetParent(unitObject.transform);
                unitTemplate.SetSpell(spell, i);

                if (Random.Range(0f, 1f) < modifierChance)
                {
                    ModifierTemplate modifier = GetRandomModifier();
                    modifier.gameObject.transform.SetParent(unitObject.transform);
                    unitTemplate.AddModifier(modifier, i);
                }
            }

            Unit unit = unitObject.GetComponent<Unit>();

            unit.Cell = hexagon;
            hexagon.IsTaken = true;
            hexagon.CurrentUnit = unit;
            unitObject.transform.position = hexagon.transform.position;

            unit.Setup(unitTemplate);
            unit.Initialize();
            CellGrid.Instance.AddUnit(unit);

            aiControllers.Add(unit.GetComponent<AiController>());
        }

        return aiControllers;
    }


    public ModifierTemplate GetRandomModifier() => Instantiate(modifierTemplates[Mathf.RoundToInt(Random.Range(0, modifierTemplates.Count))]);
    public SpellTemplate GetRandomSpell() => Instantiate(spellTemplates[Mathf.RoundToInt(Random.Range(0, spellTemplates.Count))]);
    public UnitTemplate GetRandomClass() => Instantiate(unitTemplates[Mathf.RoundToInt(Random.Range(0, unitTemplates.Count))]);

    public GameObject GetUnit(Unit.UnitType type)
    {
        foreach (var prefab in unitPrefabs)
        {
            Unit unit = prefab.GetComponent<Unit>();
            if(type == unit.Type)
            {
                return Instantiate(prefab, socket);
            }
        }

        return null;
    }
}
