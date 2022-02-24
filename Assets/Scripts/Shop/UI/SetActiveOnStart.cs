using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnStart : MonoBehaviour
{
    public bool ActiveState;

    void Start()
    {
        gameObject.SetActive(ActiveState);
    }
}
