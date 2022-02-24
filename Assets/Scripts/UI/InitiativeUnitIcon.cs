using Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Units;
using UnityEngine;

public class InitiativeUnitIcon : MonoBehaviour
{
    [SerializeField]
    private InstantClickSelectable instantClickSelectable;

    [SerializeField]
    private TextMeshProUGUI text;

    private Unit unit;

    private void Start()
    {
        instantClickSelectable.Entered += OnPointerEntered;
        instantClickSelectable.Exited += OnPointerExited;
        instantClickSelectable.Clicked += OnPointerClicked;
    }

    public void Setup(UnitEntry unit)
    {
        this.unit = unit.Unit.Unit;
        text.text = unit.init.ToString();
    }

    public void OnPointerEntered()
    {
        if (unit) unit.Cell.FocusCell();
    }

    public void OnPointerExited()
    {
        if (unit) unit.Cell.DefocusCell();
    }

    public void OnPointerClicked()
    {
        if (unit) unit.Cell.OnCellClicked();
        CamController.instance.freeCam = false;
        CamController.instance.focusTransform = unit.transform;
    }
}
