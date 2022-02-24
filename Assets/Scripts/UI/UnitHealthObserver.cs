using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

public class UnitHealthObserver : MonoBehaviour
{
    [SerializeField]
    public Unit Unit;

    [SerializeField]
    public RectTransform ReferenceSize;

    [SerializeField]
    public RectTransform MovingRect;

    [SerializeField]
    public RectTransform FullConditionPosition;


    private void Start()
    {
        Unit.Died += OnUnitDeath;
    }

    void Update()
    {
        float factor = 0;
        if(Unit.HitPoints != 0)
        {
            factor = Unit.HitPoints / (float)Unit.TotalHitPoints;
        }

        MovingRect.localPosition = new Vector3(FullConditionPosition.localPosition.x - ((1f - factor) * ReferenceSize.rect.width) , FullConditionPosition.localPosition.y, FullConditionPosition.localPosition.z);
    }

    public void OnUnitDeath(Unit unit)
    {
        gameObject.SetActive(false);
    }
}
