using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class SpellSocket : GrabbableSocket
{
    public Action<SpellTemplate, int> AddedSpell;
    public Action<int> RemovedSpell;

    [SerializeField]
    private int index = 0;

    [SerializeField]
    private Image borderImage;

    public int Index
    {
        get => index;
    }

    protected override void OnStart()
    {
        if (grabbable)
        {
            SpellTemplate spell = ((SpellTemplate)grabbable);

            borderImage.color = ConvertTypeToColour(spell.Type);
            borderImage.gameObject.SetActive(true);
        }
    }

    protected override void OnAddGrabbable(Grabbable grabbable)
    {
        if (grabbable)
        {
            SpellTemplate spell = ((SpellTemplate)grabbable);

            borderImage.color = ConvertTypeToColour(spell.Type);
            borderImage.gameObject.SetActive(true);
        }

        AddedSpell?.Invoke((SpellTemplate)grabbable, index);
    }

    protected override void OnRemovedGrabbable(Grabbable removedGrabbable)
    {
        borderImage.gameObject.SetActive(false);
        RemovedSpell?.Invoke(index);
    }

    public override void MoveContent(GrabbableSocket socket)
    {
        Grabbable retVal = socket.SetContent(RemoveGrabbable(), socket);
        if (retVal)
        {
            AddGrabbable(retVal);
            OnGrabbableStoppedMoving(grabbable);
        }
    }

    public override Grabbable SetContent(Grabbable grabbable, GrabbableSocket socket)
    {
        Grabbable oldGrabbable = RemoveGrabbable();
        AddGrabbable(grabbable);
        OnGrabbableStoppedMoving(grabbable);

        return oldGrabbable;
    }

    static public Color ConvertTypeToColour(Unit.UnitType type)
    {
        Color retval = Color.black;

        switch (type)
        {
            case Unit.UnitType.Melee:
                retval = new Color(125f / 255f, 36f / 255f, 52f / 255f);
                break;
            case Unit.UnitType.Range:
                retval = new Color(47f / 255f, 75f / 255f, 89f / 255f);
                break;
            case Unit.UnitType.Support:
                retval = new Color(65f / 255f, 84f / 255f, 44f / 255f);
                break;
            case Unit.UnitType.Universal:
                retval = new Color(110f / 255f, 23f/255f, 115f/255f);
                break;
            default:
                break;
        }

        return retval;
    }

}
