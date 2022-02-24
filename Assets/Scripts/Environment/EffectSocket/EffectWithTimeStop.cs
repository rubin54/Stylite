using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectWithTimeStop : Effect
{
    public override void OnSetup(EffectSocket socket)
    {
        socket.OnAddedEffectWithTimeStop(this);
    }
}
