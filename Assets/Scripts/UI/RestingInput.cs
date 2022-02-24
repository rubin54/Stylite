using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingInput : MonoBehaviour
{
    [SerializeField]
    public InstantClickSelectable selectable;

    public Action AttemptedResting;

    private void Start()
    {
        selectable.Clicked += AttemptResting;
    }

    public void AttemptResting()
    {
        AttemptedResting?.Invoke();
    }

    public void OnPlayerTurnStart()
    {
        gameObject.SetActive(true);
    }

    public void OnPlayerTurnEnd()
    {
        gameObject.SetActive(false);
    }
}
