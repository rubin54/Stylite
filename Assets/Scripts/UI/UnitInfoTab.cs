using System.Collections;
using System.Collections.Generic;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoTab : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> actionPointVisualizers;

    [SerializeField]
    private RawImage unitSocket;

    [SerializeField]
    private RectTransform healthRect;

    [SerializeField]
    private TextMeshProUGUI textName;

    [SerializeField]
    private TextMeshProUGUI textClass;

    private Unit unit;

    private float healthWidth;

    private Vector3 startingHealthRectPosition;

    private void Start()
    {
        healthWidth = healthRect.rect.width;
        startingHealthRectPosition = healthRect.localPosition;
    }

    private void Update()
    {
        if(unit && unit.UnitTemplate)
        {
            for (int i = 0; i < actionPointVisualizers.Count; i++)
            {
                actionPointVisualizers[i].SetActive(i < unit.ActionPoints);
            }

            healthRect.localPosition = new Vector3(startingHealthRectPosition.x + (((float)unit.HitPoints / (float)unit.TotalHitPoints) * healthWidth - healthWidth), startingHealthRectPosition.y, startingHealthRectPosition.z);

            unitSocket.texture = unit.UnitTemplate.GetComponent<RawImage>().texture;
            textName.text = unit.UnitTemplate.Name;
            textClass.text = UnitTemplate.GetClassAsString(unit.UnitTemplate);
        }
    }

    public void SetNewPlayer(Unit unit)
    {
        this.unit = unit;
    }
}
