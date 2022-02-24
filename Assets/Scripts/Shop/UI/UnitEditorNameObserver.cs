using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitEditorNameObserver : MonoBehaviour
{
    [SerializeField]
    public UnitOverview editor;

    [SerializeField]
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        editor.AddedUnit += OnAddedUnit;

        if (editor.Unit)
        {
            OnAddedUnit();
        }
    }

    public void OnAddedUnit()
    {
        text.text = editor.Unit.Name;

    }
}
