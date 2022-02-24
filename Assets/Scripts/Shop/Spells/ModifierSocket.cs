using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSocket : GrabbableSocket
{
    public Action<ModifierTemplate, int> AddedModifier;
    public Action<ModifierTemplate, int> RemovedModifier;

    [SerializeField]
    private int index = 0;

    protected override void OnAddGrabbable(Grabbable grabbable)
    {
        AddedModifier?.Invoke((ModifierTemplate)grabbable, index);
    }

    protected override void OnRemovedGrabbable(Grabbable removedGrabbable)
    {
        RemovedModifier?.Invoke((ModifierTemplate)removedGrabbable, index);
    }
}
