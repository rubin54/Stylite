using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithObjectSize : MonoBehaviour
{
    [SerializeField]
    RectTransform rect;

    [SerializeField]
    RectTransform otherRect;

    public void MoveVertical(int direction)
    {
        rect.position += new Vector3(0, otherRect.sizeDelta.y * direction, 0);
    }
}
