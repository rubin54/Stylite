using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitEditorImageObserver : MonoBehaviour
{
    [SerializeField]
    public UnitOverview editor;

    [SerializeField]
    public RawImage image;

    private void OnEnable()
    {
        editor.AddedUnit += OnAddedUnit;

        if(editor.Unit)
        {
            OnAddedUnit();
        }
    }

    public void OnAddedUnit()
    {
        image.texture = editor.Unit.GetComponent<RawImage>().texture;
    }
}
