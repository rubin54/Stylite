using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShop : MonoBehaviour
{
    [SerializeField]
    RectTransform rect;

    [SerializeField]
    public float negativeAnchor;

    [SerializeField]
    public float positiveAnchor;


    public void Move(float direction)
    {
        if(direction < 0)
        {
            rect.localPosition = new Vector3(rect.localPosition.x, negativeAnchor, rect.localPosition.z);
        }
        else
        {
            rect.localPosition = new Vector3(rect.localPosition.x, positiveAnchor, rect.localPosition.z);
        }
    }
}
