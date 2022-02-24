using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectWithScreenSize : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    [SerializeField]
    private float speed = 10;

    private Vector3 wantedPosition;

    private void Start()
    {
        wantedPosition = rect.position;
    }

    public void Move(float direction)
    {
        if (wantedPosition != rect.position) return;
        wantedPosition = rect.position + new Vector3(Screen.width * direction, 0, 0);
        
    }

    private void Update()
    {
        if((wantedPosition - rect.position).magnitude < 4)
        {
            rect.position = wantedPosition;
        }
        else
        {
            rect.position += (wantedPosition - rect.position)*speed*Time.deltaTime;
        }
    }
}
