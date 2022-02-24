using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class UnitOverviewSocket : SocketCreator
{
    [SerializeField]
    private GameObject unitOverviewPrefab;

    [SerializeField]
    public GrabbableHub SpellHub;

    [SerializeField]
    public GrabbableHub ModifierHub;

    private GameObject currentOverview;

    private Unit observedUnit;

    private void Start()
    {
        CellGrid.Instance.ClickedUnit += OnClickedUnit;
    }

    public void OnClickedUnit(Unit unit)
    {
        Unit observedUnitSave = observedUnit;
        if(observedUnit)
        {
            DestroyOverview();
        }

        if(unit != observedUnitSave)
        {
            InstantiateNewOverview(unit);
        }
    }

    public void InstantiateNewOverview(Unit unit)
    {
        currentOverview = Instantiate(unitOverviewPrefab, transform);
        UnitOverview overview = currentOverview.GetComponent<UnitOverview>();

        ModifierHub.AddSocketCreatorToList(overview.ModifierSocketCreator);
        SpellHub.AddSocketCreatorToList(overview.SpellSocketCreator);

        if (overview)
        {
            overview.Setup(unit.UnitTemplate, unit);
        }
        observedUnit = unit;

        //Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
        //RectTransform rect = currentOverview.GetComponent<RectTransform>();
        //screenPos.x -= 40 + (rect.rect.width / 2);

        //currentOverview.transform.position = screenPos;
    }

    public void DestroyOverview()
    {
        currentOverview.GetComponent<UnitOverview>().Destroy();
        Destroy(currentOverview);

        ModifierHub.RemoveSocketCreatorFromList(currentOverview.GetComponent<UnitOverview>().ModifierSocketCreator);
        SpellHub.RemoveSocketCreatorFromList(currentOverview.GetComponent<UnitOverview>().ModifierSocketCreator);
        observedUnit = null;
    }
}
