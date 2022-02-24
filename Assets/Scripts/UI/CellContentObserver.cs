using Cells;
using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellContentObserver : MonoBehaviour
{
    [SerializeField]
    public ContentVisualizer contentVisualizer;

    private void Start()
    {
        CellGrid.Instance.ClickedCell += OnCellClicked;
        gameObject.SetActive(false);


        foreach (var cell in CellGrid.Instance.Cells)
        {
            cell.CellHighlighted += OnCellHighlighted;
            cell.CellDehighlighted += OnCellDehighlighted;

        }
    }

    public void OnCellClicked(Cell cell)
    {
        //gameObject.SetActive(true);
        //contentVisualizer.Visualize(cell.GetContent());
    }

    public void OnCellHighlighted(object sender, EventArgs eventArgs)
    {
        contentVisualizer.Visualize((sender as Cell).GetContent());
        contentVisualizer.gameObject.SetActive(true);
    }

    public void OnCellDehighlighted(object sender, EventArgs eventArgs)
    {
        contentVisualizer.gameObject.SetActive(false);
    }
}
