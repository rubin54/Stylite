using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class EndlessGrabbableSocket : GrabbableSocket
{
    [SerializeField]
    private Image borderImage;


    override protected void OnStart()
    {
        if (grabbable)
        {
            SpellTemplate spell = ((SpellTemplate)grabbable);

            Debug.Log(spell.Type);
            borderImage.color = SpellSocket.ConvertTypeToColour(spell.Type);
            borderImage.gameObject.SetActive(true);
        }
    }


    public override void MoveContent(GrabbableSocket socket)
    {
        Grabbable grabbableCopy = Instantiate(grabbable.gameObject).GetComponent<Grabbable>();
        Grabbable retVal = socket.SetContent( grabbableCopy, socket);

        if (retVal)
        {
            socket.SetContent(retVal, socket);
            Destroy(grabbableCopy.gameObject);
        }

        OnGrabbableStoppedMoving(grabbable);
    }

    public override Grabbable SetContent(Grabbable grabbable, GrabbableSocket socket)
    {
        if(grabbable.IsEndless())
        {
            Destroy(grabbable.gameObject);
            return null;
        }
        
        return grabbable;
    }
}
